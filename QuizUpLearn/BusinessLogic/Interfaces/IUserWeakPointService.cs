using BusinessLogic.DTOs.UserWeakPointDtos;

namespace BusinessLogic.Interfaces
{
    public interface IUserWeakPointService
    {
        Task<IEnumerable<ResponseUserWeakPointDto>> GetByUserIdAsync(Guid userId);
        Task<ResponseUserWeakPointDto?> GetByIdAsync(Guid id);
        Task<ResponseUserWeakPointDto?> AddAsync(RequestUserWeakPointDto userWeakPoint);
        Task<ResponseUserWeakPointDto?> UpdateAsync(Guid id, RequestUserWeakPointDto userWeakPoint);
        Task<bool> DeleteAsync(Guid userId);
    }
}
