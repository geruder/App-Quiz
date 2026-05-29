namespace vocabQuizAPI.Models
{
    public class AnswerRequest
    {
        public int UserId {  get; set; }
        public int WordId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
