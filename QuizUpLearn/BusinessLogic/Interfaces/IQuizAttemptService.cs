using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IQuizAttemptService
    {
        Task<ResponseQuizAttemptDto> CreateAsync(RequestQuizAttemptDto dto);
        Task<ResponseQuizAttemptDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ResponseQuizAttemptDto>> GetAllAsync(bool includeDeleted = false);
        Task<IEnumerable<ResponseQuizAttemptDto>> GetByUserIdAsync(Guid userId, bool includeDeleted = false);
        Task<IEnumerable<ResponseQuizAttemptDto>> GetByQuizSetIdAsync(Guid quizSetId, bool includeDeleted = false);
        Task<ResponseQuizAttemptDto?> UpdateAsync(Guid id, RequestQuizAttemptDto dto);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}
