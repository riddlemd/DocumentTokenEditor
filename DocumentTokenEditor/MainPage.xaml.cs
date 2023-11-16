using DocumentTokenEditor.Tokenization;
using System.Text.Json;

namespace DocumentTokenEditor
{
    public partial class MainPage : ContentPage
    {
        private readonly ITokenService _tokenService;
        private string _fileContent;
        private List<Token> _tokens;

        public MainPage(ITokenService tokenService)
        {
            InitializeComponent();
            _tokenService = tokenService;
        }

        private async void LoadFileFlyoutItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var options = new PickOptions();

                var result = await FilePicker.Default.PickAsync(options);

                if (result == null)
                    return;

                TargetUri.Text = result.FullPath;

                _fileContent = await File.ReadAllTextAsync(result.FullPath);

                _tokens = _tokenService.GetTokensFromString(_fileContent);

                TokenBorder.IsVisible = true;

                TokenStack.Clear();

                if (_tokens.Count == 0)
                {
                    TokenStack.Add(new Label()
                    {
                        Text = "No Tokens found in file",
                        HorizontalTextAlignment = TextAlignment.Center
                    });

                    return;
                }

                foreach(var token in _tokens)
                {
                    var grid = new Grid()
                    {
                        ColumnDefinitions = [
                            new()
                            {
                                Width = 200
                            },
                            new()
                        ],
                    };

                    var label = new Label
                    {
                        Text = token.Name,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    grid.Add(label, 0, 0);

                    var view = token.Scheme.GetView((value) =>
                    {
                        token.Value = value;
                    });

                    view.HorizontalOptions = new()
                    {
                        Alignment = LayoutAlignment.Fill,
                        Expands = true,
                    };

                    grid.Add(view, 1, 0);

                    var border = new Border
                    {
                        Padding = new(10),
                        Margin = new(0, 10),
                        Content = grid,
                    };

                    TokenStack.Add(border);
                }
            }
            catch (Exception ex) { }
            {
                // The user canceled or something went wrong
            }
        }

        private void SaveFileFlyoutItem_Clicked(object sender, EventArgs e)
        {
            var output = _tokenService.ApplyTokensToString(_tokens, _fileContent);

            System.Diagnostics.Debug.WriteLine(output);
        }
    }

}
