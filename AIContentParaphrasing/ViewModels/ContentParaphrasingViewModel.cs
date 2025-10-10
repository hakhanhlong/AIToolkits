using System;
using System.Net.NetworkInformation;

namespace AIContentParaphrasing.ViewModels
{

    //Cân bằng: Diễn đạt tự nhiên mà vẫn giữ nguyên ý nghĩa
    //Trang trọng: Giọng điệu chuyên nghiệp, học thuật
    //Thường ngày: Phong cách trò chuyện, thân thiện
    //Ngắn gọn: Phiên bản ngắn gọn, trực tiếp hơn
    //Chi tiết: Nội dung được mở rộng, trau chuốt hơn

    //Balanced: Natural paraphrasing while maintaining meaning
    //Formal: Professional, academic tone
    //Casual: Conversational, friendly style
    //Concise: Shorter, more direct version
    //Detailed: Expanded, more elaborate content

    public class ContentParaphrasingRequestViewModel
    {

        public string Style { get; set; }
        public string Content {  get; set; }
    }

    public class ContentParaphrasingResponseViewModel
    {        
        public string RephraseText { get; set; }
    }
}
