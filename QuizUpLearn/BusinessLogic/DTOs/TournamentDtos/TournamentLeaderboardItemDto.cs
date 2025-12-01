namespace BusinessLogic.DTOs.TournamentDtos
{
	public class TournamentLeaderboardItemDto
	{
		public int Rank { get; set; }
		public Guid UserId { get; set; }
		public string Username { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string AvatarUrl { get; set; } = string.Empty;
		public int TotalScore { get; set; }
		public decimal AverageScore { get; set; }
		public decimal AverageAccuracy { get; set; }
		public int TotalAttempts { get; set; }
		public int TotalCorrectAnswers { get; set; }
		public int TotalQuestions { get; set; }
	}
}

