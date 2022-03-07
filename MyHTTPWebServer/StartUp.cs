using MyHTTPWebServer;
using MyHTTPWebServer.HTTP;
using MyHTTPWebServer.Responses;
using System.Net;
using System.Text;
using System.Web;

public class StartUp
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
    public static async Task Main()
    {
        await DownloadSitesAsTextFile(StartUp.FileName,
            new string[] { "https://judge.softuni.org/", "https://softuni.org/" });

        var server = new HttpServer(routes => routes
       .MapGet("/", new TextResponse("Hello from the server!"))
       .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
       .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
       .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
       .MapGet("/Content", new HtmlResponse(StartUp.DownloadForm))
       .MapPost("/Content", new TextFileResponse(StartUp.FileName))
       .MapGet("/Cookies", new HtmlResponse("", StartUp.AddCookiesAction)));

        await server.Start();
    }


    private static void AddCookiesAction(Request request, Response response)
    {
        var requstHasCookies = request.Cookies.Any();
        var bodyText = "";

        if (requstHasCookies)
        {
            var cookieText = new StringBuilder();
            cookieText.AppendLine("<h1>Cookies</h1>");

            cookieText.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

            foreach (var cookie in request.Cookies)
            {
                cookieText.AppendLine("<tr>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                cookieText.AppendLine("</tr>");
            }
            cookieText.Append("</table>");

            bodyText = cookieText.ToString();

        }
        else
        {
            bodyText = "<h1>No cookies yet!</h1>";
        }

        if (!requstHasCookies)
        {
            response.Cookies.Add("My-Cookie", "My-Value");
            response.Cookies.Add("My-Second-Cookie", "My-Second-Value");
        }
    }
    private static void AddFormDataAction(Request request, Response response)
    {
        response.Body = "";

        foreach (var (key, value) in request.Form)
        {
            response.Body += $"{key} - {value}";
            response.Body += Environment.NewLine;
        }
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
        await File.WriteAllTextAsync(filneName, responsesString);
    }
}

