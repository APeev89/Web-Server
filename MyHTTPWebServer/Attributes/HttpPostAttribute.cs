using MyHTTPWebServer.HTTP;

namespace MyHTTPWebServer.Attributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute() : base(Method.Post)
        {
        }
    }
}
