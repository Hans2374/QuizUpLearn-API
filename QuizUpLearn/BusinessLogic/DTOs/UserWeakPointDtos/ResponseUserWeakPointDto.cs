using BusinessLogic.DTOs.UserMistakeDtos;

namespace BusinessLogic.DTOs.UserWeakPointDtos
{
    public class ResponseUserWeakPointDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid UserMistakeId { get; set; }
        public string WeakPoint { get; set; } = string.Empty;
        public string? ToeicPart { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Advice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ResponseUserMistakeDto? UserMistakeDto { get; set; }
        public List<ResponseUserMistakeDto>? UserMistakes { get; set; }
    }
}
