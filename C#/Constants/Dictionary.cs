using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Dictionary
    {
        public static Dictionary<ushort, short> PayloadLengthOfFunctions = new Dictionary<ushort, short>
        {
            {(ushort) Functions.CHECK_INSTRUCTION_ROBOT_TO_GUI, -1 },// undefine
            {(ushort) Functions.LED_ROBOT_TO_GUI , 2 },
            {(ushort) Functions.LED_GUI_TO_ROBOT, 2 },
            {(ushort) Functions.TELEMETER_ROBOT_TO_GUI, 5 },
            {(ushort) Functions.MOTOR_CONSIGNE_ROBOT_TO_GUI , 8 },
            {(ushort) Functions.MOTOR_MEASURED_ROBOT_TO_GUI , 8 },
            {(ushort) Functions.MOTOR_ERROR_ROBOT_TO_GUI , 8 },
            {(ushort) Functions.MOTOR_GUI_TO_ROBOT , 2 },
            {(ushort) Functions.ROBOT_STATE_ROBOT_TO_GUI , 1},
            {(ushort) Functions.ROBOT_STATE_GUI_TO_ROBOT , 1 },
            {(ushort) Functions.MANUAL_CONTROL_ROBOT_TO_GUI , 1 },
            {(ushort) Functions.MANUAL_CONTROL_GUI_TO_ROBOT , 1 },
            {(ushort) Functions.POSITION_DATA_ROBOT_TO_GUI , 24 },
            {(ushort) Functions.POSITION_DATA_GUI_TO_ROBOT , 3 },
            {(ushort) Functions.RESET_POSITION_GUI_TO_ROBOT , 0  }, // 0 because it is an order
            {(ushort) Functions.MESSAGE_ROBOT_TO_GUI , 0 },
            {(ushort) Functions.MESSAGE_GUI_TO_ROBOT , 0 },
        };
    }//End Dictionary

}//End Constants

