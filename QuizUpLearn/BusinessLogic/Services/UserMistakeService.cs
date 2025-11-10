using AutoMapper;
using BusinessLogic.DTOs.UserMistakeDtos;
using BusinessLogic.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class UserMistakeService : IUserMistakeService
    {
        private readonly IUserMistakeRepo _repo;
        private readonly IMapper _mapper;

        public UserMistakeService(IUserMistakeRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseUserMistakeDto>> GetAllAsync()
        {
            var userMistakes = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ResponseUserMistakeDto>>(userMistakes);
        }

        public async Task<ResponseUserMistakeDto?> GetByIdAsync(Guid id)
        {
            var userMistake = await _repo.GetByIdAsync(id);
            return userMistake != null ? _mapper.Map<ResponseUserMistakeDto>(userMistake) : null;
        }

        public async Task AddAsync(RequestUserMistakeDto requestDto)
        {
            var userMistake = _mapper.Map<UserMistake>(requestDto);
            await _repo.AddAsync(userMistake);
        }

        public async Task UpdateAsync(Guid id, RequestUserMistakeDto requestDto)
        {
            await _repo.UpdateAsync(id, _mapper.Map<UserMistake>(requestDto));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<ResponseUserMistakeDto>> GetAllByUserIdAsync(Guid userId)
        {
            var userMistakes = await _repo.GetAlByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ResponseUserMistakeDto>>(userMistakes);
        }
    }
}
