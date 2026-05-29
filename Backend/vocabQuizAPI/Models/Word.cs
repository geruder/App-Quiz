namespace vocabQuizAPI.Models
{
    public class Word
    {
        public int WordId { get; set; }
        public int? CategoryId { get; set; } 
        public int? RootWordId { get; set; } 
        public int? CreatedBy { get; set; }  

        public string EnglishWord { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public string CefrLevel { get; set; } = string.Empty;
    }
}
