using DocumentTokenEditor.Tokenization.Schemes;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace DocumentTokenEditor.Tokenization
{
    public class TokenService : ITokenService
    {
        private readonly List<ITokenScheme> _schemes = [];
        private readonly IOptions<TokenServiceOptions> _tokenServiceOptions;

        public TokenService(IOptions<TokenServiceOptions> tokenServiceOptions)
        {
            _tokenServiceOptions = tokenServiceOptions;

            _schemes.Add(new TextTokenScheme());
            _schemes.Add(new ColorTokenScheme());
        }

        public List<Token> GetTokensFromString(string text)
        {
            var tokens = new List<Token>();

            var pattern = GetTokenStart() + @"([a-zA-Z0-9]+)" + GetTokenDivider() + @"([a-zA-Z0-9]+)" + GetTokenEnd();

            var matches = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.Compiled).Matches(text);

            foreach (Match match in matches.Cast<Match>())
            {
                var schema = ParseSchemaFromString(match.Groups[2].Value);

                if (schema == null)
                    continue;

                var name = match.Groups[1].Value;

                var token = new Token(name, schema);

                tokens.Add(token);
            }

            return tokens;
        }

        public string ApplyTokensToString(IEnumerable<Token> tokens, string text)
        {
            foreach(var token in tokens)
            {
                var pattern = GetTokenStart() + token.Name + GetTokenDivider() + token.Scheme.Name + GetTokenEnd();

                text = new Regex(pattern).Replace(text, token.Value, 1);
            }

            return text;
        }

        private ITokenScheme? ParseSchemaFromString(string? schemaName)
        {
            if (schemaName == null)
                return null;

            return _schemes.FirstOrDefault(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
        }

        private string GetTokenStart()
            => Regex.Escape(_tokenServiceOptions.Value.TokenStart);

        private string GetTokenEnd()
            => Regex.Escape(_tokenServiceOptions.Value.TokenEnd);

        private string GetTokenDivider()
            => Regex.Escape(_tokenServiceOptions.Value.TokenDivider);
    }
}
