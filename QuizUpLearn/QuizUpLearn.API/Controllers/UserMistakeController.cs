using Microsoft.AspNetCore.Mvc;
using BusinessLogic.DTOs.UserMistakeDtos;
using BusinessLogic.Interfaces;

namespace QuizUpLearn.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMistakeController : ControllerBase
    {
        private readonly IUserMistakeService _userMistakeService;

        public UserMistakeController(IUserMistakeService userMistakeService)
        {
            _userMistakeService = userMistakeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userMistakes = await _userMistakeService.GetAllAsync();
            return Ok(userMistakes);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userMistake = await _userMistakeService.GetByIdAsync(id);
            if (userMistake == null)
            {
                return NotFound();
            }
            return Ok(userMistake);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestUserMistakeDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userMistakeService.AddAsync(requestDto);
            return CreatedAtAction(nameof(GetById), new { id = requestDto.UserId }, requestDto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RequestUserMistakeDto requestDto)
        {
            var existingUserMistake = await _userMistakeService.GetByIdAsync(id);
            if (existingUserMistake == null)
            {
                return NotFound();
            }

            await _userMistakeService.UpdateAsync(id, requestDto);
            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUserMistake = await _userMistakeService.GetByIdAsync(id);
            if (existingUserMistake == null)
            {
                return NotFound();
            }

            await _userMistakeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
