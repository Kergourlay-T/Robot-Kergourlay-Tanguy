using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public static class ConstsProtocol
    {
        public const byte START_OF_FRAME = 0xFE;
        public const ushort MAX_MSG_LENGTH = 255;
    }
}
