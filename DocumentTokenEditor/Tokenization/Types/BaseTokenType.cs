namespace DocumentTokenEditor.Tokenization.Types
{
    public abstract class BaseTokenType(string name) : ITokenType
    {
        public string Name { get; private set; } = name;

        public abstract View GetEditorView(Token token);
    }
}
