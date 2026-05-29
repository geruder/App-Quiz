using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using vocabQuizAPI.Models;

namespace vocabQuizAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return await db.QueryAsync<Category>("SELECT category_id AS CategoryId, category_name AS CategoryName FROM categories");
            }
        }

        public async Task<IEnumerable<Word>> GetWordsByCategoryAsync(int categoryId)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
                            SELECT word_id AS WordId, english_word AS EnglishWord, turkish_meaning AS TurkishMeaning,
                            cefr_level AS CefrLevel
                            FROM words
                            WHERE category_id = @CatId";

                return await db.QueryAsync<Word>(query, new { CatId = categoryId });
            }
        }
    }
} 

