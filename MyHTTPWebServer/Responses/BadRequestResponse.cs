using MyHTTPWebServer.HTTP;


namespace MyHTTPWebServer.Responses
{
    public class BadRequestResponse : Response
    {
        public BadRequestResponse(): base( StatusCode.BadRequest)
        {
        }
    }
}
