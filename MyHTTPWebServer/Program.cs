using MyHTTPWebServer;
using System.Net;

//var server = new HttpServer("127.0.0.1", 8080);
//server.Start();

var text = "test 123 test 345 test 567";
var newText = text.Split("\r\n");
Console.WriteLine(string.Join("..", newText));


