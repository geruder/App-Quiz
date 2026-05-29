namespace vocabQuizAPI.Models
{
    public class QuizResponse
    {
        public int WordId { get; set; }
        public string EnglishWord { get; set; } = string.Empty;
        public string CorrectMeaning { get; set; } = string.Empty;
        public string CefrLevel { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
    }
}
