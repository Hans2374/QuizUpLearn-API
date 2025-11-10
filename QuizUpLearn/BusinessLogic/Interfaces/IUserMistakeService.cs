using BusinessLogic.DTOs.UserMistakeDtos;

namespace BusinessLogic.Interfaces
{
    public interface IUserMistakeService
    {
        Task<IEnumerable<ResponseUserMistakeDto>> GetAllAsync();
        Task<IEnumerable<ResponseUserMistakeDto>> GetAllByUserIdAsync(Guid userId);
        Task<ResponseUserMistakeDto?> GetByIdAsync(Guid id);
        Task AddAsync(RequestUserMistakeDto requestDto);
        Task UpdateAsync(Guid id, RequestUserMistakeDto requestDto);
        Task DeleteAsync(Guid id);
    }
}
