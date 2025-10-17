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



    public class WritingReviseRequest
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public List<string> Paragraphs { get; set; }
        public string Summary { get; set; }
        public List<string> Critiques { get; set; }
    }


    public class WritingReviseResponse
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public List<string> Paragraphs { get; set; }
        public string Summary { get; set; }
        public string Message { get; set; }
    }

}
