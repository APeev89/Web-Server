using MyHTTPWebServer.HTTP;


namespace MyHTTPWebServer.Responses
{
    public class RedirectResponse : Response
    {
        public RedirectResponse(string location): base(StatusCode.Found)
        {
            this.Headers.Add(Header.Location, location);
        }
    }
}
