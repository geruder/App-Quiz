namespace vocabQuizAPI.Models
{
    public class DailyScorecard
    {
        public int ScorecardId { get; set; }
        public int UserId { get; set; }
        public DateTime DateRecorded { get; set; }
        public int TotalSeen { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalWrong { get; set; }
    }
}