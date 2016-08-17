using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT;
using batRAT.Commands;

namespace Client.Core {
    public class bClient {

        Socket clientSocket;
        Socket serverSocket;
        IPEndPoint ipEndPoint;

        public bClient(IPEndPoint _ipEndPoint) {
            ipEndPoint = _ipEndPoint;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.BeginConnect("127.0.0.1", 5454, OnConnect, clientSocket);
        }

        void OnConnect(IAsyncResult ar) {
            clientSocket = (Socket)ar.AsyncState;
            clientSocket.EndConnect(ar);
            Console.WriteLine("Connected to " + clientSocket.RemoteEndPoint.ToString());
        }

        public void StartReceive() {
            StateObject stateObj = new StateObject();
            stateObj.socket = clientSocket;
            clientSocket.BeginReceive(stateObj.buffer, 0, stateObj.buffer.Length, 0, (ar) => {
                StateObject state = (StateObject)ar.AsyncState;
                int bytesReceived = state.socket.EndReceive(ar);

                string msg = Utilities.ExtractMessage(state, bytesReceived);
                Console.WriteLine(msg);

                MessageTypes type = Utilities.ExtractMessageType(state, bytesReceived);
                TestCommand retCommand = new TestCommand();
                switch(type) {
                    case MessageTypes.MESSAGE:
                        retCommand.Message = "I got your message!";
                        break;
                    case MessageTypes.TEST:
                        retCommand.Message = "I got your test!";
                        break;
                }

                Send<TestCommand>(retCommand, clientSocket);

                if(bytesReceived != 0) {
                    StartReceive();
                }

            }, stateObj);
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
            Socket handler = (Socket)ar.AsyncState;
            int sentBytes = handler.EndSend(ar);
            if(sentBytes <= 0) {
                Console.WriteLine("Ret value can not sent!");
                return;
            }
        }
    }
}
