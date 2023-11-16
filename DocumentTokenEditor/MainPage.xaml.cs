using CommunityToolkit.Maui.Storage;
using DocumentTokenEditor.Tokenization;
using System.Text;

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

                    SaveFileFlyoutItem.IsEnabled = false;

                    return;
                }

                SaveFileFlyoutItem.IsEnabled = true;

                foreach (var token in _tokens)
                {
                    var grid = new Grid()
                    {
                        ColumnDefinitions = [
                            new()
                            {
                                Width = new GridLength(1, GridUnitType.Star)
                            },
                            new()
                            {
                                Width = new GridLength(5, GridUnitType.Star)
                            }
                        ],
                    };

                    var label = new Label
                    {
                        Text = token.Name,
                        VerticalTextAlignment = TextAlignment.Center,
                        LineBreakMode = LineBreakMode.TailTruncation
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
                        Margin = new(0, 5),
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

        private async void SaveFileFlyoutItem_Clicked(object sender, EventArgs e)
        {
            var outputContent = _tokenService.ApplyTokensToString(_tokens, _fileContent);

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(outputContent));

            var fileInfo = new FileInfo(TargetUri.Text);

            var outputName = $"{fileInfo.Name[..(fileInfo.Extension.Length+1)]}_output{fileInfo.Extension}";

            var startingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var result = await FileSaver.Default.SaveAsync(startingDirectory, outputName, ms);

            if (result == null)
                return;

            System.Diagnostics.Debug.WriteLine(outputContent);
        }
    }

}
