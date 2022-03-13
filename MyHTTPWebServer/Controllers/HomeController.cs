using MyHTTPWebServer.HTTP;
using System.Text;
using System.Web;

namespace MyHTTPWebServer.Controllers
{
    public class HomeController : Controller
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private const string FileName = "content.txt";
        public HomeController(Request request) : base(request)
        {

        }

        public Response Index() => Text("Hello from the server");

        public Response Redirect() => Redirect("https://softuni.org/");
        public Response Html() => Html(HomeController.HtmlForm);
        public Response HtmlFormPost()
        {
            string formData = string.Empty;
            foreach (var (key,value) in this.Request.Form)
            {
                formData += $"{key} - {value}";
                formData += Environment.NewLine;
            }
            return Text(formData);
        }

        public Response Content() => Html(HomeController.DownloadForm);

        private static async Task DownloadSitesAsTextFile(string filneName, string[] urls)
        {
            List<Task<string>> downloads = new List<Task<string>>();
            foreach (string url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var responses = await Task.WhenAll(downloads);
            var responsesString = string.Join(
                Environment.NewLine + new String('-', 100), responses);
            await System.IO.File.WriteAllTextAsync(filneName, responsesString);
        }
        private static async Task<string> DownloadWebSiteContent(string url)
        {
            HttpClient httpClient = new HttpClient();
            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html.Substring(0, 2000);
            }
        }


        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(HomeController.FileName,
                new string[] { "https://judge.softuni.org/", "https://softuni.org/" })
                .Wait();

            return File(HomeController.FileName);
        }

        public Response Cookies()
        {
            if (this.Request.Cookies.Any(c => c.Name != MyHTTPWebServer.HTTP.Session.SessionCookieName))
            {
                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in this.Request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }
                cookieText.Append("</table>");
                return Html(cookieText.ToString());
            }
            var cookies = new CookieCollection();
            cookies.Add("My-Cookie", "My-Value");
            cookies.Add("My-Second-Cookie", "My-Second-Value");

            return Html("<h1>Cookies set!</h1>", cookies);
        }

        public Response Session()
        {
            string currentDateKey = "CurrentDate";
            var sessionExistis = this.Request.Session.ContainsKey(currentDateKey);
            if (sessionExistis)
            {
                var currentDate = this.Request.Session[currentDateKey];
                return Text($"Stoped date: {currentDate}!");
            }
            return Text("Current date stoped!");
        }
    }
}
