using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public class PayloadCommands
    {
        /// <summary>
        /// -1 is for undefine payload
        /// 0 is for order
        /// </summary>
        public static Dictionary<ushort, short> CommandToPayload = new Dictionary<ushort, short>
        {
            {(ushort) Commands.CHECK_INSTRUCTION_ROBOT_TO_GUI, -1 },

            {(ushort) Commands.LED_ROBOT_TO_GUI , 2 },
            {(ushort) Commands.LED_GUI_TO_ROBOT, 2 },

            {(ushort) Commands.TELEMETER_ROBOT_TO_GUI, 5 },

            {(ushort) Commands.MOTOR_CONSIGNE_ROBOT_TO_GUI , 8 },
            {(ushort) Commands.MOTOR_MEASURED_ROBOT_TO_GUI , 8 },
            {(ushort) Commands.MOTOR_ERROR_ROBOT_TO_GUI , 8 },
            {(ushort) Commands.MOTOR_GUI_TO_ROBOT , 2 },

            {(ushort) Commands.ROBOT_STATE_ROBOT_TO_GUI , 1},
            {(ushort) Commands.ROBOT_STATE_GUI_TO_ROBOT , 1 },
            {(ushort) Commands.MANUAL_CONTROL_ROBOT_TO_GUI , 1 },
            {(ushort) Commands.MANUAL_CONTROL_GUI_TO_ROBOT , 1 },

            {(ushort) Commands.POSITION_DATA_ROBOT_TO_GUI , 24 },
            {(ushort) Commands.POSITION_DATA_GUI_TO_ROBOT , 3 },
            {(ushort) Commands.RESET_POSITION_GUI_TO_ROBOT , 1 },

            {(ushort) Commands.MESSAGE_ROBOT_TO_GUI , -1 },
            {(ushort) Commands.MESSAGE_GUI_TO_ROBOT , -1 },

            {(ushort) Commands.SPEED_POLAR_ODOMETRY_ROBOT_TO_GUI , 32 },
            {(ushort) Commands.SPEED_POLAR_CORRECTIONS_ROBOT_TO_GUI , 24 },
            {(ushort) Commands.SPEED_POLAR_GAINS_ROBOT_TO_GUI , 24 },
            {(ushort) Commands.SPEED_POLAR_LIMIT_GAINS_ROBOT_TO_GUI , 24 }
        };
    }
}
