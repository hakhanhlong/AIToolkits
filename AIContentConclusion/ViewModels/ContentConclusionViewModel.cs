namespace AIContentConclusion.ViewModels
{
    public class ContentConclusionRequestViewModel
    {

        /*
         [Specify the intended audience for the content.  Be as detailed as possible. Examples:
         "Gen Z college students," "Experienced marketing professionals," "General public interested in environmental
         issues," "Senior citizens with limited tech experience", "Marketers", "Developers"]

        "Sinh viên đại học thế hệ Z", "Chuyên gia tiếp thị giàu kinh nghiệm", "Công chúng quan tâm đến các vấn đề môi trường", 
        "Người cao tuổi có ít kinh nghiệm về công nghệ", "Nhân viên tiếp thị", 
        "Nhà phát triển"
         */
        public string Target_Audience { get; set; }


        /*
         * [Choose a tone that best suits the content. Options: "Formal," "Informal," "Enthusiastic,"
            "Serious," "Empathetic," "Humorous," "Authoritative," "Reflective"]

         "Trang trọng", "Thân mật", "Nhiệt tình",
        "Nghiêm túc", "Đồng cảm", "Hài hước", "Quyền uy", "Suy ngẫm"
        Trung lập, Tự tin, Thân thiện, Chuyên nghiệp, Thuyết phục, Giáo dục

         */
        public string Tone { get; set; } //Neutral,Confident,Friendly,Professional,Persuasive,Educational


        /*
         * Specify the desired writing style. Options: "Clear and concise," "Descriptive and evocative,"
          "Technical and precise," "Simple and accessible," "Academic"
           "Rõ ràng và súc tích", "Miêu tả và gợi cảm",
        "Kỹ thuật và chính xác", "Đơn giản và dễ hiểu", "Học thuật".
         */
        public string Style {  get; set; }


        /*
         * [Specify the desired length of the conclusion. Options: "Short (1-3 sentences),"
           "Medium (4-6 sentences)," "Long (7-10 sentences)," "Detailed (11+ sentences)"

            Các tùy chọn: "Ngắn (1-2 câu)", "Trung bình (3-5 câu)", "Dài (1-2 đoạn văn)", "Chi tiết (5+ đoạn văn)
         */
        public string Conclusion_Length { get; set; }


        /*
         * Examples: "Generate 3 distinct conclusions," "Provide 2 contrasting conclusions," "Produce 1 summary and 1 call to action."
         * "Đưa ra 3 kết luận khác biệt", "Đưa ra 2 kết luận trái ngược", "Đưa ra 1 bản tóm tắt và 1 lời kêu gọi hành động.
         */
        public string Number_Of_Conclusions { get; set; }


        public string Content {  get; set; }

    }

    public class ContentConclusionResponseViewModel
    {
        public List<string> Conclusions { get; set; }
    }
}
