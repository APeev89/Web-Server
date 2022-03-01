using MyHTTPWebServer;
using MyHTTPWebServer.Responses;
using System.Net;



public class StartUp
{
    public static void Main()
        => new HttpServer(routes => routes
        .MapGet("/", new TextResponse("Hello from the server!"))
        .MapGet("HTML", new HtmlResponse("<h1>HTML respose</h1>"))
        .MapGet("Redirect", new RedirectResponse("https://softuni.org/")))
        .Start();

}

