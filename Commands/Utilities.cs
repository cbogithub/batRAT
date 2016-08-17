using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT.Commands;

namespace batRAT {
    public class Utilities {
        public static string ExtractMessage(StateObject state, int bytesReceived) {
            byte[] buf = new byte[bytesReceived];
            Array.Copy(state.buffer, 0, buf, 0, bytesReceived);
            using(var ms = new MemoryStream(buf)) {
                TestCommand o = (TestCommand)new BinaryFormatter().Deserialize(ms);
                ms.Close();
                return o.Message;
            }
        }

        public static MessageTypes ExtractMessageType(StateObject state, int bytesReceived) {
            byte[] buf = new byte[bytesReceived];
            Array.Copy(state.buffer, 0, buf, 0, bytesReceived);
            using(var ms = new MemoryStream(buf)) {
                TestCommand o = (TestCommand)new BinaryFormatter().Deserialize(ms);
                ms.Close();
                return o.MessageType;
            }
        }
    }
}
