namespace vocabQuizAPI.Models.Dtos
{
    public class UpdateWordDto
    {
        public int RequestingUserId { get; set; } 
        public int CategoryId { get; set; }
        public string EnglishWord { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public string CefrLevel { get; set; } = string.Empty;
    }
}
