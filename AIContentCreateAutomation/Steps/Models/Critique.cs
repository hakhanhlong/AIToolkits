namespace AIContentCreateAutomation.Steps.Models
{
    public class CritiqueRequest
    {
        public WritingResponse WritingResponse { get; set; }
    }

    public class CritiqueResponse
    {
        public List<string> Critiques { get; set; }
        public List<string> Paragraphs { get; set; }
    }
}
