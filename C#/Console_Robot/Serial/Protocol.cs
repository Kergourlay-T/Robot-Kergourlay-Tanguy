using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Robot.Serial
{
    class Protocol
    {
        public const byte SOF = 0xFE;
        public const ushort MAX_MSG_LENGTH = 255;
        public enum Functions : ushort
        {
            SET_LED = 0x0020,
            GET_IR = 0x0030,
            SET_MOTOR = 0x0040,
            GET_STATE = 0x0050,
            SET_STATE = 0x0051,
            GET_POSITION = 0x0061,
            SET_RESET_POSITION = 0x0062,
            SET_POSITION = 0x0063,
            GET_TEXT = 0x0080,
            GET_ASSERV_POLAR_PARAM = 0x0090,
            SET_ASSERV_PARAM = 0x0091
        }


    }
}
