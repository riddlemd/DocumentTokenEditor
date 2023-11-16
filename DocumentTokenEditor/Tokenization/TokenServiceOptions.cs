namespace DocumentTokenEditor.Tokenization
{
    public class TokenServiceOptions
    {
        public string TokenStart { get; set; } = "{{";

        public string TokenEnd { get; set; } = "}}";

        public string TokenDivider { get; set; } = ":";
    }
}
