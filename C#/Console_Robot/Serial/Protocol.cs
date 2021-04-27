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
            LED_PROTOCOL = 0x0020,
            TELEMETER_PROTOCOL= 0x0030,
            MOTOR_PORTOCOL = 0x0040,
            STATE_PROTOCOL = 0x0050,
            SET_ROBOT_STATE = 0x0051,
            SET_ROBOT_MANUAL_CONTROL = 0x0052,
            POSITION_DATA = 0x0061,
            MESSAGE_PROTOCOL = 0x0080,
        }

        public static short CheckFunctionLenght(ushort msgFunction)
        {
            switch (msgFunction)
            {
                // -2               : UNKNOW
                // -1               : UNLIMITED 
                // [0:MAX_LENGHT]   : FIXED
                case (ushort)Functions.LED_PROTOCOL:
                    return 2;
                case (ushort)Functions.TELEMETER_PROTOCOL:
                    return 3;
                case (ushort)Functions.MOTOR_PORTOCOL:
                    return 2;
                case (ushort)Functions.MESSAGE_PROTOCOL:
                    return -1;
                case (ushort)Functions.STATE_PROTOCOL:
                    return 5;
                case (ushort)Functions.SET_ROBOT_STATE:
                    return 1;
                case (ushort)Functions.SET_ROBOT_MANUAL_CONTROL:
                    return 1;
                case (ushort)Functions.POSITION_DATA:
                    return 24;
                default:
                    return -2;
            }
        }

    }
}
