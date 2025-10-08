using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class AnswerOptionService : IAnswerOptionService
    {
        private readonly IAnswerOptionRepo _repo;
        private readonly IMapper _mapper;

        public AnswerOptionService(IAnswerOptionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseAnswerOptionDto> CreateAsync(RequestAnswerOptionDto dto)
        {
            var entity = _mapper.Map<AnswerOption>(dto);
            var created = await _repo.CreateAsync(entity);
            return _mapper.Map<ResponseAnswerOptionDto>(created);
        }

        public async Task<IEnumerable<ResponseAnswerOptionDto>> GetAllAsync(bool includeDeleted = false)
        {
            var list = await _repo.GetAllAsync(includeDeleted);
            return _mapper.Map<IEnumerable<ResponseAnswerOptionDto>>(list);
        }

        public async Task<ResponseAnswerOptionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ResponseAnswerOptionDto>(entity);
        }

        public async Task<IEnumerable<ResponseAnswerOptionDto>> GetByQuizIdAsync(Guid quizId, bool includeDeleted = false)
        {
            var list = await _repo.GetByQuizIdAsync(quizId, includeDeleted);
            return _mapper.Map<IEnumerable<ResponseAnswerOptionDto>>(list);
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            return await _repo.RestoreAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            return await _repo.SoftDeleteAsync(id);
        }

        public async Task<ResponseAnswerOptionDto?> UpdateAsync(Guid id, RequestAnswerOptionDto dto)
        {
            var entity = _mapper.Map<AnswerOption>(dto);
            var updated = await _repo.UpdateAsync(id, entity);
            return updated == null ? null : _mapper.Map<ResponseAnswerOptionDto>(updated);
        }
    }
}
