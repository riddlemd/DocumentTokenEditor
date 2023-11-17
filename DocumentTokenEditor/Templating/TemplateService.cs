using Microsoft.Extensions.Options;

namespace DocumentTokenEditor.Templating
{
    public class TemplateService : ITemplateService
    {
        private readonly IOptions<TemplateServiceOptions> _templateServiceOptions;

        public TemplateService(IOptions<TemplateServiceOptions> templateServiceOptions)
        {
            _templateServiceOptions = templateServiceOptions;
        }

        public List<TemplateMenuFlyoutItem> GetMenuItems()
        {
            var menuItems = new List<TemplateMenuFlyoutItem>();

            var files = Directory.EnumerateFiles(_templateServiceOptions.Value.Directory);

            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);

                if (name == "EMPTY")
                    continue;

                var extension = Path.GetExtension(file);

                if (extension == ".dtes")
                    continue;

                var menuFlyoutItem = new TemplateMenuFlyoutItem
                {
                    Text = name,
                    FullPath = file
                };

                menuItems.Add(menuFlyoutItem);
            }

            return menuItems;
        }
    }
}
