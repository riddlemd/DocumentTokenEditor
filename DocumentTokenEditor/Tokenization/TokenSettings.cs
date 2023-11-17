namespace DocumentTokenEditor.Tokenization
{
    public class TokenSettings
    {
        public string? Scheme { get; init; }

        public string? DefaultValue { get; init; }

        public int? MaxLength { get; init; }

        public string? Placeholder { get; init; }

        public Dictionary<string, string>? Options { get; init; }

        public bool? NewlineToBr { get; init;  }
    }
}
