using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using vocabQuizAPI.Models;

namespace vocabQuizAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public WordRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Word>> GetAllWordsAsync()
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT word_id as WordId, english_word as EnglishWord,  
                                turkish_meaning as TurkishMeaning, cefr_level as CefrLevel 
                                FROM words"; // tum kelimeleri cekiyoruz

                return await dbConnection.QueryAsync<Word>(query);
            }
        }

        public async Task<QuizResponse>? GetNextQuizWordAsync(int userID)
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                dbConnection.Open();
                
                string targetQuery = @"
            SELECT w.word_id AS WordId, w.english_word AS EnglishWord, 
                   w.turkish_meaning AS TurkishMeaning, w.cefr_level AS CefrLevel
            FROM words w
            LEFT JOIN user_word_progress uwp 
                ON w.word_id = uwp.word_id AND uwp.user_id = @UserId
            WHERE uwp.word_id IS NULL 
            ORDER BY RAND() LIMIT 1"; // word_id null secerek daha once gormedigimiz kelimeleri cekiyoruz 

                var targetWord = await dbConnection.QueryFirstOrDefaultAsync<Word>(targetQuery, new { UserId = userID });

                if(targetWord == null) return null;

                
                
                string distractorQuery = @"SELECT turkish_meaning as TurkishMeaning
                                FROM words
                                WHERE word_id != @TargetId
                                ORDER BY RAND()
                                LIMIT 3"; // diger 3 yanlis kelimeyi cekiyoruz

                var distractors = await dbConnection.QueryAsync<string>(distractorQuery, new { TargetId = targetWord.WordId });


                var options = distractors.ToList();
                options.Add(targetWord.TurkishMeaning);

                var shuffledOptions = options.OrderBy(x => Guid.NewGuid()).ToList();


                return new QuizResponse
                {
                    WordId = targetWord.WordId,
                    EnglishWord = targetWord.EnglishWord,
                    CorrectMeaning = targetWord.TurkishMeaning,
                    CefrLevel = targetWord.CefrLevel,
                    Options = shuffledOptions
                };
            }
        }

        public async Task<QuizResponse>? GetNextReviewWordAsync(int userID)
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                dbConnection.Open();
                
                string query = @"SELECT w.word_id as WordId, w.english_word as EnglishWord, 
                                    w.turkish_meaning as TurkishMeaning, w.cefr_level as CefrLevel
                                FROM words w
                                INNER JOIN user_word_progress uwp
                                    ON w.word_id = uwp.word_id 
                                WHERE uwp.user_id = @UserId AND uwp.status = 'review'
                                ORDER BY RAND()
                                LIMIT 1"; // review kismi icin kelimeleri cekiyoruz


                var reviewWord = await dbConnection.QueryFirstOrDefaultAsync<Word>(query, new { UserId = userID });

                if (reviewWord == null) return null;

                string distractorQuery = @"SELECT turkish_meaning as TurkishMeaning
                                FROM words
                                WHERE word_id != @ReviewId
                                ORDER BY RAND()
                                LIMIT 3"; // diger 3 yanlis kelimeyi cekiyoruz

                var distractors = await dbConnection.QueryAsync<string>(distractorQuery, new { ReviewId = reviewWord.WordId });

                var options = distractors.ToList();
                options.Add(reviewWord.TurkishMeaning);

                var shuffledOptions = options.OrderBy(x => Guid.NewGuid()).ToList();

                return new QuizResponse
                {
                    WordId = reviewWord.WordId,
                    EnglishWord = reviewWord.EnglishWord,
                    CorrectMeaning = reviewWord.TurkishMeaning,
                    CefrLevel = reviewWord.CefrLevel,
                    Options = shuffledOptions
                };
            }
        }

        public async Task UpdateProgressAsync(int userId, int wordId, bool isCorrect)
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                dbConnection.Open();

                string checkQuery = @"
            SELECT 
                progress_id AS ProgressId, 
                user_id AS UserId, 
                word_id AS WordId, 
                status AS Status, 
                streak AS Streak 
            FROM user_word_progress 
            WHERE user_id = @UserId AND word_id = @WordId";


                var currentProgress = await dbConnection.QueryFirstOrDefaultAsync<UserWordProgress>(checkQuery, new { UserId = userId, WordId = wordId });

                if (currentProgress == null) {
                    if (isCorrect)
                    {
                        string insertMastered = @"
                    INSERT INTO user_word_progress (user_id, word_id, status, streak) 
                    VALUES (@UserId, @WordId, 'mastered', 0)"; // tekte dogru bilme 
                        await dbConnection.ExecuteAsync(insertMastered, new { UserId = userId, WordId = wordId });
                    }
                    else
                    {
                        string insertReview = @"
                    INSERT INTO user_word_progress (user_id, word_id, status, streak) 
                    VALUES (@UserId, @WordId, 'review', 0)"; // yanlis bildiyse review
                        await dbConnection.ExecuteAsync(insertReview, new { UserId = userId, WordId = wordId });
                    }
                }
                else
                {
                    if (isCorrect)
                    {
                        int newStreak = currentProgress.Streak + 1;
                        string newStatus = "review";

                        if (newStreak >= 3)
                        {
                            newStatus = "mastered";
                        }

                        string updateCorrect = @"UPDATE user_word_progress
                                                SET streak = @Streak, status = @Status
                                                WHERE progress_id = @Progressid";

                        await dbConnection.ExecuteAsync(updateCorrect, new { Streak = newStreak, Status = newStatus, ProgressId = currentProgress.ProgressId });
                    }
                    else
                    {
                        string updateWrong = @"
                            UPDATE user_word_progress
                            SET streak = 0, status = 'review'
                            WHERE progress_id = @ProgressId";
                        await dbConnection.ExecuteAsync(updateWrong, new { ProgressId = currentProgress.ProgressId });
                    }
                }
            }
        }


        // crud create kismi
        public async Task<int> AddWordAsync(Word word)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO words (category_id, created_by, english_word, turkish_meaning, cefr_level)
                    VALUES (@CategoryId, @CreatedBy, @EnglishWord, @TurkishMeaning, @CefrLevel);
                    SELECT LAST_INSERT_ID();";

                return await db.ExecuteScalarAsync<int>(query, word);
            }
        }


        //crud read kismi
        public async Task<IEnumerable<Word>> GetWordsByCreatorAsync(int userId)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        word_id AS WordId, category_id AS CategoryId, created_by AS CreatedBy,
                        english_word AS EnglishWord, turkish_meaning AS TurkishMeaning, cefr_level AS CefrLevel
                    FROM words 
                    WHERE created_by = @UserId
                    ORDER BY word_id DESC"; // stack, yeniden eskiye

                return await db.QueryAsync<Word>(query, new { UserId = userId });
            }
        }

        //crud rea
        public async Task<Word?> GetWordByIdAsync(int wordId)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        word_id AS WordId, category_id AS CategoryId, created_by AS CreatedBy,
                        english_word AS EnglishWord, turkish_meaning AS TurkishMeaning, cefr_level AS CefrLevel
                    FROM words WHERE word_id = @WordId";

                return await db.QueryFirstOrDefaultAsync<Word>(query, new { WordId = wordId });
            }
        }

        //crud update
        public async Task<bool> UpdateWordAsync(Word word)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE words 
                    SET category_id = @CategoryId,
                        english_word = @EnglishWord,
                        turkish_meaning = @TurkishMeaning,
                        cefr_level = @CefrLevel
                    WHERE word_id = @WordId";

                int rowsAffected = await db.ExecuteAsync(query, word);
                return rowsAffected > 0; // eger degisiklik olduysa zaten true
            }
        }


        //crud delete
        public async Task<bool> DeleteWordAsync(int wordId)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM words WHERE word_id = @WordId";

                int rowsAffected = await db.ExecuteAsync(query, new { WordId = wordId });
                return rowsAffected > 0;
            }
        }

        //sql injection
        public async Task<IEnumerable<Word>> UnsafeSearchAsync(string term)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {

                string query = $@"
                                SELECT 
                                word_id AS WordId, 
                                category_id AS CategoryId,
                                root_word_id AS RootWordId,
                                created_by AS CreatedBy,
                                english_word AS EnglishWord, 
                                turkish_meaning AS TurkishMeaning, 
                                cefr_level AS CefrLevel
                                FROM words 
                                WHERE english_word = '{term}'";

                return await db.QueryAsync<Word>(query);
            }
        }


    }
}
