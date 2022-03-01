using MyHTTPWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHTTPWebServer.Responses
{
    public class TextResponse: ContentResponse
    {
        public TextResponse(string text): base(text, ContentType.PlaintText)
        {

        }
    }
}
