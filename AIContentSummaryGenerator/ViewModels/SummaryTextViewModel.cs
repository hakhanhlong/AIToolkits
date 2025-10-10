namespace AIContentSummaryGenerator.ViewModels
{
    public class SummaryTextRequestViewModel
    {
        public int TargetWords {  get; set; }
        public string Content {  get; set; }

        public int KeywordNumber {  get; set; }
    }

    public class SummaryTextResult
    {
        public string SummaryText {  get; set; }
        public List<string> Keywords {  get; set; }
    }
}
