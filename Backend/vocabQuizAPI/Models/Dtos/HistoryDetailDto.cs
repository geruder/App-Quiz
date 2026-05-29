namespace vocabQuizAPI.Models.Dtos
{
    public class HistoryDetailDto
    {
        public bool IsCorrect { get; set; }
        public DateTime AttemptAt { get; set; }
        public string EnglishWord { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
    }
}
