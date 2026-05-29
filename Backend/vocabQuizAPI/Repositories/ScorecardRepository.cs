using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using vocabQuizAPI.Models;



namespace vocabQuizAPI.Repositories
{
    public class ScorecardRepository : IScorecardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ScorecardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DailyScorecard?> GetTodayScorecardAsync(int userId)
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string query = @"
                SELECT scorecard_id AS ScorecardId, user_id AS UserId, date_recorded AS DateRecorded,
                        total_seen AS TotalSeen, total_correct AS TotalCorrect, total_wrong AS TotalWrong
                FROM daily_scorecard
                WHERE user_id = @UserId AND date_recorded = CURDATE()";

                return await dbConnection.QueryFirstOrDefaultAsync<DailyScorecard>(query, new { UserId = userId });
            }
        }

        public async Task<IEnumerable<DailyScorecard>> GetMonthlyStatsAsync(int userId)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        scorecard_id AS ScorecardId,
                        date_recorded AS DateRecorded,
                        total_seen AS TotalSeen,
                        total_correct AS TotalCorrect,
                        total_wrong AS TotalWrong
                    FROM daily_scorecard 
                    WHERE user_id = @UserId 
                    ORDER BY date_recorded ASC
                    LIMIT 30";

                return await db.QueryAsync<DailyScorecard>(query, new { UserId = userId });
            }
        }




        public async Task UpdateScoreAsync(int userId, bool isCorrect)
        {
            using(IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO daily_scorecard (user_id, date_recorded, total_seen, total_correct, total_wrong)
                VALUES (@UserId, CURDATE(), 1, @IsCorrectInt, @IsWrongInt)
                ON DUPLICATE KEY UPDATE
                    total_seen = total_seen + 1,
                    total_correct = total_correct + @IsCorrectInt,
                    total_wrong = total_wrong + @IsWrongInt";

                await dbConnection.ExecuteAsync(query, new
                {
                    UserId = userId,
                    IsCorrectInt = isCorrect ? 1 : 0,
                    IsWrongInt = isCorrect ? 0 : 1
                });
            }
        }
    }
}
