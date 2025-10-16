namespace AIContentCreateAutomation.Steps.Models
{
    public class GoogleSearchResult
    {
        public string Title {  get; set; }
        public string Snippet { get; set; }
        public string Link { get; set; }
        public string DisplayLink { get; set; }
        public string Kind { get; set; }
    }

    public sealed class GoogleSearchResultState
    {
        internal List<GoogleSearchResult> Results { get; } = new();
        public string ResponseText { get; set; } = "";
    }
}
