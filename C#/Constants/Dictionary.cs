using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Dictionary
    {
        public static Dictionary<ushort, short> CheckPayloadLengthAssoicatedToFunction = new Dictionary<ushort, short>
        {
            {(ushort) Enums.Functions.LED_PROTOCOL , 2 },
            {(ushort) Enums.Functions.TELEMETER_PROTOCOL,3},
            {(ushort) Enums.Functions.MOTOR_PORTOCOL, 2 },
            {(ushort) Enums.Functions.TEXT_PROTOCOL, -1 },
            {(ushort) Enums.Functions.STATE_PROTOCOL, 5 },
            {(ushort) Enums.Functions.SET_ROBOT_STATE,1 },
            {(ushort) Enums.Functions.SET_ROBOT_MANUAL_CONTROL,1 },
            {(ushort) Enums.Functions.POSITION_DATA, 24 },
        };
    }//End Dictionary

}//End Constants

