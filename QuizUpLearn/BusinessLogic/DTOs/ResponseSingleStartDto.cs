using BusinessLogic.DTOs.QuizDtos;
using BusinessLogic.DTOs.QuizGroupItemDtos;

namespace BusinessLogic.DTOs
{
    public class ResponseSingleStartDto
    {
        public required Guid AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public required IEnumerable<QuizStartResponseDto> Questions { get; set; }
        public List<ResponseQuizGroupItemDto> QuizGroupItems { get; set; } = new List<ResponseQuizGroupItemDto>();
    }
}


