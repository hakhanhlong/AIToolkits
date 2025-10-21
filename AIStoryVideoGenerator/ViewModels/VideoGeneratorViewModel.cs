namespace AIStoryVideoGenerator.ViewModels
{
    public class StoryboardRequestViewModel
    {

        public int Num_Scenes {  get; set; }


        /*
         *  "children's story", câu chuyện thiếu nhi
            "adventure story", câu chuyện phiêu lưu
            "fairy tale", truyện cổ tích
            "sci-fi story", truyện khoa học viễn tưởng
            "fantasy story", câu chuyện tưởng tượng
            "mystery story", câu chuyện bí ẩn
            "fable", truyện ngụ ngôn
         */
        public string Style {  get; set; }


        /*
         * e.g., Một câu chuyện thiếu nhi vui nhộn về một chú robot vụng về cố gắng nướng bánh mừng sinh nhật người sáng tạo ra nó trong một căn bếp tương lai
         */
        public string IdeaText {  get; set; }

    }


    public class StoryboardResponseViewModel
    {

        public string Name {  get; set; }
        public List<StoryboardScene> Scenes {  get; set; }

    }

    public class StoryboardScene
    {        
        public string Description { get; set; }
        public string Narration { get; set; }
    }



    public class SceneImageRequestViewModel
    {
        public StoryboardResponseViewModel Storyboard { get; set; }


        /*
         *  "digital art", nghệ thuật số
            "cartoon", hoạt hình
            "watercolor", màu nước
            "photorealistic", # Changed 'realistic', giống như ảnh thật, thực tế
            "anime", phim hoạt hình
            "pixel art", nghệ thuật pixel
            "oil painting", tranh sơn dầu
            "line art", nghệ thuật đường nét
            "cinematic", điện ảnh
         */
        public string ImageStyle { get; set; }
    }

    public class SceneImageResponseViewModel
    {
        public string Name { get; set; }
        public List<SceneImage> SceneImages { get; set; } = new();
    }

    public class SceneImage
    {
        public string Description { get; set; }
        public string Narration { get; set; }
        public string Prompt { get; set; }
        public string PromptEnhanced { get; set; }
    }

}
