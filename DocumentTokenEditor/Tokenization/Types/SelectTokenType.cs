namespace DocumentTokenEditor.Tokenization.Types
{
    public class SelectTokenType : BaseTokenType
    {
        private const string _name = "Select";

        public SelectTokenType()
            : base(_name)
        {
        }

        public override View GetEditorView(Token token)
        {
            var options = token.TokenSettings?.SelectOptions ?? [];

            var picker = new Picker
            {
                ItemsSource = options.Keys.ToList()
            };

            picker.SelectedIndexChanged += (object? sender, EventArgs e) =>
            {
                token.Value = options[(string)picker.SelectedItem];
            };

            return picker;
        }
    }
}
