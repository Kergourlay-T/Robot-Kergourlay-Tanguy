using System;
using EventArgsLibrary;
using Utilities;
using Protocol;

namespace MessageGenerator
{
    public class MsgGenerator
    {
        #region Constructor
        public MsgGenerator()
        {
            OnMessageGeneratorCreated();
        }
        public event EventHandler<EventArgs> OnMessageGeneratorCreatedEvent;
        public virtual void OnMessageGeneratorCreated() => OnMessageGeneratorCreatedEvent?.Invoke(this, new EventArgs());

        #endregion

        #region Input Event
        public void GenerateMessageLEDSetStateConsigneToRobot(object sender, LEDMessageArgs e)
        {
            byte[] payload = new byte[] { (byte)e.nbLed_a, (byte)(e.stateLed_a ? 0x00 : 0x01) };
            OnMessageToRobot((ushort)Commands.LED_GUI_TO_ROBOT, 2, payload);
        }

        public void GenerateMessageMotorSetSpeedToRobot(object sender, MotorMessageArgs e)
        {
            byte[] payload = new byte[2];
            if (100 >= e.leftMotor_a && e.leftMotor_a <= -100)
                payload[0] = (byte)e.leftMotor_a;
            else
                payload[0] = (byte)((e.leftMotor_a > 0) ? 100 : -100);

            if (100 >= e.rightMotor_a && e.rightMotor_a <= -100)
                payload[1] = (byte)e.rightMotor_a;
            else
                payload[1] = (byte)((e.rightMotor_a > 0) ? 100 : -100);

            OnMessageToRobot((ushort)Commands.MOTOR_GUI_TO_ROBOT, 2, payload);
        }

        public void GenerateMessageSetStateToRobot(object sender, UshortEventArgs e)
        {
            byte[] payload = new byte[] { (byte)e.Value };
            OnMessageToRobot((ushort)Commands.ROBOT_STATE_GUI_TO_ROBOT, 1, payload);
        }

        public void GenerateMessageSetAutoControlStateToRobot(object sender, BoolEventArgs e)
        {
            byte[] payload = new byte[] { Convert.ToByte(e.Value) };
            OnMessageToRobot((ushort)Commands.MANUAL_CONTROL_GUI_TO_ROBOT, 1, payload);
        }

        public void GenerateMessageSetPositionToRobot(object sender, SetPositionMessageArgs e)
        {
            byte[] payload = new byte[3];
            int pos = 0;
            payload[pos++] = Convert.ToByte(e.xPos_a);
            payload[pos++] = Convert.ToByte(e.yPos_a);
            payload[pos++] = Convert.ToByte(e.angleRadian_a);
            OnMessageToRobot((ushort)Commands.RESET_POSITION_GUI_TO_ROBOT, 3, payload);
        }

        public void GenerateMessageResetPositionToRobot(object sender, BoolEventArgs e)
        {
            byte[] payload = new byte[] { Convert.ToByte(e.Value) };
            OnMessageToRobot((ushort)Commands.RESET_POSITION_GUI_TO_ROBOT, 1, payload);
        }

        public void GenerateMessageTextToRobot(object sender, TextMessageArgs e)
        {
            byte[] payload = Extensions.GetBytes(e.text_a);
            OnMessageToRobot((ushort)Commands.MESSAGE_GUI_TO_ROBOT, (ushort)payload.Length, payload);
        }
        #endregion //Input Event

        #region Ouput Event
        public event EventHandler<MessageToRobotArgs> OnMessageToRobotGeneratedEvent;
        public virtual void OnMessageToRobot(ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload)
            => OnMessageToRobotGeneratedEvent?.Invoke(this, new MessageToRobotArgs
            {
                MsgFunction = msgFunction,
                MsgPayloadLength = msgPayloadLength,
                MsgPayload = msgPayload
            });
        #endregion
    }
}
