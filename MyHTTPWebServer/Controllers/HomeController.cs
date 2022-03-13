using MyHTTPWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHTTPWebServer.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(Request request):base(request)
        {

        }

        public Response Index() => Text("Hello from the server");
    }
}
