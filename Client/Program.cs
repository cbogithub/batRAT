using System;
using System.Net;

using Client.Core;

namespace Client {
    static class Program {
        static void Main() {
            Console.ReadKey();
            bClient client = new bClient(new IPEndPoint(IPAddress.Any, 5454));
            Console.ReadKey();
        }
    }
}
