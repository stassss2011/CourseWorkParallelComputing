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

        static void Main(string[] args)
        {
            var dir = args[0];
            var indexBuilder = new IndexBuilder(int.Parse(args[1]));

            var stopwatch = Stopwatch.StartNew();
            var index = indexBuilder.UpdateFromPath(dir);
            stopwatch.Stop();
            Console.WriteLine($"Index build in {stopwatch.Elapsed}s");
        }
    }
}