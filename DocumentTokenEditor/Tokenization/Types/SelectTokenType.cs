namespace DocumentTokenEditor.Tokenization.Types
{
    public class SelectTokenType : BaseTokenType
    {
        private const string _name = "Select";

        public SelectTokenType()
            : base(_name)
        {
            //
        }

        public override View GetEditorView(Token token)
        {
            var options = token.TokenSettings?.SelectOptions ?? [];

            var optionsList = options.Keys?.ToList();

            var selectedIndex = optionsList?.IndexOf(token.TokenSettings?.DefaultValue ?? "") ?? -1;

            var picker = new Picker
            {
                ItemsSource = optionsList,
                SelectedIndex = selectedIndex
            };

            picker.SelectedIndexChanged += (object? sender, EventArgs e) =>
            {
                token.Value = options[(string)picker.SelectedItem];
            };

            return picker;
        }
    }
}
