namespace DocumentTokenEditor.Tokenization.Types
{
    public interface ITokenType
    {
        public string Name { get; }

        public View GetEditorView(Token token);
    }
}
