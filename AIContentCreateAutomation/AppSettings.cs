namespace AIContentCreateAutomation
{
    public class AppSettings
    {
        public GeminiSettings GeminiSettings { get; set; }
        public GoogleSearchSettings GoogleSearchSettings {  get; set; }
    }


    public class GeminiSettings
    {
        public string APIKey { get; set; }
        public string ModelCode { get; set; }
    }

    public class GoogleSearchSettings
    {
        public string APIKey { get; set; }
        public string SearchEngineId { get; set; }
    }
}
