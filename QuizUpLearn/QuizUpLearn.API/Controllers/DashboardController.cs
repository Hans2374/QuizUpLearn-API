using BusinessLogic.DTOs.DashboardDtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace QuizUpLearn.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get complete dashboard data for the authenticated user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                var dashboardData = await _dashboardService.GetDashboardDataAsync(userId);
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get user statistics (total quizzes, accuracy rate, streak, rank)
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetUserStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                var stats = await _dashboardService.GetUserStatsAsync(userId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user stats");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get user progress chart data
        /// </summary>
        [HttpGet("progress")]
        public async Task<IActionResult> GetUserProgress([FromQuery] int days = 7)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (days < 1 || days > 30)
                {
                    return BadRequest("Days parameter must be between 1 and 30");
                }

                var progress = await _dashboardService.GetUserProgressAsync(userId, days);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user progress");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get recent user activities
        /// </summary>
        [HttpGet("activities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int count = 10)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (count < 1 || count > 50)
                {
                    return BadRequest("Count parameter must be between 1 and 50");
                }

                var activities = await _dashboardService.GetRecentActivitiesAsync(userId, count);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent activities");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get user event participations
        /// </summary>
        [HttpGet("events")]
        public async Task<IActionResult> GetEventParticipations([FromQuery] int count = 5)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (count < 1 || count > 20)
                {
                    return BadRequest("Count parameter must be between 1 and 20");
                }

                var events = await _dashboardService.GetEventParticipationsAsync(userId, count);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event participations");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get recent quiz history
        /// </summary>
        [HttpGet("quiz-history")]
        public async Task<IActionResult> GetRecentQuizHistory([FromQuery] int count = 10)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (count < 1 || count > 50)
                {
                    return BadRequest("Count parameter must be between 1 and 50");
                }

                var quizHistory = await _dashboardService.GetRecentQuizHistoryAsync(userId, count);
                return Ok(quizHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent quiz history");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get user weak points for improvement recommendations
        /// </summary>
        [HttpGet("weak-points")]
        public async Task<IActionResult> GetUserWeakPoints([FromQuery] int count = 5)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (count < 1 || count > 20)
                {
                    return BadRequest("Count parameter must be between 1 and 20");
                }

                var weakPoints = await _dashboardService.GetUserWeakPointsAsync(userId, count);
                return Ok(weakPoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user weak points");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Record a new user activity
        /// </summary>
        [HttpPost("activities")]
        public async Task<IActionResult> RecordActivity([FromBody] RecordActivityRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized("User not authenticated");
                }

                if (string.IsNullOrEmpty(request.ActivityType) || string.IsNullOrEmpty(request.Description))
                {
                    return BadRequest("ActivityType and Description are required");
                }

                var success = await _dashboardService.RecordActivityAsync(userId, request.ActivityType, request.Description, request.Metadata);
                
                if (success)
                {
                    return Ok(new { message = "Activity recorded successfully" });
                }
                else
                {
                    return StatusCode(500, "Failed to record activity");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording activity");
                return StatusCode(500, "Internal server error");
            }
        }

        private Guid GetCurrentUserId()
        {
            // Ưu tiên claim "userId" (User entity), fallback NameIdentifier/Sub (Account)
            var userIdClaim = User.FindFirst("userId")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }

    public class RecordActivityRequest
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
