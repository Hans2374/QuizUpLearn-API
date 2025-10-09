using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly IQuizAttemptRepo _repo;
        private readonly IMapper _mapper;

        public QuizAttemptService(IQuizAttemptRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseQuizAttemptDto> CreateAsync(RequestQuizAttemptDto dto)
        {
            var entity = _mapper.Map<QuizAttempt>(dto);
            var created = await _repo.CreateAsync(entity);
            return _mapper.Map<ResponseQuizAttemptDto>(created);
        }

        public async Task<IEnumerable<ResponseQuizAttemptDto>> GetAllAsync(bool includeDeleted = false)
        {
            var list = await _repo.GetAllAsync(includeDeleted);
            return _mapper.Map<IEnumerable<ResponseQuizAttemptDto>>(list);
        }

        public async Task<ResponseQuizAttemptDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ResponseQuizAttemptDto>(entity);
        }

        public async Task<IEnumerable<ResponseQuizAttemptDto>> GetByUserIdAsync(Guid userId, bool includeDeleted = false)
        {
            var list = await _repo.GetByUserIdAsync(userId, includeDeleted);
            return _mapper.Map<IEnumerable<ResponseQuizAttemptDto>>(list);
        }

        public async Task<IEnumerable<ResponseQuizAttemptDto>> GetByQuizSetIdAsync(Guid quizSetId, bool includeDeleted = false)
        {
            var list = await _repo.GetByQuizSetIdAsync(quizSetId, includeDeleted);
            return _mapper.Map<IEnumerable<ResponseQuizAttemptDto>>(list);
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            return await _repo.RestoreAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            return await _repo.SoftDeleteAsync(id);
        }

        public async Task<ResponseQuizAttemptDto?> UpdateAsync(Guid id, RequestQuizAttemptDto dto)
        {
            var entity = _mapper.Map<QuizAttempt>(dto);
            var updated = await _repo.UpdateAsync(id, entity);
            return updated == null ? null : _mapper.Map<ResponseQuizAttemptDto>(updated);
        }
    }
}
