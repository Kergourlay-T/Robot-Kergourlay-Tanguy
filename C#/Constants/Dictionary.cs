using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    //Have to be converted into dictionary

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
