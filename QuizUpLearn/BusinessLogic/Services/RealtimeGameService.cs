using BusinessLogic.DTOs;
using Repository.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BusinessLogic.Services
{
    /// <summary>
    /// Service quản lý Kahoot-style realtime quiz game
    /// State được lưu trong Redis (Distributed Cache) 
    /// Hỗ trợ scale-out multiple servers
    /// </summary>
    public class RealtimeGameService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache; // ✨ Redis Cache
        private readonly ILogger<RealtimeGameService> _logger;

        // ⏱️ Timers vẫn giữ in-memory (không thể serialize Timer vào Redis)
        private static readonly ConcurrentDictionary<string, Timer> _questionTimers = new();

        public RealtimeGameService(
            IServiceProvider serviceProvider,
            IDistributedCache cache, // ✨ Inject Redis
            ILogger<RealtimeGameService> logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache; // ✨ Redis
            _logger = logger;
        }

        // ==================== REDIS HELPER METHODS ====================
        private async Task<GameSessionDto?> GetGameSessionFromRedisAsync(string gamePin)
        {
            try
            {
                var json = await _cache.GetStringAsync($"game:{gamePin}");
                if (string.IsNullOrEmpty(json)) return null;
                
                return JsonSerializer.Deserialize<GameSessionDto>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting game session {gamePin} from Redis");
                return null;
            }
        }

        private async Task SaveGameSessionToRedisAsync(string gamePin, GameSessionDto session)
        {
            try
            {
                var json = JsonSerializer.Serialize(session);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                };
                
                await _cache.SetStringAsync($"game:{gamePin}", json, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving game session {gamePin} to Redis");
                throw;
            }
        }

        private async Task<Dictionary<Guid, bool>?> GetCorrectAnswersFromRedisAsync(string gamePin)
        {
            try
            {
                var json = await _cache.GetStringAsync($"answers:{gamePin}");
                if (string.IsNullOrEmpty(json)) return null;
                
                return JsonSerializer.Deserialize<Dictionary<Guid, bool>>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting correct answers for {gamePin} from Redis");
                return null;
            }
        }

        private async Task SaveCorrectAnswersToRedisAsync(string gamePin, Dictionary<Guid, bool> answers)
        {
            try
            {
                var json = JsonSerializer.Serialize(answers);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                };
                
                await _cache.SetStringAsync($"answers:{gamePin}", json, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving correct answers for {gamePin} to Redis");
                throw;
            }
        }

        private async Task DeleteGameFromRedisAsync(string gamePin)
        {
            try
            {
                await _cache.RemoveAsync($"game:{gamePin}");
                await _cache.RemoveAsync($"answers:{gamePin}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting game {gamePin} from Redis");
            }
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
                var existingGame = await GetGameSessionFromRedisAsync(gamePin);
                if (existingGame == null) break; // PIN is unique
            } while (true);

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

            await SaveGameSessionToRedisAsync(gamePin, gameSession);
            await SaveCorrectAnswersToRedisAsync(gamePin, correctAnswersMap);

            _logger.LogInformation($"✅ Game created in Redis with PIN: {gamePin} by Host: {dto.HostUserName}");

            return new CreateGameResponseDto
            {
                GamePin = gamePin,
                GameSessionId = gameSessionId,
                CreatedAt = gameSession.CreatedAt
            };
        }

        public async Task<bool> HostConnectAsync(string gamePin, string connectionId)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
                return false;

            session.HostConnectionId = connectionId;
            await SaveGameSessionToRedisAsync(gamePin, session);
            
            _logger.LogInformation($"✅ Host connected to game {gamePin}");
            return true;
        }

        // ==================== PLAYER JOIN/LEAVE ====================
        /// <summary>
        /// Player join vào lobby bằng Game PIN
        /// </summary>
        public async Task<PlayerInfo?> PlayerJoinAsync(string gamePin, string playerName, string connectionId)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
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
            await SaveGameSessionToRedisAsync(gamePin, session);
            
            _logger.LogInformation($"✅ Player '{playerName}' joined game {gamePin}. Total players: {session.Players.Count}");

            return player;
        }

        /// <summary>
        /// Player rời lobby
        /// </summary>
        public async Task<bool> PlayerLeaveAsync(string gamePin, string connectionId)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
                return false;

            var player = session.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null)
                return false;

            session.Players.Remove(player);
            await SaveGameSessionToRedisAsync(gamePin, session);

            _logger.LogInformation($"Player '{player.PlayerName}' left game {gamePin}");

            return true;
        }

        /// <summary>
        /// Lấy thông tin lobby (số người chơi, v.v.)
        /// </summary>
        public async Task<GameSessionDto?> GetGameSessionAsync(string gamePin)
        {
            return await GetGameSessionFromRedisAsync(gamePin);
        }

        // ==================== START GAME ====================
        /// <summary>
        /// Host start game - chuyển sang câu hỏi đầu tiên
        /// </summary>
        public async Task<QuestionDto?> StartGameAsync(string gamePin, Action<string> onQuestionTimeout)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
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
            await SaveGameSessionToRedisAsync(gamePin, session);
            
            _logger.LogInformation($"✅ Game {gamePin} started with {session.Players.Count} players");

            return question;
        }

        // ==================== SUBMIT ANSWER ====================
        /// <summary>
        /// Player submit câu trả lời
        /// </summary>
        public async Task<bool> SubmitAnswerAsync(string gamePin, string connectionId, Guid questionId, Guid answerId)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
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
            var correctMap = await GetCorrectAnswersFromRedisAsync(gamePin);
            if (correctMap != null)
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
                IsCorrect = isCorrect,
                PointsEarned = points,
                TimeSpent = timeSpent,
                SubmittedAt = DateTime.UtcNow
            };

            session.CurrentAnswers[connectionId] = answer;

            // Cập nhật điểm của player
            player.Score += points;

            await SaveGameSessionToRedisAsync(gamePin, session);

            _logger.LogInformation($"✅ Player '{player.PlayerName}' submitted answer for question {questionId}. Correct: {isCorrect}, Points: {points}");

            return true;
        }

        // ==================== QUESTION TIMEOUT & SHOW RESULT ====================
        /// <summary>
        /// Xử lý khi hết giờ - trả về kết quả câu hỏi
        /// </summary>
        public async Task<GameAnswerResultDto?> GetQuestionResultAsync(string gamePin)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
                return null;

            var currentQuestion = session.Questions[session.CurrentQuestionIndex];

            // Tìm đáp án đúng
            Guid correctAnswerId = Guid.Empty;
            string correctAnswerText = string.Empty;

            var correctMap = await GetCorrectAnswersFromRedisAsync(gamePin);
            if (correctMap != null)
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
            await SaveGameSessionToRedisAsync(gamePin, session);

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
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
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
            await SaveGameSessionToRedisAsync(gamePin, session);

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
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
                return null;

            session.CurrentQuestionIndex++;

            // Check nếu hết câu hỏi
            if (session.CurrentQuestionIndex >= session.Questions.Count)
            {
                session.Status = GameStatus.Completed;
                await SaveGameSessionToRedisAsync(gamePin, session);
                return null; // Hết câu hỏi
            }

            session.Status = GameStatus.InProgress;
            session.QuestionStartedAt = DateTime.UtcNow;
            session.CurrentAnswers.Clear();

            var question = session.Questions[session.CurrentQuestionIndex];

            // Start timer trên server
            StartQuestionTimer(gamePin, session.TimePerQuestion, onQuestionTimeout);

            await SaveGameSessionToRedisAsync(gamePin, session);
            
            _logger.LogInformation($"✅ Game {gamePin} moved to question {session.CurrentQuestionIndex + 1}");

            return question;
        }

        // ==================== GAME END ====================
        /// <summary>
        /// Lấy kết quả cuối cùng khi game kết thúc
        /// </summary>
        public async Task<FinalResultDto?> GetFinalResultAsync(string gamePin)
        {
            var session = await GetGameSessionFromRedisAsync(gamePin);
            if (session == null)
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

            // ✨ Xóa khỏi Redis
            await DeleteGameFromRedisAsync(gamePin);

            _logger.LogInformation($"✅ Game {gamePin} cleaned up from Redis");
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
                _logger.LogInformation($"⏰ Question timer expired for game {gamePin}");
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
            // ⚠️ NOTE: Redis không hỗ trợ scan tất cả games dễ dàng
            // Giải pháp tạm: Lưu mapping connectionId -> gamePin riêng
            // Hoặc client phải gọi LeaveGame trước khi disconnect
            
            _logger.LogWarning($"HandleDisconnectAsync called for {connectionId}. Consider implementing connection tracking in Redis.");
            
            return null;
        }

        /// <summary>
        /// Tìm game PIN từ connection ID
        /// </summary>
        public async Task<string?> GetGamePinByConnectionAsync(string connectionId)
        {
            // ⚠️ NOTE: Redis không hỗ trợ scan tất cả games dễ dàng
            // Giải pháp: Lưu mapping connectionId -> gamePin riêng trong Redis
            // Ví dụ: await _cache.GetStringAsync($"connection:{connectionId}");
            
            _logger.LogWarning($"GetGamePinByConnectionAsync called for {connectionId}. Consider implementing connection tracking in Redis.");
            
            return null;
        }
    }
}
