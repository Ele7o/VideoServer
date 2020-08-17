using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tao http server tai port 8080");
            HttpServer server = new HttpServer(8080);
            server.Start();
        }
    }
}
