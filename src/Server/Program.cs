using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    class Program
    {
        private const int Port = 12000;

        static void Main(string[] args)
        {
            var dir = args[0];
            var indexBuilder = new IndexBuilder(int.Parse(args[1]));

            var stopwatch = Stopwatch.StartNew();
            var index = indexBuilder.UpdateFromPath(dir);
            stopwatch.Stop();
            Console.WriteLine($"Index build in {stopwatch.Elapsed}s");

            var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(ipPoint);
            listenSocket.Listen();

            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                var handler = listenSocket.Accept();
                var builder = new StringBuilder();
                var data = new byte[256];
                do
                {
                    builder.Append(Encoding.Unicode.GetString(data, 0, handler.Receive(data)));
                } while (handler.Available > 0);

                Console.WriteLine(builder.ToString());

                var req = JsonConvert.DeserializeObject<Request>(builder.ToString())
                          ?? throw new Exception("Serialization error");

                string resp;
                if (req.Operation == "get")
                {
                    var respObject = new Response()
                    {
                        Query = req.Query,
                        Instances = index.Get(req.Query)
                    };
                    resp = JsonConvert.SerializeObject(respObject);
                }
                else
                {
                    continue;
                }

                data = Encoding.Unicode.GetBytes(resp);
                handler.Send(data);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}