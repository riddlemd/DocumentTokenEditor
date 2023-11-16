using DocumentTokenEditor.Tokenization.Schemes;

namespace DocumentTokenEditor.Tokenization
{
    public class Token(string name, ITokenScheme scheme)
    {
        public string Name { get; set; } = name;

        public string Value { get; set; } = "";

        public ITokenScheme Scheme { get; set; } = scheme;
    }
}
