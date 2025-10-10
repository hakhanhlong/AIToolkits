namespace AIContentAnalyze.ViewModels
{
    public class AnalyzeContentQualityRequestViewModel
    {
        public string Content {  get; set; }
    }

    public class AnalyzeContentQualityViewModel
    {
        public int Overall_Score { get; set; }
        public int Readability_Score {  get; set; }
        public int Seo_Score { get; set; }

        public string Engagement_Potential { get; set; }

        public List<string> Improvement_Suggestions { get; set; }
    }
}
