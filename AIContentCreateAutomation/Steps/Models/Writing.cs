namespace AIContentCreateAutomation.Steps.Models
{
    public class WritingRequest
    {
        public string Keyword {  get; set; }
        public List<string> Links { get; set; }

        public List<GoogleSearchResult> GoogleSearchResults {  get; set; }
    }


    public class WritingResponse
    {
        public string Title { get; set; }
        public string Date {  get; set; }
        public List<string> Paragraphs {  get; set; }

        public string Summary { get; set; }
    }
}
