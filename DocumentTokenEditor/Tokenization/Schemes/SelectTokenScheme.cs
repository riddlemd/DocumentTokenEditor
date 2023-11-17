namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class SelectTokenScheme : BaseTokenScheme
    {
        private const string _name = "Select";

        public SelectTokenScheme()
            : base(_name)
        {
        }

        public override View GetEditorView(Token token)
        {
            var options = token.TokenSettings?.Options ?? [];

            var picker = new Picker();
            picker.ItemsSource = options.Keys.ToList();

            picker.SelectedIndexChanged += (object? sender, EventArgs e) =>
            {
                token.Value = options[(string)picker.SelectedItem];
            };

            return picker;
        }
    }
}
