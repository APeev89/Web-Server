using MyHTTPWebServer.HTTP;

namespace MyHTTPWebServer.Responses
{
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string text)
            : base(text, ContentType.Html)
        {

        }
    }
}
