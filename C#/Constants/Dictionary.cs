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
            {(ushort) Enums.Functions.CHECK_INSTRUCTION_ROBOT_TO_GUI, -1 },// undefine
            {(ushort) Enums.Functions.LED_ROBOT_TO_GUI , 2 },
            {(ushort) Enums.Functions.LED_GUI_TO_ROBOT, 2 },
            {(ushort) Enums.Functions.TELEMETER_ROBOT_TO_GUI, 5 },
            {(ushort) Enums.Functions.MOTOR_CONSIGNE_ROBOT_TO_GUI , 8 },
            {(ushort) Enums.Functions.MOTOR_MEASURED_ROBOT_TO_GUI , 8 },
            {(ushort) Enums.Functions.MOTOR_ERROR_ROBOT_TO_GUI , 8 },
            {(ushort) Enums.Functions.MOTOR_GUI_TO_ROBOT , 2 },
            {(ushort) Enums.Functions.ROBOT_STATE_ROBOT_TO_GUI , 1},
            {(ushort) Enums.Functions.ROBOT_STATE_GUI_TO_ROBOT , 1 },
            {(ushort) Enums.Functions.MANUAL_CONTROL_ROBOT_TO_GUI , 1 },
            {(ushort) Enums.Functions.MANUAL_CONTROL_GUI_TO_ROBOT , 1 },
            {(ushort) Enums.Functions.POSITION_DATA_ROBOT_TO_GUI , 24 },
            {(ushort) Enums.Functions.POSITION_DATA_GUI_TO_ROBOT , 3 },
            {(ushort) Enums.Functions.RESET_POSITION_GUI_TO_ROBOT , 0  }, // 0 because it is an order
            {(ushort) Enums.Functions.MESSAGE_ROBOT_TO_GUI , 0 },
            {(ushort) Enums.Functions.MESSAGE_GUI_TO_ROBOT , 0 },
        };
    }//End Dictionary

}//End Constants

