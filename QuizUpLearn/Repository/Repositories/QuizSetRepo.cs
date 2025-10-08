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

        public async Task<QuizSet> GetQuizSetByIdAsync(Guid id)
        {
            return await _context.QuizSets
                .Include(qs => qs.Creator)
                .Include(qs => qs.Quizzes)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(qs => qs.Id == id && qs.DeletedAt == null);
        }

        public async Task<IEnumerable<QuizSet>> GetAllQuizSetsAsync()
        {
            return await _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => qs.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizSet>> GetQuizSetsByCreatorAsync(Guid creatorId)
        {
            return await _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => qs.CreatedBy == creatorId && qs.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizSet>> GetPublishedQuizSetsAsync()
        {
            return await _context.QuizSets
                .Include(qs => qs.Creator)
                .Where(qs => qs.IsPublished && qs.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<QuizSet> UpdateQuizSetAsync(QuizSet quizSet)
        {
            quizSet.UpdatedAt = DateTime.UtcNow;
            _context.QuizSets.Update(quizSet);
            await _context.SaveChangesAsync();
            return quizSet;
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
    }
}
