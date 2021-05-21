using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Server;

namespace Client
{
    class Program
    {
        private const int Port = 12000;

        static void Main(string[] args)
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            var query = "";
            while (query != "\\q")
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                Console.Write("Input query:");
                query = Console.ReadLine();
                var reqObject = new Request
                {
                    Operation = "get",
                    Query = query
                };
                var data = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(reqObject));
                socket.Send(data);

                data = new byte[256];
                var builder = new StringBuilder();

                do
                {
                    builder.Append(Encoding.Unicode.GetString(data, 0,
                        socket.Receive(data, data.Length, 0)));
                } while (socket.Available > 0);

                var resp = JsonConvert.DeserializeObject<Response>(builder.ToString());
                Console.WriteLine($"Response:\n\tQuery - {resp.Query}\n\tCount - {resp.Count}");
                Console.WriteLine(String.Join("\n", resp.Instances));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}