using MyHTTPWebServer.HTTP;
using MyHTTPWebServer.Routing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyHTTPWebServer
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        private readonly RoutingTable routingTable;

        public HttpServer(Action<IRoutingTable> routingTable) : this(8080, routingTable)
        {
        }

        public HttpServer(int port, Action<IRoutingTable> routingTable) : this("127.0.0.1", port, routingTable)
        {
        }

        public HttpServer(string ipAddress , int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, port);

            routingTableConfiguration(this.routingTable = new RoutingTable());
        }

        
       
        public void Start()
        {
            this.serverListener.Start();


            Console.WriteLine($"Server started on port {port}");
            Console.WriteLine($"Listening for requests...");


            while (true)
            {
                var connection = serverListener.AcceptTcpClient();

                var netWorkStream = connection.GetStream();
                string requestText = this.ReadRequest(netWorkStream);
                Console.WriteLine(requestText);

                var request = Request.Parse(requestText);
                var response = this.routingTable.MatchRequest(request);

                if (response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }
                WriteResponse(netWorkStream, response);

                

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream netWorkStream, Response response)
        {

            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            netWorkStream.Write(responseBytes);
        }

        private string ReadRequest(NetworkStream netWorkStream)
        {
            int bufferLenght = 1024;
            byte[] buffer = new byte[bufferLenght];

            StringBuilder requestBuilder = new StringBuilder();
            int totalBytes = 0;

            do
            {
                var bytesRead = netWorkStream.Read(buffer, 0, bufferLenght);

                totalBytes += bytesRead;
                
                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }
                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            } while (netWorkStream.DataAvailable);

            return requestBuilder.ToString();
        }
    }

    
}
