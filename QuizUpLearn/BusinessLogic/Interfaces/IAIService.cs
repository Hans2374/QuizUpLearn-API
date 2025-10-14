using BusinessLogic.DTOs.AiDtos;
using BusinessLogic.DTOs.QuizSetDtos;

namespace BusinessLogic.Interfaces
{
    public interface IAIService
    {
        Task<string> GeminiGenerateContentAsync(string prompt);
        Task<QuizSetResponseDto> GeneratePracticeQuizSetAsync(AiGenerateQuizSetRequestDto inputData);
        Task AnalyzeUserProgress();
        Task<(bool, string)> ValidateQuizSetAsync(Guid quizSetId);
    }
}
