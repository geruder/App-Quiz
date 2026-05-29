namespace vocabQuizAPI.Models
{
    public class UserWordProgress
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int WordId { get; set; }
        public string Status { get; set; } = "review";
        public int Streak { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
