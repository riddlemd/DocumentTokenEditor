using DocumentTokenEditor.Tokenization.Types;

namespace DocumentTokenEditor.Tokenization
{
    public class Token(string name, ITokenType type, TokenSettings? tokenSettings)
    {
        public string Name { get; } = name;

        public string? Value { get; set; } = tokenSettings?.DefaultValue;

        public ITokenType Type { get; } = type;

        public TokenSettings? TokenSettings { get; } = tokenSettings;
    }
}
