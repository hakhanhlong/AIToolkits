namespace AIContentSpellGrammaticalCheck.ViewModels
{
    public class SpellGrammaticalCheckRequestViewModel
    {
        public string Content {  get; set; }
    }


    public class SpellGrammaticalCheckResult
    {
        public string CorrectText { get; set; }

        public List<SpellGrammaticalCheckReplacement> Replacements { get; set; }

    }

    public class SpellGrammaticalCheckReplacement
    {
        public string ChangeFrom { get; set; }
        public string ChangeTo { get; set; }
        public string Reason { get; set; }
    }
}
