using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class QuizRepo : IQuizRepo
    {
        private readonly MyDbContext _context;

        public QuizRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz> GetQuizByIdAsync(Guid id)
        {
            return await _context.Quizzes
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id && q.DeletedAt == null);
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            return await _context.Quizzes
                .Include(q => q.AnswerOptions)
                .Where(q => q.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByQuizSetIdAsync(Guid quizSetId)
        {
            return await _context.Quizzes
                .Include(q => q.AnswerOptions)
                .Where(q => q.QuizSetId == quizSetId && q.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetActiveQuizzesAsync()
        {
            return await _context.Quizzes
                .Include(q => q.AnswerOptions)
                .Where(q => q.IsActive && q.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Quiz> UpdateQuizAsync(Quiz quiz)
        {
            quiz.UpdatedAt = DateTime.UtcNow;
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> SoftDeleteQuizAsync(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return false;

            quiz.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteQuizAsync(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> QuizExistsAsync(Guid id)
        {
            return await _context.Quizzes.AnyAsync(q => q.Id == id && q.DeletedAt == null);
        }
    }
}
