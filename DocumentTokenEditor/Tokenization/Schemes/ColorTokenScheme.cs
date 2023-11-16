namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class ColorTokenScheme : BaseTokenScheme
    {
        private const string _name = "Color";

        public ColorTokenScheme() : base(_name)
        {
            //
        }

        public override View GetEditorView(Token token)
        {
            var grid = new Grid
            {
                ColumnDefinitions = [
                    new()
                    {
                        Width = new GridLength(8, GridUnitType.Star)
                    },
                    new()
                ]
            };

            if (!Color.TryParse(token.TokenSettings?.DefaultValue, out var defaultColor))
                defaultColor = Colors.Black;

            var label = new Label
            {
                Text = token.TokenSettings?.DefaultValue == null ? "Invalid Color" : "",
                TextColor = Colors.White,
                BackgroundColor = defaultColor,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            grid.Add(label, 1, 0);

            var entry = new Entry()
            {
                Text = token.TokenSettings?.DefaultValue,
                MaxLength = token.TokenSettings?.MaxLength ?? int.MaxValue
            };

            entry.TextChanged += (s, e) =>
            {
                token.Value = e.NewTextValue;

                try
                {
                    var color = Color.Parse(e.NewTextValue);
                    label.BackgroundColor = color;
                    label.Text = "";
                }
                catch (Exception)
                {
                    label.BackgroundColor = Colors.Black;
                    label.Text = "Invalid Color";
                }
            };

            grid.Add(entry, 0, 0);

            return grid;
        }
    }
}
