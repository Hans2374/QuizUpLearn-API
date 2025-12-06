namespace BusinessLogic.DTOs.UserWeakPointDtos
{
    public class RequestUserWeakPointDto
    {
        public Guid UserId { get; set; }
        public required string WeakPoint { get; set; }
        public required string ToeicPart { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Advice { get; set; }
    }
}
