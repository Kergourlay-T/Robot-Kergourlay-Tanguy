using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Enums
    {
        public enum Functions : ushort
        {
            LED_PROTOCOL = 0x0020,
            TELEMETER_PROTOCOL = 0x0030,
            MOTOR_PORTOCOL = 0x0040,
            STATE_PROTOCOL = 0x0050,
            SET_ROBOT_STATE = 0x0051,
            SET_ROBOT_MANUAL_CONTROL = 0x0052,
            POSITION_DATA = 0x0061,
            TEXT_PROTOCOL = 0x0080,
        }
    }

}
