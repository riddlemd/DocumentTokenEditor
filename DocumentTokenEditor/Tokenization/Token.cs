using DocumentTokenEditor.Tokenization.Schemes;

namespace DocumentTokenEditor.Tokenization
{
    public class Token(string name, ITokenScheme scheme, TokenSettings? tokenSettings)
    {
        public string Name { get; } = name;

        public string? Value { get; set; } = tokenSettings?.DefaultValue;

        public ITokenScheme Scheme { get; } = scheme;

        public TokenSettings? TokenSettings { get; } = tokenSettings;
    }
}
