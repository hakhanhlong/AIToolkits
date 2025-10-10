namespace AIContentAnalyze.ViewModels
{
    public class PredictContentPerformanceRequestViewModel
    {
        public string Content {  get; set; }
    }

    public class PredictContentPerformanceViewModel
    {
        public List<Traffic_Prediction> Traffic_Predictions { get; set; }

        public List<Engagement_Prediction> Engagement_Predictions { get; set; }
        public List<Ranking_Prediction> Ranking_Predictions { get; set; }
        public List<Conversion_Prediction> Conversion_Predictions { get; set; }

        public List<string> Risk_Factors { get; set; }
        public List<string> Success_Factors { get; set; }

    }

    public class Traffic_Prediction
    {
        public string Estimated_Monthly_Traffic { get; set; }
        public string Traffic_Growth_Tate { get; set; }
        public string Peak_Traffic_Month { get; set; }
        public string Confidence_Level { get; set; }
    }

    public class Engagement_Prediction
    {
        public string Estimated_Time_On_Page { get; set; }
        public string Estimated_Bounce_Rate { get; set; }
        public string Estimated_Social_Shares { get; set; }
        public string Estimated_Comments { get; set; }
        public string Confidence_Level { get; set; }
    }

    public class Ranking_Prediction
    {
        public string Estimated_Ranking_Position { get; set; }
        public string Estimated_Ranking_Time { get; set; }
        public string Ranking_Confidence { get; set; }
        public string Competition_Level { get; set; }        
    }

    public class Conversion_Prediction
    {
        public string Estimated_Conversion_Rate { get; set; }
        public string Estimated_Lead_Generation { get; set; }
        public string Estimated_Revenue_Impact { get; set; }
        public string Confidence_Level { get; set; }
    }
}
