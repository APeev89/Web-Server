using MyHTTPWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHTTPWebServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {

        public HttpMethodAttribute(Method httpMethod)
       => HttpMethod = httpMethod;
        public Method HttpMethod { get; }
    }
}
