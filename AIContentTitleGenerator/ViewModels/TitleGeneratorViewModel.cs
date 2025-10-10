namespace AIContentTitleGenerator.ViewModels
{
    public class TitleGeneratorRequestViewModel
    {
        public List<string> Keywords { get; set; }
        public string Context { get; set; }

        public string YourRequirement { get; set; }

        public int Keyword_Number{get;set;}

    }

    public sealed class TitleGeneratorResult
    {
        public List<string>? Results { get; set; }
    }
}
