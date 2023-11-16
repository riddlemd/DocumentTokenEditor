namespace DocumentTokenEditor.Tokenization
{
    public class TokenParserManifest
    {
        public Dictionary<string, TokenSettings> TokenSettings { get; set; } = new();

        public TokenFormat? TokenFormat { get; set; }

        public TokenSettings? GetTokenSettingsByName(string name)
            => TokenSettings.FirstOrDefault(kvp => kvp.Key == name).Value;
    }
}
