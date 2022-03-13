﻿using MyHTTPWebServer;
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

    private const string LoginForm = @"<form action='/Login' method='POST'>
   Username: <input type='text' name='Username'/>
   Password: <input type='text' name='Password'/>
   <input type='submit' value ='Log In' /> 
</form>";

    private const string Username = "user";
    private const string Password = "user123";
    public static async Task Main()
    =>await new HttpServer(routes => routes
       .MapGet<HomeController>("/", c => c.Index())
       .MapGet<HomeController>("/Redirect",c => c.Redirect())
       .MapGet<HomeController>("/HTML", c => c.Html())
       .MapPos<HomeController>t("/HTML", c => c.HtmlFormPost())
       .MapGet<HomeController>("/Content", c => c.Content())
       .MapPos<HomeController>t("/Content", c => c.DownloadContent())
       .MapGet<HomeController>("/Cookies", c => c.Cookies())
       .MapGet<HomeController>("/Session", c => c.Session()))
       //.MapGet<HomeController>("/Login", new HtmlResponse(StartUp.LoginForm))
       //.MapPos<HomeController>t("/Login", new HtmlResponse("", StartUp.LoginAction))
       //.MapGet<HomeController>("/Logout", new HtmlResponse("", StartUp.LogoutAction))
       //.MapGet<HomeController>("/UserProfile",new HtmlResponse("", StartUp.GetUserDataAction)))
      .Start();
    

    private static void GetUserDataAction(Request request, Response response)
    {
        if (request.Session.ContainsKey(Session.SessionUserKey))
        {
            response.Body = "";
            response.Body += $"<h3>Currently logged-in user is with username '{Username}'</h3>";
        }
        else
        {
            response.Body = "";
            response.Body += $"<h3>You should first log in " + 
                "- <a href='/Login'>Login</a></h3>";
        }

    }

    private static void LogoutAction(Request request, Response response)
    {
        request.Session.Clear();
        response.Body = "";
        response.Body += "<h3>Logged out successfully</h3>";
    }

    private static void LoginAction(Request request, Response response)
    {
        request.Session.Clear();

        var bodyText = "";
        var usernameMatches = request.Form["Username"] == StartUp.Username;
        var passwordMatches = request.Form["Password"] == StartUp.Password;
        if (usernameMatches && passwordMatches)
        {
            request.Session[Session.SessionUserKey] = "MyUserId";
            response.Cookies.Add(Session.SessionCookieName, request.Session.Id);
            bodyText = "<h3>Logged successfully!</h3>";
        }
        else
        {
            bodyText = StartUp.LoginForm;
        }

        response.Body = "";
        response.Body += bodyText;
    }

    private static void DisplaySessionInfoAction(Request request, Response response)
    {
        var sessionExistis = request.Session.ContainsKey(Session.SessionCurrentDateKey);

        var bodyText = "";

        if (sessionExistis)
        {
            var currentData = request.Session[Session.SessionCurrentDateKey];
            bodyText = $"Stoped date: {currentData}!";

        }
        else
        {
            bodyText = "Current date stoped!";
        }

        response.Body = "";
        response.Body += bodyText;

    }

    private static void AddCookiesAction(Request request, Response response)
    {
        var requstHasCookies = request.Cookies.Any(c => c.Name != Session.SessionCookieName);
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
            bodyText = "<h1>Cookies set!</h1>";
            
        }

        if (!requstHasCookies)
        {
            response.Cookies.Add("My-Cookie", "My-Value");
            response.Cookies.Add("My-Second-Cookie", "My-Second-Value");
        }

        response.Body = bodyText;
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

