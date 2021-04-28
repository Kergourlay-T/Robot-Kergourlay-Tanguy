using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Dictionary
    {
        public Dictionary()
        {

        }

        public static Dictionary<ushort, short> CheckPayloadLengthAssoicatedToFunction = new Dictionary<ushort, short>()
        {
            {Enums.Functions.LED_PROTOCOL, 2 },
            {Enums.Functions.TELEMETER_PROTOCOL,3},
            {Enums.Functions.MOTOR_PROTOCOL, 2 },
            {Enums.Functions.MESSAGE_PROTOCOL, -1 },
            {Enums.Functions.STATE_PROTOCOL, 1 },
            {Enums.Functions.SET_ROBOT_STATE,1 },
            {Enums.Functions.SET_ROBOT_MANUAL_CONTROL,1 },
            {Enums.Functions.POSITION_DATA, 24 }
        };
    }//End Dictionary

    //public static short CheckFunctionLenght(ushort msgFunction)
    //{
    //    switch (msgFunction)
    //    {
    //        // -2               : UNKNOW
    //        // -1               : UNLIMITED 
    //        // [0:MAX_LENGHT]   : FIXED
    //        case (ushort)Functions.LED_PROTOCOL:
    //            return 2;
    //        case (ushort)Functions.TELEMETER_PROTOCOL:
    //            return 3;
    //        case (ushort)Functions.MOTOR_PORTOCOL:
    //            return 2;
    //        case (ushort)Functions.MESSAGE_PROTOCOL:
    //            return -1;
    //        case (ushort)Functions.STATE_PROTOCOL:
    //            return 5;
    //        case (ushort)Functions.SET_ROBOT_STATE:
    //            return 1;
    //        case (ushort)Functions.SET_ROBOT_MANUAL_CONTROL:
    //            return 1;
    //        case (ushort)Functions.POSITION_DATA:
    //            return 24;
    //        default:
    //            return -2;
    //    }
    //}

}//End Constants

