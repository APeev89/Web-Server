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

        public HttpServer(string ipAddress , int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, port);
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
                string content = "Hello from the server!";

                WriteResponse(netWorkStream, content);

                

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream netWorkStream, string content)
        {
            int contentLength = Encoding.UTF8.GetByteCount(content);

            string response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{content}";

            var responseBytes = Encoding.UTF8.GetBytes(response);

            netWorkStream.Write(responseBytes, 0, responseBytes.Length);
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
