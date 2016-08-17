using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT.Commands;

namespace Client.Core {
    public class bClient {

        public class StateObject {
            public Socket socket = null;
            public const int bufferSize = 1024;
            public byte[] buffer = new byte[bufferSize];
        }

        Socket clientSocket;
        IPEndPoint ipEndPoint;

        public bClient(IPEndPoint _ipEndPoint) {
            ipEndPoint = _ipEndPoint;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.BeginConnect("127.0.0.1", 5454, OnConnect, null);
        }

        void OnConnect(IAsyncResult ar) {
            Console.WriteLine("Connected to server!");
            clientSocket.EndConnect(ar);
        }

        public void StartReceive() {
            StateObject stateObj = new StateObject();
            stateObj.socket = clientSocket;
            clientSocket.BeginReceive(stateObj.buffer, 0, stateObj.buffer.Length, 0, (ar) => {
                StateObject state = (StateObject)ar.AsyncState;
                int bytesReceived = state.socket.EndReceive(ar);

                string msg = ExtractMessage(state, bytesReceived);
                Console.WriteLine(msg);

                if(bytesReceived != 0) {
                    StartReceive();
                }

            }, stateObj);
        }

        string ExtractMessage(StateObject state, int bytesReceived) {
            byte[] buf = new byte[bytesReceived];
            Array.Copy(state.buffer, 0, buf, 0, bytesReceived);
            using(var ms = new MemoryStream(buf)) {
                TestCommand o = (TestCommand)new BinaryFormatter().Deserialize(ms);
                ms.Close();
                return o.Message;
            }
        }

    }
}
