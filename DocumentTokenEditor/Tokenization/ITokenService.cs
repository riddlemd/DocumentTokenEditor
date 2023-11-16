

namespace DocumentTokenEditor.Tokenization
{
    public interface ITokenService
    {
        string ApplyTokensToString(IEnumerable<Token> tokens, string text);
        List<Token> GetTokensFromString(string text);
    }
}