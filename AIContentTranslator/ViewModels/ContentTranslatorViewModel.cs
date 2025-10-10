namespace AIContentTranslator.ViewModels
{
    public class ContentTranslatorRequestViewModel
    {
        public string FromLanguage {  get; set; }
        public string ToLanguage { get; set; }
        public string Content {  get; set; }

    }

    public class ContentTranslatorResponseViewModel
    {
        public string FromLanguage { get; set; }
        public string ToLanguage { get; set; }
        public string TranslateText { get; set; }

    }
}
