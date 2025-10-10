using BusinessLogic.DTOs.AiDtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace QuizUpLearn.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }
        [HttpPost("generate-quiz-set")]
        public async Task<IActionResult> GenerateQuizSet([FromBody] AiGenerateQuizSetRequestDto inputData)
        {
            if (inputData == null)
            {
                return BadRequest("Prompt cannot be empty.");
            }
            try
            {
                var result = await _aiService.GeneratePracticeQuizSetAsync(inputData);
                return Ok(new { content = result });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
