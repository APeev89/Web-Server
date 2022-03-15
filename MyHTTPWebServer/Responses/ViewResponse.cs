using MyHTTPWebServer.HTTP;

namespace MyHTTPWebServer.Responses
{
    public class ViewResponse : ContentResponse
    {
        private const char PathSeparator = '/';
        public ViewResponse(string viewName, string controllerName, object model = null) : base("", ContentType.Html)
        {
            if (!viewName.Contains(PathSeparator))
            {
                viewName = controllerName + PathSeparator + viewName;
            }
            var viewPath = Path.GetFullPath($"./Views/" + viewName.TrimStart(PathSeparator) + ".cshtml");
            var viewContent = File.ReadAllText(viewPath);

            this.Body = viewContent;

            if (model != null)
            {
                viewContent = this.PopulateModel(viewContent, model);
            }
            this.Body = viewContent;
        }

        private string PopulateModel(string viewContent, object model)
        {
            var data = model
                .GetType()
                .GetProperties()
                .Select(p => new
                {
                    p.Name,
                    Value = p.GetValue(model)
                });

            foreach (var item in data)
            {
                const string openingBrackets = "{{";
                const string closingBrackets = "}}";

                viewContent = viewContent.Replace($"{openingBrackets}{item.Name}{closingBrackets}",
                    item.Value.ToString());
            };

            return viewContent;
        }
    }
}
