using MyHTTPWebServer.HTTP;


namespace MyHTTPWebServer.Responses
{
    public class UnauthorizedResponse : Response
    {

        public UnauthorizedResponse() : base(StatusCode.Unauthorized)
        {
        }
    }
}
