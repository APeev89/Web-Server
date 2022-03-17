using MyHTTPWebServer;
using MyHTTPWebServer.Controllers;

public class StartUp
{
    public static async Task Main()
    => await new HttpServer(routes => routes
        .MapControllers())
      .Start();

}

