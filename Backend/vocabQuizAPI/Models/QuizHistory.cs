namespace vocabQuizAPI.Models
{
    public class QuizHistory
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public int WordId { get; set; }
        public bool IsCorrect { get; set; }
        public string QuizMode { get; set; } = "quiz";
        public DateTime AttemptAt { get; set; }
    }
}