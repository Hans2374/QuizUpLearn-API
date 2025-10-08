using BusinessLogic.DTOs.QuizDtos;

namespace BusinessLogic.Interfaces
{
    public interface IQuizService
    {
        Task<QuizResponseDto> CreateQuizAsync(QuizRequestDto quizDto);
        Task<QuizResponseDto> GetQuizByIdAsync(Guid id);
        Task<IEnumerable<QuizResponseDto>> GetAllQuizzesAsync();
        Task<IEnumerable<QuizResponseDto>> GetQuizzesByQuizSetIdAsync(Guid quizSetId);
        Task<IEnumerable<QuizResponseDto>> GetActiveQuizzesAsync();
        Task<QuizResponseDto> UpdateQuizAsync(Guid id, QuizRequestDto quizDto);
        Task<bool> SoftDeleteQuizAsync(Guid id);
        Task<bool> HardDeleteQuizAsync(Guid id);
    }
}
