using DocumentTokenEditor.Tokenization.Types;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DocumentTokenEditor.Tokenization
{
    public class TokenService : ITokenService
    {
        private readonly List<ITokenType> _tokenTypes = [];
        private readonly IOptions<TokenServiceOptions> _tokenServiceOptions;

        public TokenService(IOptions<TokenServiceOptions> tokenServiceOptions)
        {
            _tokenServiceOptions = tokenServiceOptions;

            _tokenTypes.Add(new SingleLineTextTokenType());
            _tokenTypes.Add(new MultiLineTextTokenType());
            _tokenTypes.Add(new ColorTokenType());
            _tokenTypes.Add(new SelectTokenType());
        }

        public List<Token> GetTokensFromString(string text, TokenParserManifest? parserManifest = null)
        {
            var tokens = new List<Token>();

            var pattern = string.Format("{0}([a-zA-Z0-9]+)({1})?([a-zA-Z0-9]*){2}", GetTokenStart(parserManifest?.TokenFormat), GetTokenDivider(parserManifest?.TokenFormat), GetTokenEnd(parserManifest?.TokenFormat));

            var matches = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.Compiled).Matches(text);

            foreach (Match match in matches.Cast<Match>())
            {
                var name = match.Groups[1].Value;

                var tokenSettings = parserManifest?.GetTokenSettingsByName(name);

                var typeName = !string.IsNullOrWhiteSpace(match.Groups[3].Value) ? match.Groups[3].Value : tokenSettings?.Type;

                var type = ParseSchemaFromString(typeName);

                type ??= _tokenTypes.First();

                var token = new Token(name, type, tokenSettings);

                tokens.Add(token);
            }

            return tokens;
        }

        public string? ApplyTokensToString(IEnumerable<Token>? tokens, string? text, TokenParserManifest? parserManifest = null)
        {
            if (tokens is null)
                return text;

            if (text is null)
                return text;

            foreach (var token in tokens)
            {
                var pattern = string.Format("{0}{1}({2})?[^{3}]*{3}", GetTokenStart(parserManifest?.TokenFormat), token.Name, GetTokenDivider(parserManifest?.TokenFormat), GetTokenEnd(parserManifest?.TokenFormat));

                text = new Regex(pattern).Replace(text, token.Value ?? "", 1);
            }

            return text;
        }

        public async Task<TokenParserManifest?> LoadParserManifestFromFileAsync(string uri)
        {
            try
            {
                if (!File.Exists(uri))
                    return null;

                var json = await File.ReadAllTextAsync(uri);

                var settings = JsonSerializer.Deserialize<TokenParserManifest>(json);

                return settings;
            }
            catch (Exception)
            {
                // Do nothing
            }

            return null;
        }

        private ITokenType? ParseSchemaFromString(string? schemaName)
        {
            if (schemaName == null)
                return null;

            return _tokenTypes.FirstOrDefault(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
        }

        private string GetTokenStart(TokenFormat? tokenFormat = null)
        {
            tokenFormat ??= _tokenServiceOptions.Value.DefaultTokenFormat;

            return Regex.Escape(tokenFormat.Start);
        }

        private string GetTokenEnd(TokenFormat? tokenFormat = null)
        {
            tokenFormat ??= _tokenServiceOptions.Value.DefaultTokenFormat;

            return Regex.Escape(tokenFormat.End);
        }

        private string GetTokenDivider(TokenFormat? tokenFormat = null)
        {
            tokenFormat ??= _tokenServiceOptions.Value.DefaultTokenFormat;

            return Regex.Escape(tokenFormat.Divider);
        }
    }
}
