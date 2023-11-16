


namespace DocumentTokenEditor.Tokenization
{
    public interface ITokenService
    {
        string ApplyTokensToString(IEnumerable<Token> tokens, string? text, TokenParserManifest? parserManifest = null);
        List<Token> GetTokensFromString(string text, TokenParserManifest? parserManifest = null);
        Task<TokenParserManifest?> LoadParserManifestFromFileAsync(string uri);
    }
}