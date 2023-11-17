using CommunityToolkit.Maui.Storage;
using DocumentTokenEditor.Templating;
using DocumentTokenEditor.Tokenization;
using Microsoft.Maui.Layouts;
using System.Text;

namespace DocumentTokenEditor
{
    public partial class MainPage : ContentPage
    {
        private readonly ITokenService _tokenService;
        private readonly ITemplateService _templateService;
        private string? _fileContent;
        private TokenParserManifest? _tokenParserManifest;
        private List<Token>? _tokens;

        public MainPage(ITokenService tokenService, ITemplateService templateService)
        {
            _tokenService = tokenService;
            _templateService = templateService;

            InitializeComponent();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            var templateMenuBarItems = _templateService.GetMenuItems();

            if (templateMenuBarItems.Count > 0)
            {
                TemplatesMenuBarItem.Clear();

                foreach (var templateMenubarItem in templateMenuBarItems)
                {
                    templateMenubarItem.Clicked += async (s, e) =>
                    {
                        await LoadFileAsync(templateMenubarItem.FullPath);
                    };

                    TemplatesMenuBarItem.Add(templateMenubarItem);
                }
            }
        }
        private async Task LoadFileAsync(string? uri)
        {
            if (string.IsNullOrEmpty(uri?.Trim()))
                return;

            TargetUri.Text = uri;

            _fileContent = await File.ReadAllTextAsync(uri);

            var fileTokenSettingsUri = $"{uri}.dtes";

            try
            {
                _tokenParserManifest = await _tokenService.LoadParserManifestFromFileAsync(fileTokenSettingsUri);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to parse .dtes file!", "Ok");
            }

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
                    Text = token.Type.Name,
                    VerticalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    FontSize = 8,
                    TextColor = new Color(255, 255, 255, 128)
                };

                flex.Add(typeLabel);

                grid.Add(flex, 0, 0);

                var view = token.Type.GetEditorView(token);

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

        private async void LoadFileFlyoutItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var options = new PickOptions();

                var result = await FilePicker.Default.PickAsync(options);

                if (result == null)
                    return;

                await LoadFileAsync(result.FullPath);
            }
            catch (Exception) { }
            {
                // The user canceled or something went wrong
            }
        }

        private async void SaveFileFlyoutItem_Clicked(object sender, EventArgs e)
        {
            var outputContent = _tokenService.ApplyTokensToString(_tokens, _fileContent, _tokenParserManifest);

            if (string.IsNullOrEmpty(outputContent))
                return;

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
