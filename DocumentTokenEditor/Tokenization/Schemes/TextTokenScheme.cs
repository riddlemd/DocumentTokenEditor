namespace DocumentTokenEditor.Tokenization.Schemes
{
    public class TextTokenScheme : BaseTokenScheme
    {
        private const string _name = "Text";

        public TextTokenScheme()
            : base(_name)
        {
            //
        }

        public override View GetEditorView(Action<string> valueHandler)
        {
            var view = new Entry();
            view.TextChanged += (s, e) =>
            {
                valueHandler(e.NewTextValue);
            };

            return view;
        }
    }
}
