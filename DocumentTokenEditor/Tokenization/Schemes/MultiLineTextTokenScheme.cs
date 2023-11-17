using System.Text.Encodings.Web;

namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class MultiLineTextTokenScheme : BaseTokenScheme
    {
        private const string _name = "MultiLineText";

        public MultiLineTextTokenScheme()
            : base(_name)
        {
            //
        }

        public override View GetEditorView(Token token)
        {
            var grid = new Grid();

            var maxLength = token.TokenSettings?.MaxLength ?? int.MaxValue;

            var editor = new Editor()
            {
                Text = token.TokenSettings?.DefaultValue,
                MaxLength = maxLength,
                HeightRequest = 200,
                Placeholder = token.TokenSettings?.Placeholder
            };

            var maxLengthLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.End,
                HorizontalTextAlignment = TextAlignment.Start,
                Text = GetMaxLengthString(token.TokenSettings?.DefaultValue?.Length, maxLength),
                FontSize = 12
            };

            grid.Add(maxLengthLabel);

            editor.TextChanged += (s, e) =>
            {
                var value = e.NewTextValue;

                if (token.TokenSettings?.NlToBr ?? false)
                    value = value.Replace("\r", "<br>\r");

                token.Value = value;

                maxLengthLabel.Text = GetMaxLengthString(e.NewTextValue.Length, maxLength);
            };

            grid.Add(editor);

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
