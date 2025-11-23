namespace BusinessLogic.DTOs
{
    public class PlacementTestHistoryResponseDto
    {
        public IEnumerable<PlacementTestHistoryItemDto> Attempts { get; set; } = new List<PlacementTestHistoryItemDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }
}

