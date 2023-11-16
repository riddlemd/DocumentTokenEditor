namespace DocumentTokenEditor.Tokenization.Schemes
{
    public interface ITokenScheme
    {
        public string Name { get; }

        public View GetEditorView(Action<string> valueHandler);
    }
}
