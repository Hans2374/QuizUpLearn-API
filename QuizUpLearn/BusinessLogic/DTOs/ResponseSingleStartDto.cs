using BusinessLogic.DTOs.QuizDtos;

namespace BusinessLogic.DTOs
{
    public class ResponseSingleStartDto
    {
        public required Guid AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public required IEnumerable<QuizStartResponseDto> Questions { get; set; }
    }
}


