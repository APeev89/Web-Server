using MyHTTPWebServer.HTTP;

namespace MyHTTPWebServer.Responses
{
    public class NotFoundResponse : Response
    {

        public NotFoundResponse() : base(StatusCode.NotFound)
        {
        }
    }
}
