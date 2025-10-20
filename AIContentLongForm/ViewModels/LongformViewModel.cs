namespace AIContentLongForm.ViewModels
{
    public class LongformRequestViewModel
    {

        /*
         * Keyword or topic
         */
        public string Keyword {  get; set; }

        /*
         * Độ dài nội dung long-form
         */
        public int Length {  get; set; }

        /*
         *  Tone:
         *  "Trang trọng", "Thân mật", "Nhiệt tình",
            "Nghiêm túc", "Đồng cảm", "Hài hước", "Quyền uy", "Suy ngẫm"
            Trung lập, Tự tin, Thân thiện, Chuyên nghiệp, Thuyết phục, Giáo dục
         */
        public string Tone {  get; set; }


        /*
        Tin tức (News)
        Phóng sự (Feature)
        Bài bình luận (Opinion/Editorial)
        Bài phản ánh (Reportage/Reflection)
        Bài phỏng vấn (Interview)
        Bài điều tra (Investigative Report)
        Bài chân dung (Profile)
        Bài tường thuật (Report/Chronicle)
        Bài phê bình (Criticism)
        Blog/Bài viết cá nhân (Personal Blog/Column)
        Bài đánh giá (Review)
        Tiểu phẩm (Column)
        Bài phản hồi độc giả (Letters to the Editor)
         */
        public string Type {  get; set; }

        /*
          "Sinh viên đại học thế hệ Z", "Chuyên gia tiếp thị giàu kinh nghiệm", 
          "Công chúng quan tâm đến các vấn đề môi trường", 
          "Người cao tuổi có ít kinh nghiệm về công nghệ", "Nhân viên tiếp thị", 
          "Nhà phát triển", ""
         */
        public string Demographic { get; set; }

        public string Language { get; set; } = "Vietnam";

    }

    public class LongformCreateRequestViewModel: LongformRequestViewModel
    {

        public string OutlineContent {  get; set; }
        public string DiscoveryContent {  get; set; }

    }

    public class LongformResponse
    {
        public string Title { get; set; }        
        public List<string> Paragraphs { get; set; }
        public string Summary { get; set; }
    }
}
