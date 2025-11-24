using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.QuizSetDtos;
using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class QuizSetService : IQuizSetService
    {
        private readonly IQuizSetRepo _quizSetRepo;
        private readonly IMapper _mapper;

        public QuizSetService(IQuizSetRepo quizSetRepo, IMapper mapper)
        {
            _quizSetRepo = quizSetRepo;
            _mapper = mapper;
        }

        public async Task<QuizSetResponseDto> CreateQuizSetAsync(QuizSetRequestDto quizSetDto)
        {
            if(quizSetDto.CreatedBy == null || quizSetDto.CreatedBy == Guid.Empty)
            {
                throw new ArgumentException("CreatedBy cannot be null or empty");
            }
            var quizSet = _mapper.Map<QuizSet>(quizSetDto);
            var createdQuizSet = await _quizSetRepo.CreateQuizSetAsync(quizSet);
            return _mapper.Map<QuizSetResponseDto>(createdQuizSet);
        }

        public async Task<QuizSetResponseDto> GetQuizSetByIdAsync(Guid id)
        {
            var quizSet = await _quizSetRepo.GetQuizSetByIdAsync(id);
            return _mapper.Map<QuizSetResponseDto>(quizSet);
        }

        public async Task<PaginationResponseDto<QuizSetResponseDto>> GetAllQuizSetsAsync(bool includeDeleted, PaginationRequestDto pagination)
        {
            var filters = ExtractFilterValues(pagination);

            var quizSets = await _quizSetRepo.GetAllQuizSetsAsync(
                includeDeleted, 
                pagination.SearchTerm, 
                pagination.SortBy, 
                pagination.SortDirection,
                filters.showDeleted,
                filters.showPremiumOnly,
                filters.showNonPremium,
                filters.showPublished,
                filters.showUnpublished,
                filters.showAIGenerated,
                filters.showManuallyCreated);

            var dtos = _mapper.Map<IEnumerable<QuizSetResponseDto>>(quizSets);
            return PaginationHelper.CreatePagedResponse(dtos, pagination);
        }

        public async Task<PaginationResponseDto<QuizSetResponseDto>> GetQuizSetsByCreatorAsync(Guid creatorId, PaginationRequestDto pagination)
        {
            var filters = ExtractFilterValues(pagination);

            var quizSets = await _quizSetRepo.GetQuizSetsByCreatorAsync(
                creatorId, 
                pagination.SearchTerm, 
                pagination.SortBy, 
                pagination.SortDirection,
                filters.showDeleted,
                filters.showPremiumOnly,
                filters.showNonPremium,
                filters.showPublished,
                filters.showUnpublished,
                filters.showAIGenerated,
                filters.showManuallyCreated);

            var dtos = _mapper.Map<IEnumerable<QuizSetResponseDto>>(quizSets);
            return PaginationHelper.CreatePagedResponse(dtos, pagination);
        }

        public async Task<PaginationResponseDto<QuizSetResponseDto>> GetPublishedQuizSetsAsync(PaginationRequestDto pagination)
        {
            var quizSets = await _quizSetRepo.GetPublishedQuizSetsAsync(
                pagination.SearchTerm, 
                pagination.SortBy, 
                pagination.SortDirection);

            var dtos = _mapper.Map<IEnumerable<QuizSetResponseDto>>(quizSets);
            return PaginationHelper.CreatePagedResponse(dtos, pagination);
        }

        public async Task<QuizSetResponseDto> UpdateQuizSetAsync(Guid id, QuizSetRequestDto quizSetDto)
        {
            var updatedQuizSet = await _quizSetRepo.UpdateQuizSetAsync(id, _mapper.Map<QuizSet>(quizSetDto));
            return _mapper.Map<QuizSetResponseDto>(updatedQuizSet);
        }

        public async Task<bool> SoftDeleteQuizSetAsync(Guid id)
        {
            return await _quizSetRepo.SoftDeleteQuizSetAsync(id);
        }

        public async Task<bool> HardDeleteQuizSetAsync(Guid id)
        {
            return await _quizSetRepo.HardDeleteQuizSetAsync(id);
        }

        public async Task<QuizSetResponseDto> RestoreQuizSetAsync(Guid id)
        {
            var quizSet = await _quizSetRepo.RestoreQuizSetAsync(id);
            return _mapper.Map<QuizSetResponseDto>(quizSet);
        }

        private (bool? showDeleted, bool? showPremiumOnly, bool? showNonPremium, bool? showPublished, 
                bool? showUnpublished, bool? showAIGenerated, bool? showManuallyCreated) ExtractFilterValues(PaginationRequestDto pagination)
        {
            if (pagination.Filters == null)
                return (null, null, null, null, null, null, null);

            bool? showDeleted = pagination.Filters.ContainsKey("showDeleted") && 
                               pagination.Filters["showDeleted"] is bool sd ? sd : null;

            bool? showPremiumOnly = pagination.Filters.ContainsKey("showPremiumOnly") && 
                                   pagination.Filters["showPremiumOnly"] is bool spo ? spo : null;

            bool? showNonPremium = pagination.Filters.ContainsKey("showNonPremium") && 
                                  pagination.Filters["showNonPremium"] is bool snp ? snp : null;

            bool? showPublished = pagination.Filters.ContainsKey("showPublished") && 
                                 pagination.Filters["showPublished"] is bool sp ? sp : null;

            bool? showUnpublished = pagination.Filters.ContainsKey("showUnpublished") && 
                                   pagination.Filters["showUnpublished"] is bool su ? su : null;

            bool? showAIGenerated = pagination.Filters.ContainsKey("showAIGenerated") && 
                                   pagination.Filters["showAIGenerated"] is bool sai ? sai : null;

            bool? showManuallyCreated = pagination.Filters.ContainsKey("showManuallyCreated") && 
                                       pagination.Filters["showManuallyCreated"] is bool smc ? smc : null;

            return (showDeleted, showPremiumOnly, showNonPremium, showPublished, showUnpublished, showAIGenerated, showManuallyCreated);
        }
    }
}
