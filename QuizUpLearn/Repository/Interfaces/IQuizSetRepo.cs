using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IQuizSetRepo
    {
        Task<QuizSet> CreateQuizSetAsync(QuizSet quizSet);
        Task<QuizSet?> GetQuizSetByIdAsync(Guid id);
        Task<IEnumerable<QuizSet>> GetAllQuizSetsAsync(
            bool includeDeleted, 
            string? searchTerm = null, 
            string? sortBy = null, 
            string? sortDirection = null,
            bool? showDeleted = null,
            bool? showPremiumOnly = null,
            bool? showNonPremium = null,
            bool? showPublished = null,
            bool? showUnpublished = null,
            bool? showAIGenerated = null,
            bool? showManuallyCreated = null);
        Task<IEnumerable<QuizSet>> GetQuizSetsByCreatorAsync(
            Guid creatorId, 
            string? searchTerm = null, 
            string? sortBy = null, 
            string? sortDirection = null,
            bool? showDeleted = null,
            bool? showPremiumOnly = null,
            bool? showNonPremium = null,
            bool? showPublished = null,
            bool? showUnpublished = null,
            bool? showAIGenerated = null,
            bool? showManuallyCreated = null);
        Task<IEnumerable<QuizSet>> GetPublishedQuizSetsAsync(
            string? searchTerm = null, 
            string? sortBy = null, 
            string? sortDirection = null);
        Task<QuizSet?> UpdateQuizSetAsync(Guid id, QuizSet quizSet);
        Task<bool> SoftDeleteQuizSetAsync(Guid id);
        Task<bool> HardDeleteQuizSetAsync(Guid id);
        Task<bool> QuizSetExistsAsync(Guid id);
        Task<QuizSet?> RestoreQuizSetAsync(Guid id);
    }
}
