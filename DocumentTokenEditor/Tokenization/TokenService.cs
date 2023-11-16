using DocumentTokenEditor.Tokenization.Schemes;
using System.Text.RegularExpressions;

namespace DocumentTokenEditor.Tokenization
{
    public class TokenService : ITokenService
    {
        private readonly List<ITokenScheme> _schemes = [];

        public TokenService()
        {
            _schemes.Add(new TextTokenScheme());
            _schemes.Add(new ColorTokenScheme());
        }

        public List<Token> GetTokensFromString(string text)
        {
            var tokens = new List<Token>();

            var matches = new Regex(@"\{\{([a-zA-Z0-9]+):([a-zA-Z0-9]+)\}\}", RegexOptions.CultureInvariant | RegexOptions.Compiled).Matches(text);

            foreach (Match match in matches)
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
                text = new Regex("{{" + $"{token.Name}:{token.Scheme.Name}" + "}}").Replace(text, token.Value, 1);
            }

            return text;
        }

        private ITokenScheme? ParseSchemaFromString(string? schemaName)
        {
            if (schemaName == null)
                return null;

            return _schemes.FirstOrDefault(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
