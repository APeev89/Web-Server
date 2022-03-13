using MyHTTPWebServer.HTTP;


namespace MyHTTPWebServer.Responses
{
    public class TextResponse: ContentResponse
    {
        public TextResponse(string text)
            : base(text, ContentType.PlaintText)
        {
        }
    }
}
