using MyHTTPWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHTTPWebServer.Controllers
{
    public abstract class Controller
    {
        protected Controller()
        {

        }

        public Request Request { get; private init; }
    }
}
