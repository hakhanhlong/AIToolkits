namespace AIKeywordAnalyze.ViewModels
{
    public class KeywordAnalyzeRequestViewModel
    {
        public List<string> Keywords { get; set; }
        public string Content {  get; set; }
    }


    public class KeywordAnalyzeResponseViewModel
    {
        //Từ khóa chính
        public List<string> Primary { get; set; }
        //Từ khóa phụ
        public List<string> Secondary { get; set; }

        //Từ khóa dài
        public List<string> Long_Tail { get; set; }
       
        public string Search_Intent { get; set; }

        //Độ khó của từ khóa (1-10)
        public int Difficulty { get; set; }

        //Những nội dung khả năng đối thủ còn thiếu
        public List<string> Content_Gaps { get; set; }

        //Từ khóa ngữ nghĩa 
        public List<string> Semantic_Keywords { get; set; }

        //Thuật ngữ thịnh hành
        public List<string> Trending_Terms { get; set; }

        //Đưa ra phân tích chi tiết
        public string Analysis_Insights { get; set; }

    }
}
