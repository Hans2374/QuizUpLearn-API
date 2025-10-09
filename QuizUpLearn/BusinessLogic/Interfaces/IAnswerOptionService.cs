using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IAnswerOptionService
    {
        Task<ResponseAnswerOptionDto> CreateAsync(RequestAnswerOptionDto dto);
        Task<ResponseAnswerOptionDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ResponseAnswerOptionDto>> GetAllAsync(bool includeDeleted = false);
        Task<IEnumerable<ResponseAnswerOptionDto>> GetByQuizIdAsync(Guid quizId, bool includeDeleted = false);
        Task<ResponseAnswerOptionDto?> UpdateAsync(Guid id, RequestAnswerOptionDto dto);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}
