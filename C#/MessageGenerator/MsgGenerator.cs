using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEncoder;
using Constants;
using EventArgsLibrary;
using Utilities;

namespace MessageGenerator
{
    public class MsgGenerator
    {
        public static MsgEncoder msgEncoder;
        public MsgGenerator()
        {
            OnMessageGeneratorCreated();
            MsgEncoder msgEncoder = new MsgEncoder();
        }
        public event EventHandler<EventArgs> OnMessageGeneratorCreatedEvent;
        public virtual void OnMessageGeneratorCreated()
        {
            OnMessageGeneratorCreatedEvent?.Invoke(this, new EventArgs());
        }

        public void GenerateMessageLEDSetStateConsigneToRobot(object sender, LEDMessageArgs e)
        {
            byte[] payload = new byte[] { (byte)e.LEDNumber, (byte)(e.LEDState ? 0x00 : 0x01) };
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.LED_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageMotorSetSpeedToRobot(object sender, MotorMessageArgs e)
        {
            byte[] payload = new byte[Dictionary.PayloadLengthOfFunctions[(ushort)Functions.MOTOR_GUI_TO_ROBOT]];
            if (100 >= e.leftMotor && e.leftMotor <= -100)
            {
                payload[0] = (byte)e.leftMotor;
            }
            else
            {
                payload[0] = (byte)((e.leftMotor > 0) ? 100 : -100);
            }
            if (100 >= e.rightMotor && e.rightMotor <= -100) 
            {
                payload[1] = (byte)e.rightMotor;
            }
            else
            {
                payload[1] = (byte)((e.rightMotor > 0) ? 100 : -100);
            }
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.MOTOR_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageSetStateToRobot(object sender, StateMessageArgs e)
        {
            byte[] payload = new byte[] { (byte)e.state };
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.ROBOT_STATE_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageSetAutoControlStateToRobot (object sender, StateAutoControlMessageArgs e)
        {
            byte[] payload = new byte[] { Convert.ToByte(e.stateAutoControl) };
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.MANUAL_CONTROL_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageSetPositionToRobot (object sender, SetPositionMessageArgs e)
        {
            byte[] payload = new byte[Dictionary.PayloadLengthOfFunctions[(ushort)Functions.POSITION_DATA_GUI_TO_ROBOT]];
            int pos = 0;
            payload[pos++] = Convert.ToByte(e.xPos);
            payload[pos++] = Convert.ToByte(e.yPos);
            payload[pos++] = Convert.ToByte(e.angleRadian);
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.RESET_POSITION_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageResetPositionToRobot(object sender, EventArgs e)
        {
            byte[] payload = new byte[Dictionary.PayloadLengthOfFunctions[(ushort)Functions.RESET_POSITION_GUI_TO_ROBOT]];
            msgEncoder.UartEncodeAndSendMessage((ushort)Functions.RESET_POSITION_GUI_TO_ROBOT, payload);
        }

        public void GenerateMessageTextToRobot(object sender, TextMessageArgs e)
        {
            byte[] payload = Extensions.GetBytes(e.text);
            msgEncoder.UartEncodeAndSendMessage((ushort) payload.Length, payload);
        }

    }//End MsgGenerator
}//End Message Generator
