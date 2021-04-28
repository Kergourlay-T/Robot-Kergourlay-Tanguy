using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;


namespace Console_Robot.Serial
{
    public class MsgGenerator
    {
        public MsgGenerator()
        {
            OnMessageGeneratorCreated();
        }

        public void GenerateMessageLEDSetStateConsigneToRobot(ushort numberLED, bool LEDState)
        {
            byte[] payload = new byte[] { (byte)numberLED, (byte)(LEDState ? 0x00 : 0x01) };
            Serial.MsgEncoder.UartEcodeAndSendMessage((ushort)Protocol.Functions.LED_PROTOCOL, payload);
        }

        public void GenerateMessageMotorSetSpeedToRobot(byte leftMotorSpeed, byte rigthMotorSpeed)
        {
            byte[] payload = new byte[2];
            if( 100 >= leftMotorSpeed && leftMotorSpeed <= -100)
            {
                payload[0] = (byte) leftMotorSpeed;
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
                Serial.MsgEncoder.UartEcodeAndSendMessage((ushort)Protocol.Functions.MOTOR_PORTOCOL, payload);
        }

        public void GenerateMessageSetStateRobot(byte StateRobot)
        {
            byte[] payload = new byte[] { (byte)StateRobot };
            Serial.MsgEncoder.UartEcodeAndSendMessage((ushort)Protocol.Functions.SET_ROBOT_STATE, payload);
        }

        #region Events
        public event EventHandler<EventArgs> OnMessageGeneratorCreatedEvent;
        public virtual void OnMessageGeneratorCreated()
        {
            OnMessageGeneratorCreatedEvent?.Invoke(this, new EventArgs());
        }
        #endregion

    }//End MsgGenerator
}//End namespace Console_Robot.Serial 
