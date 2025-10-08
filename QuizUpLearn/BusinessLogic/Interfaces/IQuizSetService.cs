using BusinessLogic.DTOs.QuizSetDtos;

namespace BusinessLogic.Interfaces
{
    public interface IQuizSetService
    {
        Task<QuizSetResponseDto> CreateQuizSetAsync(QuizSetRequestDto quizSetDto);
        Task<QuizSetResponseDto> GetQuizSetByIdAsync(Guid id);
        Task<IEnumerable<QuizSetResponseDto>> GetAllQuizSetsAsync();
        Task<IEnumerable<QuizSetResponseDto>> GetQuizSetsByCreatorAsync(Guid creatorId);
        Task<IEnumerable<QuizSetResponseDto>> GetPublishedQuizSetsAsync();
        Task<QuizSetResponseDto> UpdateQuizSetAsync(Guid id, QuizSetRequestDto quizSetDto);
        Task<bool> SoftDeleteQuizSetAsync(Guid id);
        Task<bool> HardDeleteQuizSetAsync(Guid id);
    }
}
