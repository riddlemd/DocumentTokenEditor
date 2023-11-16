namespace DocumentTokenEditor.Tokenization.Schemes
{
    public abstract class BaseTokenScheme(string name) : ITokenScheme
    {
        public string Name { get; private set; } = name;

        public abstract View GetView(Action<string> valueHandler);
    }
}
