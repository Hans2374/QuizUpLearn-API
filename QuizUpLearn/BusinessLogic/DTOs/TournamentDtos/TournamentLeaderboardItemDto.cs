namespace BusinessLogic.DTOs.TournamentDtos
{
	public class TournamentLeaderboardItemDto
	{
		public int Rank { get; set; }
		public Guid UserId { get; set; }
		public string Username { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string AvatarUrl { get; set; } = string.Empty;
		public int Score { get; set; }
		public DateTime Date { get; set; }
	}
}

