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
                HeightRequest = 200
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
                token.Value = e.NewTextValue;

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
