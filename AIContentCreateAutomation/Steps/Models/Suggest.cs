namespace AIContentCreateAutomation.Steps.Models
{
    public class SuggestRequest
    {
        public string Keyword {  get; set; }
        public List<GoogleSearchResult> GoogleSearchResults {  get; set; }
    }

    public class SuggestResponse
    {        
        public List<string> Links { get; set; }
    }
}
