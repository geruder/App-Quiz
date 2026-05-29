namespace vocabQuizAPI.Models.Dtos
{
    public class CreateWordDto
    {
        public int UserId { get; set; } 
        public int CategoryId { get; set; }
        public string EnglishWord { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public string CefrLevel { get; set; } = "A1"; // default
    }
}
