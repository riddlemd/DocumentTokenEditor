namespace DocumentTokenEditor.Tokenization
{
    public class TokenSettings
    {
        public string? Type { get; init; }

        public string? DefaultValue { get; init; }

        public int? MaxLength { get; init; }

        public string? Placeholder { get; init; }

        public Dictionary<string, string>? SelectOptions { get; init; }

        public bool? NewlineToBr { get; init; }
    }
}
