using vocabQuizAPI.Models;

namespace vocabQuizAPI.Repositories
{
    public interface IWordRepository
    {
        Task<IEnumerable<Word>> GetAllWordsAsync();

        Task<QuizResponse>? GetNextQuizWordAsync(int userID);
        Task<QuizResponse>? GetNextReviewWordAsync(int userID);
        Task UpdateProgressAsync(int userId, int wordId, bool isCorrect);

        Task<int> AddWordAsync(Word word);
        Task<IEnumerable<Word>> GetWordsByCreatorAsync(int userId);
        Task<Word?> GetWordByIdAsync(int wordId);
        Task<bool> UpdateWordAsync(Word word);
        Task<bool> DeleteWordAsync(int wordId);

        Task<IEnumerable<Word>> UnsafeSearchAsync(string term); // sql injection icin
    }
}
