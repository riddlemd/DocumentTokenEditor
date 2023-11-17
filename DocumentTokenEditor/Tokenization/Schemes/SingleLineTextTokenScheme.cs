using System.Text.Encodings.Web;

namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class SingleLineTextTokenScheme : BaseTokenScheme
    {
        private const string _name = "SingleLineText";

        public SingleLineTextTokenScheme()
            : base(_name)
        {
            //
        }

        public override View GetEditorView(Token token)
        {
            var grid = new Grid();

            var maxLength = token.TokenSettings?.MaxLength ?? int.MaxValue;

            var entry = new Entry()
            {
                Text = token.TokenSettings?.DefaultValue,
                MaxLength = maxLength,
                Placeholder = token.TokenSettings?.Placeholder
            };

            var maxLengthLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.End,
                HorizontalTextAlignment = TextAlignment.End,
                Text = GetMaxLengthString(token.TokenSettings?.DefaultValue?.Length, maxLength),
                FontSize = 12
            };

            grid.Add(maxLengthLabel);

            entry.TextChanged += (s, e) =>
            {
                var value = e.NewTextValue;

                if (token.TokenSettings?.NlToBr ?? false)
                    value = value.Replace("\r\n", "<br>\r\n");

                token.Value = value;

                maxLengthLabel.Text = GetMaxLengthString(e.NewTextValue.Length, maxLength);
            };

            grid.Add(entry);

            return grid;
        }

        private static string? GetMaxLengthString(int? currentLength, int maxLength)
        {
            if (maxLength == int.MaxValue)
                return null;

            currentLength ??= 0;

            return $"{currentLength}/{maxLength}";
        }
    }
}
