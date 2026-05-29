namespace vocabQuizAPI.Models
{
    public class ContactRequest
    {
        public int RequestId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ContactInfoCombined { get; set; } = string.Empty;
    }
}