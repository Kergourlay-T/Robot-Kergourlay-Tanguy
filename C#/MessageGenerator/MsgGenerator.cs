using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEncoder;
using Constants;

namespace MessageGenerator
{
    public class MsgGenerator
    {
        public MsgGenerator()
        {
            OnMessageGeneratorCreated();
        }
        public event EventHandler<EventArgs> OnMessageGeneratorCreatedEvent;
        public virtual void OnMessageGeneratorCreated()
        {
            OnMessageGeneratorCreatedEvent?.Invoke(this, new EventArgs());
        }


        public void GenerateMessageLEDSetStateConsigneToRobot(ushort numberLED, bool LEDState)
        {
            byte[] payload = new byte[] { (byte)numberLED, (byte)(LEDState ? 0x00 : 0x01) };
            MsgEncoder.UartEncodeAndSendMessage((ushort)Enums.Functions.LED_PROTOCOL, payload);
        }

        public void GenerateMessageMotorSetSpeedToRobot(byte leftMotorSpeed, byte rigthMotorSpeed)
        {
            byte[] payload = new byte[2];
            if (100 >= leftMotorSpeed && leftMotorSpeed <= -100)
            {
                payload[0] = (byte)leftMotorSpeed;
            }
            else
            {
                payload[0] = (byte)((leftMotorSpeed > 0) ? 100 : -100);
            }
            if (100 >= rigthMotorSpeed && rigthMotorSpeed <= -100)
            {
                payload[1] = (byte)rigthMotorSpeed;
            }
            else
            {
                payload[1] = (byte)((rigthMotorSpeed > 0) ? 100 : -100);
            }
            MsgEncoder.UartEncodeAndSendMessage((ushort)Enums.Functions.MOTOR_PORTOCOL, payload);
        }

        public void GenerateMessageSetStateRobot(byte StateRobot)
        {
            byte[] payload = new byte[] { (byte)StateRobot };
            MsgEncoder.UartEncodeAndSendMessage((ushort)Enums.Functions.SET_ROBOT_STATE, payload);
        }

    }//End MsgGenerator
}//End Message Generator
