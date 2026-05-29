using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using vocabQuizAPI.Models;
using vocabQuizAPI.Models.Dtos;
using vocabQuizAPI.Repositories;

namespace vocabQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;
        private readonly IScorecardRepository _scorecardRepository;
        private readonly IHistoryRepository _historyRepository;

        public WordController(IWordRepository wordRepository, IScorecardRepository scorecardRepository, IHistoryRepository historyRepository)
        {
            _wordRepository = wordRepository;
            _scorecardRepository = scorecardRepository;
            _historyRepository = historyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWords()
        {
            try
            {
                var words = await _wordRepository.GetAllWordsAsync();

                return Ok(words);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("quiz/{userID}")]
        public async Task<IActionResult> GetQuizWord(int userID)
        {
            try
            {
                var word = await _wordRepository.GetNextQuizWordAsync(userID);

                if (word == null)
                {
                    return Ok(new { message = "All words are mastered", finished = true });
                }

                return Ok(word);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("review/{userID}")]
        public async Task<IActionResult> GetReviewWord(int userID)
        {
            try
            {
                var word = await _wordRepository.GetNextReviewWordAsync(userID);

                if (word == null)
                {
                    return Ok(new { message = "No words to review", finished = true });
                }
                return Ok(word);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("answer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] AnswerRequest request)
        {
            try
            {
                await _wordRepository.UpdateProgressAsync(request.UserId, request.WordId, request.IsCorrect);
                
                await _scorecardRepository.UpdateScoreAsync(request.UserId, request.IsCorrect);

                await _historyRepository.LogAttemptAsync(request.UserId, request.WordId, request.IsCorrect, "quiz"); // simdilik quiz olarak kalsin sonra frontend requestten cekeriz

                return Ok(new { message = "Progress updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("stats/{userId}")]
        public async Task<IActionResult> GetDailyStats(int userId)
        {
            try
            {
                var stats = await _scorecardRepository.GetTodayScorecardAsync(userId);

                if (stats == null)
                {
                    return Ok(new { totalSeen = 0, totalCorrect = 0, totalWrong = 0 });
                }
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateWord([FromBody] CreateWordDto request)
        {
            var newWord = new Word
            {
                CreatedBy = request.UserId,
                CategoryId = request.CategoryId,
                EnglishWord = request.EnglishWord,
                TurkishMeaning = request.TurkishMeaning,
                CefrLevel = request.CefrLevel
            };

            int id = await _wordRepository.AddWordAsync(newWord);
            return Ok(new { message = "Word created", wordId = id });
        }

        

        [HttpGet("my-words/{userId}")]
        public async Task<IActionResult> GetMyWords(int userId)
        {
            var words = await _wordRepository.GetWordsByCreatorAsync(userId);
            return Ok(words);
        }

        
        [HttpPut("update/{wordId}")]
        public async Task<IActionResult> UpdateWord(int wordId, [FromBody] UpdateWordDto request)
        {
            
            var existingWord = await _wordRepository.GetWordByIdAsync(wordId);
            if (existingWord == null) return NotFound("Word not found.");

            // kelimeyi sadece ekleyen kullanici
            if (existingWord.CreatedBy != request.RequestingUserId)
                return Unauthorized("You do not own this word.");

            
            existingWord.CategoryId = request.CategoryId;
            existingWord.EnglishWord = request.EnglishWord;
            existingWord.TurkishMeaning = request.TurkishMeaning;
            existingWord.CefrLevel = request.CefrLevel;

            await _wordRepository.UpdateWordAsync(existingWord);
            return Ok(new { message = "Word updated successfully" });
        }

      
        [HttpDelete("delete/{wordId}")]
        public async Task<IActionResult> DeleteWord(int wordId, [FromQuery] int requestingUserId)
        {
            
            var existingWord = await _wordRepository.GetWordByIdAsync(wordId);
            if (existingWord == null) return NotFound("Word not found.");

            // sadece ekleyen kullanici
            if (existingWord.CreatedBy != requestingUserId)
                return Unauthorized("You do not own this word.");

        
            await _wordRepository.DeleteWordAsync(wordId);
            return Ok(new { message = "Word deleted successfully" });
        }

        [HttpGet("unsafe-search")]
        public async Task<IActionResult> UnsafeSearch([FromQuery] string term)
        {
            try
            {
                var results = await _wordRepository.UnsafeSearchAsync(term);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var history = await _historyRepository.GetRecentHistoryAsync(userId);
            return Ok(history);
        }

        [HttpGet("stats/monthly/{userId}")]
        public async Task<IActionResult> GetMonthlyStats(int userId)
        {
            var stats = await _scorecardRepository.GetMonthlyStatsAsync(userId);
            return Ok(stats);
        }

    }
}
