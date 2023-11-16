using CommunityToolkit.Maui.Converters;

namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class ColorTokenScheme : BaseTokenScheme
    {
        private const string _name = "Color";

        public ColorTokenScheme() : base(_name)
        {
            //
        }

        public override View GetView(Action<string> valueHandler)
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

            var label = new Label
            {
                Text = "Invalid Color",
                TextColor = Colors.White,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            grid.Add(label, 1, 0);

            var entry = new Entry();
            entry.TextChanged += (s, e) =>
            {
                valueHandler(e.NewTextValue);

                try
                {
                    var color = Color.Parse(e.NewTextValue);
                    label.BackgroundColor = color;
                    label.Text = "";
                }
                catch(Exception)
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
