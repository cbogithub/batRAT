using System;

using batRAT;

namespace batRAT.Commands {
    [Serializable]
    public class TestCommand {
        public string Message { get; set; }
        public MessageTypes MessageType { get; set; }
    }
}
