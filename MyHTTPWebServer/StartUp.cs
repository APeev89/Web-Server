using MyHTTPWebServer;
using System.Net;

var server = new HttpServer("127.0.0.1", 8080);
server.Start();

//public class StartUp
//{
//    public static void Main()
//        => new HttpServer(routes =>
//        .MapGet("/", new TextResponse("Hello from the server!"))
//        .MapGet("HTML", new HtmlResponse("<h1>HTML respose</h1>"))
//        .MapGet("Redirect", new RedirectResponse("https://softuni.org/")))
//        .Start();

//}

