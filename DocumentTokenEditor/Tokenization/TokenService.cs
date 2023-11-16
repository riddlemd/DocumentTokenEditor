using DocumentTokenEditor.Tokenization.Schemes;
using Microsoft.Extensions.Options;
using System.Text.Json;
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

        public List<Token> GetTokensFromString(string text, TokenParserManifest? parserManifest = null)
        {
            var tokens = new List<Token>();

            var pattern = GetTokenStart(parserManifest?.TokenFormat) + @"([a-zA-Z0-9]+)(" + GetTokenDivider(parserManifest?.TokenFormat) + @")?([a-zA-Z0-9]*)" + GetTokenEnd(parserManifest?.TokenFormat);

            var matches = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.Compiled).Matches(text);

            foreach (Match match in matches.Cast<Match>())
            {
                var name = match.Groups[1].Value;

                var tokenSettings = parserManifest?.GetTokenSettingsByName(name);

                var schemeName = !string.IsNullOrWhiteSpace(match.Groups[3].Value) ? match.Groups[3].Value : tokenSettings?.Scheme;

                var schema = ParseSchemaFromString(schemeName);

                schema ??= _schemes.First();

                var token = new Token(name, schema, tokenSettings);

                tokens.Add(token);
            }

            return tokens;
        }

        public string ApplyTokensToString(IEnumerable<Token> tokens, string text, TokenParserManifest? parserManifest = null)
        {
            if (text is null)
                return text;

            foreach (var token in tokens)
            {
                var pattern = GetTokenStart(parserManifest?.TokenFormat) + token.Name + @":?[^" + GetTokenEnd(parserManifest?.TokenFormat) + @"]*" + GetTokenEnd(parserManifest?.TokenFormat);

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
            catch (Exception ex)
            {
                return null;
            }
        }

        private ITokenScheme? ParseSchemaFromString(string? schemaName)
        {
            if (schemaName == null)
                return null;

            return _schemes.FirstOrDefault(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
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
