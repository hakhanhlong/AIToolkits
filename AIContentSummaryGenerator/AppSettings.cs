namespace AIContentSummaryGenerator
{
    public class AppSettings
    {
        public GeminiSettings GeminiSettings { get; set; }
    }


    public class GeminiSettings
    {
        public string APIKey { get; set; }
        public string ModelCode { get; set; }
    }
}
