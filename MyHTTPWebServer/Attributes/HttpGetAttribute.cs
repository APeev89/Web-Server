using MyHTTPWebServer.HTTP;

namespace MyHTTPWebServer.Attributes
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute() : base(Method.Get)
        {
        }
    }
}
