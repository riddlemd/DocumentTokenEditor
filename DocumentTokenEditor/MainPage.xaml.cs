using CommunityToolkit.Maui.Storage;
using DocumentTokenEditor.Tokenization;
using Microsoft.Maui.Layouts;
using System.Text;

namespace DocumentTokenEditor
{
    public partial class MainPage : ContentPage
    {
        private readonly ITokenService _tokenService;
        private string? _fileContent;
        private TokenParserManifest _tokenParserManifest;
        private List<Token>? _tokens;

        public MainPage(ITokenService tokenService)
        {
            _tokenService = tokenService;

            InitializeComponent();
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

                var fileTokenSettingsUri = $"{result.FullPath}.dtes";

                _tokenParserManifest = await _tokenService.LoadParserManifestFromFileAsync(fileTokenSettingsUri);

                _tokens = _tokenService.GetTokensFromString(_fileContent, _tokenParserManifest);

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

                    var flex = new FlexLayout
                    {
                        Direction = FlexDirection.Column,
                        VerticalOptions = new()
                        {
                            Alignment = LayoutAlignment.Center,
                        },
                        HorizontalOptions = new()
                        {
                            Alignment = LayoutAlignment.Center
                        }
                    };

                    var nameLabel = new Label
                    {
                        Text = token.Name,
                        VerticalTextAlignment = TextAlignment.Center,
                        LineBreakMode = LineBreakMode.TailTruncation
                    };

                    flex.Add(nameLabel);

                    var typeLabel = new Label
                    {
                        Text = token.Scheme.Name,
                        VerticalTextAlignment = TextAlignment.Center,
                        LineBreakMode = LineBreakMode.TailTruncation,
                        FontSize = 8,
                        TextColor = new Color(255, 255, 255, 128)
                    };

                    flex.Add(typeLabel);

                    grid.Add(flex, 0, 0);

                    var view = token.Scheme.GetEditorView(token);

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
            var outputContent = _tokenService.ApplyTokensToString(_tokens, _fileContent, _tokenParserManifest);

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(outputContent));

            var fileInfo = new FileInfo(TargetUri.Text);

            var outputName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}_output{fileInfo.Extension}";

            var startingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var result = await FileSaver.Default.SaveAsync(startingDirectory, outputName, ms);

            if (result == null)
                return;

            System.Diagnostics.Debug.WriteLine(outputContent);
        }
    }

}
