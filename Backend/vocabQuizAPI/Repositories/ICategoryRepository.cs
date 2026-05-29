using vocabQuizAPI.Models;

namespace vocabQuizAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Word>> GetWordsByCategoryAsync(int categoryId);
    }
}
