using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using vocabQuizAPI.Models;
using vocabQuizAPI.Models.Dtos;

namespace vocabQuizAPI.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public HistoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task LogAttemptAsync(int userId, int wordId, bool isCorrect, string mode)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO quiz_history (user_id, word_id, is_correct, quiz_mode)
                    VALUES (@UserId, @WordId, @IsCorrect, @Mode)";

                await db.ExecuteAsync(query, new { UserId = userId, WordId = wordId, IsCorrect = isCorrect, Mode = mode });
            }
        }

        public async Task<IEnumerable<HistoryDetailDto>> GetRecentHistoryAsync(int userId)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        h.is_correct AS IsCorrect, 
                        h.attempt_at AS AttemptAt,
                        w.english_word AS EnglishWord, 
                        w.turkish_meaning AS TurkishMeaning
                    FROM quiz_history h
                    INNER JOIN words w ON h.word_id = w.word_id
                    WHERE h.user_id = @UserId 
                    ORDER BY h.attempt_at DESC 
                    LIMIT 25";

                return await db.QueryAsync<HistoryDetailDto>(query, new { UserId = userId });
            }
        }
    }
}
