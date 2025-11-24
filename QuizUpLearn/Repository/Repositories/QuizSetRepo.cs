using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class QuizSetRepo : IQuizSetRepo
    {
        private readonly MyDbContext _context;

        public QuizSetRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<QuizSet> CreateQuizSetAsync(QuizSet quizSet)
        {
            await _context.QuizSets.AddAsync(quizSet);
            await _context.SaveChangesAsync();
            return quizSet;
        }

        public async Task<QuizSet?> GetQuizSetByIdAsync(Guid id)
        {
            return await _context.QuizSets
                .Include(qs => qs.Creator)
                .Include(qs => qs.Quizzes)
                    .ThenInclude(q => q.AnswerOptions)
                .Include(qs => qs.QuizGroupItems)
                .FirstOrDefaultAsync(qs => qs.Id == id && qs.DeletedAt == null);
        }

        public async Task<IEnumerable<QuizSet>> GetAllQuizSetsAsync(
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
            bool? showManuallyCreated = null)
        {
            var query = _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => includeDeleted || qs.DeletedAt == null);

            query = ApplyFilters(query, showDeleted, showPremiumOnly, showNonPremium, 
                               showPublished, showUnpublished, showAIGenerated, showManuallyCreated);
            query = ApplySearch(query, searchTerm);
            query = ApplySorting(query, sortBy, sortDirection);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<QuizSet>> GetQuizSetsByCreatorAsync(
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
            bool? showManuallyCreated = null)
        {
            var query = _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => qs.CreatedBy == creatorId && qs.DeletedAt == null);

            query = ApplyFilters(query, showDeleted, showPremiumOnly, showNonPremium, 
                               showPublished, showUnpublished, showAIGenerated, showManuallyCreated);
            query = ApplySearch(query, searchTerm);
            query = ApplySorting(query, sortBy, sortDirection);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<QuizSet>> GetPublishedQuizSetsAsync(
            string? searchTerm = null, 
            string? sortBy = null, 
            string? sortDirection = null)
        {
            var query = _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => qs.IsPublished && qs.DeletedAt == null);

            query = ApplySearch(query, searchTerm);
            query = ApplySorting(query, sortBy, sortDirection);

            return await query.ToListAsync();
        }

        public async Task<QuizSet?> UpdateQuizSetAsync(Guid id, QuizSet quizSet)
        {
            var existingQuizSet = await _context.QuizSets.FindAsync(id);
            if (existingQuizSet == null || existingQuizSet.DeletedAt != null)
                return null;

            if(!string.IsNullOrEmpty(quizSet.Title))
                existingQuizSet.Title = quizSet.Title;
            if(!string.IsNullOrEmpty(quizSet.Description))
                existingQuizSet.Description = quizSet.Description;
            if (!string.IsNullOrEmpty(quizSet.QuizSetType.ToString()))
                existingQuizSet.QuizSetType = quizSet.QuizSetType;
            if (!string.IsNullOrEmpty(quizSet.DifficultyLevel))
                existingQuizSet.DifficultyLevel = quizSet.DifficultyLevel;

            existingQuizSet.IsPublished = quizSet.IsPublished;
            existingQuizSet.IsPremiumOnly = quizSet.IsPremiumOnly;
            
            existingQuizSet.UpdatedAt = DateTime.UtcNow;
            _context.QuizSets.Update(existingQuizSet);
            await _context.SaveChangesAsync();
            return existingQuizSet;
        }

        public async Task<bool> SoftDeleteQuizSetAsync(Guid id)
        {
            var quizSet = await _context.QuizSets.FindAsync(id);
            if (quizSet == null)
                return false;

            quizSet.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteQuizSetAsync(Guid id)
        {
            var quizSet = await _context.QuizSets.FindAsync(id);
            if (quizSet == null)
                return false;

            _context.QuizSets.Remove(quizSet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> QuizSetExistsAsync(Guid id)
        {
            return await _context.QuizSets.AnyAsync(qs => qs.Id == id && qs.DeletedAt == null);
        }

        public async Task<QuizSet?> RestoreQuizSetAsync(Guid id)
        {
            var quizSet = await _context.QuizSets.FindAsync(id);
            if (quizSet == null)
                return null;
            if (quizSet.DeletedAt == null)
                return null;

            quizSet.DeletedAt = null;
            _context.QuizSets.Update(quizSet);
            await _context.SaveChangesAsync();
            return quizSet;
        }

        private IQueryable<QuizSet> ApplyFilters(
            IQueryable<QuizSet> query,
            bool? showDeleted = null,
            bool? showPremiumOnly = null,
            bool? showNonPremium = null,
            bool? showPublished = null,
            bool? showUnpublished = null,
            bool? showAIGenerated = null,
            bool? showManuallyCreated = null)
        {
            if (showDeleted == true)
            {
                query = query.Where(qs => qs.DeletedAt != null);
            }

            if (showPremiumOnly == true)
            {
                query = query.Where(qs => qs.IsPremiumOnly == true);
            }

            if (showNonPremium == true)
            {
                query = query.Where(qs => qs.IsPremiumOnly == false);
            }

            if (showPublished == true)
            {
                query = query.Where(qs => qs.IsPublished == true);
            }

            if (showUnpublished == true)
            {
                query = query.Where(qs => qs.IsPublished == false);
            }

            if (showAIGenerated == true)
            {
                query = query.Where(qs => qs.IsAIGenerated == true);
            }

            if (showManuallyCreated == true)
            {
                query = query.Where(qs => qs.IsAIGenerated == false);
            }

            return query;
        }

        private IQueryable<QuizSet> ApplySearch(IQueryable<QuizSet> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            var normalizedSearchTerm = searchTerm.ToLower();

            return query.Where(qs => 
                (qs.Title != null && qs.Title.ToLower().Contains(normalizedSearchTerm)) ||
                (qs.Description != null && qs.Description.ToLower().Contains(normalizedSearchTerm)) ||
                (qs.DifficultyLevel != null && qs.DifficultyLevel.ToLower().Contains(normalizedSearchTerm))
            );
        }

        private IQueryable<QuizSet> ApplySorting(IQueryable<QuizSet> query, string? sortBy, string? sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy))
                return query.OrderByDescending(qs => qs.CreatedAt);

            var isDescending = !string.IsNullOrEmpty(sortDirection) && 
                              sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return sortBy.ToLower() switch
            {
                "title" => isDescending 
                    ? query.OrderByDescending(qs => qs.Title) 
                    : query.OrderBy(qs => qs.Title),
                "createdat" => isDescending 
                    ? query.OrderByDescending(qs => qs.CreatedAt) 
                    : query.OrderBy(qs => qs.CreatedAt),
                "updatedat" => isDescending 
                    ? query.OrderByDescending(qs => qs.UpdatedAt) 
                    : query.OrderBy(qs => qs.UpdatedAt),
                "deletedat" => isDescending 
                    ? query.OrderByDescending(qs => qs.DeletedAt) 
                    : query.OrderBy(qs => qs.DeletedAt),
                "difficultylevel" => isDescending 
                    ? query.OrderByDescending(qs => qs.DifficultyLevel) 
                    : query.OrderBy(qs => qs.DifficultyLevel),
                "totalattempts" => isDescending 
                    ? query.OrderByDescending(qs => qs.TotalAttempts) 
                    : query.OrderBy(qs => qs.TotalAttempts),
                "averagescore" => isDescending 
                    ? query.OrderByDescending(qs => qs.AverageScore) 
                    : query.OrderBy(qs => qs.AverageScore),
                _ => query.OrderByDescending(qs => qs.CreatedAt)
            };
        }
    }
}
