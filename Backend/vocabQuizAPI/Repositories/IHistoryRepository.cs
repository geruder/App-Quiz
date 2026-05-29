using vocabQuizAPI.Models;
using vocabQuizAPI.Models.Dtos;

namespace vocabQuizAPI.Repositories
{
    public interface IHistoryRepository
    {
        Task LogAttemptAsync(int userId, int wordId, bool isCorrect, string mode);
        Task<IEnumerable<HistoryDetailDto>> GetRecentHistoryAsync(int userId);
    }
    
}
