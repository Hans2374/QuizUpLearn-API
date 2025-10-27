using BusinessLogic.DTOs;
using Repository.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Services
{
    /// <summary>
    /// Service quản lý Kahoot-style realtime quiz game
    /// State được lưu trong Memory (ConcurrentDictionary) 
    /// Trong production nên dùng Redis
    /// </summary>
    public class RealtimeGameService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RealtimeGameService> _logger;

        // In-memory storage (thay bằng Redis trong production)
        private static readonly ConcurrentDictionary<string, GameSessionDto> _gameSessions = new(); // GamePin -> Session
        private static readonly ConcurrentDictionary<string, Timer> _questionTimers = new(); // GamePin -> Timer
        private static readonly ConcurrentDictionary<string, Dictionary<Guid, bool>> _correctAnswers = new(); // GamePin -> QuestionId -> IsCorrect map

        public RealtimeGameService(
            IServiceProvider serviceProvider,
            ILogger<RealtimeGameService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        // ==================== HOST CREATES GAME ====================
        /// <summary>
        /// Host tạo game session và nhận Game PIN
        /// </summary>
        public async Task<CreateGameResponseDto> CreateGameAsync(CreateGameDto dto)
        {
            // Tạo scope để resolve scoped services
            using var scope = _serviceProvider.CreateScope();
            var quizSetRepository = scope.ServiceProvider.GetRequiredService<IQuizSetRepo>();
            var quizRepository = scope.ServiceProvider.GetRequiredService<IQuizRepo>();
            var answerOptionRepository = scope.ServiceProvider.GetRequiredService<IAnswerOptionRepo>();

            // Validate quiz set exists
            var quizSet = await quizSetRepository.GetQuizSetByIdAsync(dto.QuizSetId);
            if (quizSet == null)
                throw new ArgumentException("Quiz set not found");

            // Load questions
            var quizzes = await quizRepository.GetQuizzesByQuizSetIdAsync(dto.QuizSetId);
            var questionsList = new List<QuestionDto>();
            var correctAnswersMap = new Dictionary<Guid, bool>();

            int questionNumber = 1;
            foreach (var quiz in quizzes)
            {
                var answerOptions = await answerOptionRepository.GetByQuizIdAsync(quiz.Id);
                
                var questionDto = new QuestionDto
                {
                    QuestionId = quiz.Id,
                    QuestionText = quiz.QuestionText,
                    ImageUrl = quiz.ImageURL,
                    AudioUrl = quiz.AudioURL,
                    QuestionNumber = questionNumber,
                    TotalQuestions = quizzes.Count(),
                    TimeLimit = dto.TimePerQuestion,
                    AnswerOptions = answerOptions.Select(ao => new AnswerOptionDto
                    {
                        AnswerId = ao.Id,
                        OptionText = ao.OptionText
                        // Không gửi IsCorrect cho client!
                    }).ToList()
                };

                questionsList.Add(questionDto);

                // Lưu đáp án đúng vào map riêng
                foreach (var ao in answerOptions)
                {
                    correctAnswersMap[ao.Id] = ao.IsCorrect;
                }

                questionNumber++;
            }

            // Generate unique 6-digit Game PIN
            string gamePin;
            do
            {
                gamePin = new Random().Next(100000, 999999).ToString();
            } while (_gameSessions.ContainsKey(gamePin));

            var gameSessionId = Guid.NewGuid();

            // Create game session
            var gameSession = new GameSessionDto
            {
                GamePin = gamePin,
                GameSessionId = gameSessionId,
                HostUserId = dto.HostUserId,
                HostUserName = dto.HostUserName,
                QuizSetId = dto.QuizSetId,
                TimePerQuestion = dto.TimePerQuestion,
                Status = GameStatus.Lobby,
                Questions = questionsList,
                CurrentQuestionIndex = 0,
                CreatedAt = DateTime.UtcNow
            };

            _gameSessions[gamePin] = gameSession;
            _correctAnswers[gamePin] = correctAnswersMap;

            _logger.LogInformation($"Game created with PIN: {gamePin} by Host: {dto.HostUserName}");

            return new CreateGameResponseDto
            {
                GamePin = gamePin,
                GameSessionId = gameSessionId,
                CreatedAt = gameSession.CreatedAt
            };
        }

        // ==================== LOBBY ====================
        /// <summary>
        /// Host kết nối vào game session (sau khi tạo)
        /// </summary>
        public async Task<bool> HostConnectAsync(string gamePin, string connectionId)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return false;

            session.HostConnectionId = connectionId;
            _logger.LogInformation($"Host connected to game {gamePin}");
            return true;
        }

        /// <summary>
        /// Player join vào lobby bằng Game PIN
        /// </summary>
        public async Task<PlayerInfo?> PlayerJoinAsync(string gamePin, string playerName, string connectionId)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            if (session.Status != GameStatus.Lobby)
                return null; // Chỉ join được khi đang ở lobby

            // Check duplicate name
            if (session.Players.Any(p => p.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
                return null;

            var player = new PlayerInfo
            {
                ConnectionId = connectionId,
                PlayerName = playerName,
                Score = 0,
                JoinedAt = DateTime.UtcNow
            };

            session.Players.Add(player);
            _logger.LogInformation($"Player '{playerName}' joined game {gamePin}. Total players: {session.Players.Count}");

            return player;
        }

        /// <summary>
        /// Player rời lobby
        /// </summary>
        public async Task<bool> PlayerLeaveAsync(string gamePin, string connectionId)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return false;

            var player = session.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null)
                return false;

            session.Players.Remove(player);
            _logger.LogInformation($"Player '{player.PlayerName}' left game {gamePin}");

            return true;
        }

        /// <summary>
        /// Lấy thông tin lobby (số người chơi, v.v.)
        /// </summary>
        public async Task<GameSessionDto?> GetGameSessionAsync(string gamePin)
        {
            _gameSessions.TryGetValue(gamePin, out var session);
            return session;
        }

        // ==================== START GAME ====================
        /// <summary>
        /// Host start game - chuyển sang câu hỏi đầu tiên
        /// </summary>
        public async Task<QuestionDto?> StartGameAsync(string gamePin, Action<string> onQuestionTimeout)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            if (session.Status != GameStatus.Lobby)
                return null;

            if (session.Players.Count == 0)
                return null; // Cần ít nhất 1 player

            session.Status = GameStatus.InProgress;
            session.CurrentQuestionIndex = 0;
            session.QuestionStartedAt = DateTime.UtcNow;
            session.CurrentAnswers.Clear();

            var question = session.Questions[0];

            // Start timer trên server
            StartQuestionTimer(gamePin, session.TimePerQuestion, onQuestionTimeout);

            _logger.LogInformation($"Game {gamePin} started with {session.Players.Count} players");

            return question;
        }

        // ==================== SUBMIT ANSWER ====================
        /// <summary>
        /// Player submit câu trả lời
        /// </summary>
        public async Task<bool> SubmitAnswerAsync(string gamePin, string connectionId, Guid questionId, Guid answerId)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return false;

            if (session.Status != GameStatus.InProgress)
                return false; // Chỉ submit được khi đang InProgress

            if (!session.QuestionStartedAt.HasValue)
                return false;

            var player = session.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null)
                return false;

            // Check nếu đã submit rồi
            if (session.CurrentAnswers.ContainsKey(connectionId))
                return false; // Không cho submit lại

            // Check thời gian
            var timeSpent = (DateTime.UtcNow - session.QuestionStartedAt.Value).TotalSeconds;
            if (timeSpent > session.TimePerQuestion)
                return false; // Hết giờ rồi

            // Check đáp án đúng
            bool isCorrect = false;
            if (_correctAnswers.TryGetValue(gamePin, out var correctMap))
            {
                isCorrect = correctMap.GetValueOrDefault(answerId, false);
            }

            // Tính điểm: 1000 điểm cơ bản + bonus theo thời gian
            // Nếu trả lời nhanh hơn = điểm cao hơn (Kahoot style)
            int points = 0;
            if (isCorrect)
            {
                double timeRatio = 1.0 - (timeSpent / session.TimePerQuestion);
                points = (int)(1000 + (timeRatio * 500)); // Tối đa 1500 điểm
            }

            var answer = new PlayerAnswer
            {
                ConnectionId = connectionId,
                PlayerName = player.PlayerName,
                QuestionId = questionId,
                AnswerId = answerId,
                TimeSpent = timeSpent,
                IsCorrect = isCorrect,
                PointsEarned = points,
                SubmittedAt = DateTime.UtcNow
            };

            session.CurrentAnswers[connectionId] = answer;

            // Cập nhật điểm của player
            player.Score += points;

            _logger.LogInformation($"Player '{player.PlayerName}' submitted answer for question {questionId}. Correct: {isCorrect}, Points: {points}");

            return true;
        }

        // ==================== QUESTION TIMEOUT & SHOW RESULT ====================
        /// <summary>
        /// Xử lý khi hết giờ - trả về kết quả câu hỏi
        /// </summary>
        public async Task<GameAnswerResultDto?> GetQuestionResultAsync(string gamePin)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            var currentQuestion = session.Questions[session.CurrentQuestionIndex];

            // Tìm đáp án đúng
            Guid correctAnswerId = Guid.Empty;
            string correctAnswerText = string.Empty;

            if (_correctAnswers.TryGetValue(gamePin, out var correctMap))
            {
                foreach (var option in currentQuestion.AnswerOptions)
                {
                    if (correctMap.GetValueOrDefault(option.AnswerId, false))
                    {
                        correctAnswerId = option.AnswerId;
                        correctAnswerText = option.OptionText;
                        break;
                    }
                }
            }

            // Thống kê đáp án
            var answerStats = new Dictionary<Guid, int>();
            foreach (var option in currentQuestion.AnswerOptions)
            {
                answerStats[option.AnswerId] = 0;
            }

            var playerResults = new List<PlayerAnswerResult>();

            foreach (var answer in session.CurrentAnswers.Values)
            {
                if (answerStats.ContainsKey(answer.AnswerId))
                {
                    answerStats[answer.AnswerId]++;
                }

                playerResults.Add(new PlayerAnswerResult
                {
                    PlayerName = answer.PlayerName,
                    IsCorrect = answer.IsCorrect,
                    PointsEarned = answer.PointsEarned,
                    TimeSpent = answer.TimeSpent
                });
            }

            session.Status = GameStatus.ShowingResult;

            return new GameAnswerResultDto
            {
                QuestionId = currentQuestion.QuestionId,
                CorrectAnswerId = correctAnswerId,
                CorrectAnswerText = correctAnswerText,
                AnswerStats = answerStats,
                PlayerResults = playerResults
            };
        }

        // ==================== LEADERBOARD & NEXT QUESTION ====================
        /// <summary>
        /// Lấy leaderboard hiện tại
        /// </summary>
        public async Task<LeaderboardDto?> GetLeaderboardAsync(string gamePin)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            var rankings = session.Players
                .OrderByDescending(p => p.Score)
                .Select((p, index) => new PlayerScore
                {
                    PlayerName = p.PlayerName,
                    TotalScore = p.Score,
                    CorrectAnswers = 0, // TODO: Track this
                    Rank = index + 1
                })
                .ToList();

            session.Status = GameStatus.ShowingLeaderboard;

            return new LeaderboardDto
            {
                Rankings = rankings,
                CurrentQuestion = session.CurrentQuestionIndex + 1,
                TotalQuestions = session.Questions.Count
            };
        }

        /// <summary>
        /// Host chuyển sang câu hỏi tiếp theo
        /// </summary>
        public async Task<QuestionDto?> NextQuestionAsync(string gamePin, Action<string> onQuestionTimeout)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            session.CurrentQuestionIndex++;

            // Check nếu hết câu hỏi
            if (session.CurrentQuestionIndex >= session.Questions.Count)
            {
                session.Status = GameStatus.Completed;
                return null; // Hết câu hỏi
            }

            session.Status = GameStatus.InProgress;
            session.QuestionStartedAt = DateTime.UtcNow;
            session.CurrentAnswers.Clear();

            var question = session.Questions[session.CurrentQuestionIndex];

            // Start timer trên server
            StartQuestionTimer(gamePin, session.TimePerQuestion, onQuestionTimeout);

            _logger.LogInformation($"Game {gamePin} moved to question {session.CurrentQuestionIndex + 1}");

            return question;
        }

        // ==================== GAME END ====================
        /// <summary>
        /// Lấy kết quả cuối cùng khi game kết thúc
        /// </summary>
        public async Task<FinalResultDto?> GetFinalResultAsync(string gamePin)
        {
            if (!_gameSessions.TryGetValue(gamePin, out var session))
                return null;

            if (session.Status != GameStatus.Completed)
                return null;

            var rankings = session.Players
                .OrderByDescending(p => p.Score)
                .Select((p, index) => new PlayerScore
                {
                    PlayerName = p.PlayerName,
                    TotalScore = p.Score,
                    CorrectAnswers = 0, // TODO: Track this
                    Rank = index + 1
                })
                .ToList();

            var winner = rankings.FirstOrDefault();

            return new FinalResultDto
            {
                GamePin = gamePin,
                FinalRankings = rankings,
                Winner = winner,
                CompletedAt = DateTime.UtcNow,
                TotalPlayers = session.Players.Count,
                TotalQuestions = session.Questions.Count
            };
        }

        /// <summary>
        /// Cleanup game session sau khi kết thúc
        /// </summary>
        public async Task CleanupGameAsync(string gamePin)
        {
            // Stop timer nếu đang chạy
            if (_questionTimers.TryRemove(gamePin, out var timer))
            {
                timer.Dispose();
            }

            _gameSessions.TryRemove(gamePin, out _);
            _correctAnswers.TryRemove(gamePin, out _);

            _logger.LogInformation($"Game {gamePin} cleaned up");
        }

        // ==================== TIMER MANAGEMENT ====================
        /// <summary>
        /// Start timer trên server - SOURCE OF TRUTH
        /// </summary>
        private void StartQuestionTimer(string gamePin, int seconds, Action<string> onTimeout)
        {
            // Stop existing timer nếu có
            if (_questionTimers.TryRemove(gamePin, out var existingTimer))
            {
                existingTimer.Dispose();
            }

            // Create new timer
            var timer = new Timer(_ =>
            {
                _logger.LogInformation($"Question timer expired for game {gamePin}");
                onTimeout(gamePin);

                // Dispose timer sau khi chạy
                if (_questionTimers.TryRemove(gamePin, out var t))
                {
                    t.Dispose();
                }
            }, null, TimeSpan.FromSeconds(seconds), Timeout.InfiniteTimeSpan);

            _questionTimers[gamePin] = timer;
        }

        // ==================== CONNECTION MANAGEMENT ====================
        /// <summary>
        /// Xử lý khi player disconnect
        /// </summary>
        public async Task<PlayerInfo?> HandleDisconnectAsync(string connectionId)
        {
            // Tìm player trong tất cả các game
            foreach (var session in _gameSessions.Values)
            {
                var player = session.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (player != null)
                {
                    session.Players.Remove(player);
                    _logger.LogInformation($"Player '{player.PlayerName}' disconnected from game {session.GamePin}");
                    return player;
                }
            }

            return null;
        }

        /// <summary>
        /// Tìm game PIN từ connection ID
        /// </summary>
        public async Task<string?> GetGamePinByConnectionAsync(string connectionId)
        {
            foreach (var session in _gameSessions.Values)
            {
                if (session.HostConnectionId == connectionId || 
                    session.Players.Any(p => p.ConnectionId == connectionId))
                {
                    return session.GamePin;
                }
            }

            return null;
        }
    }
}
