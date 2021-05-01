using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceRobot
{
    class EvenTemporaire
    {

        #region Events
        public event EventHandler<EventArgs> OnMessageFromConsoleCreatedEvent;
        public event EventHandler<LEDMessageArgs> OnLEDMessageFromConsoleReceivedEvent;
        public event EventHandler<IRMessageArgs> OnIRMessageFromConsoleReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorMessageFromConsoleReceivedEvent;
        public event EventHandler<StateMessageArgs> OnStateMessageFromConsoleReceivedEvent;
        public event EventHandler<PositionMessageArgs> OnPositionMessageFromConsoleReceivedEvent;
        public event EventHandler<TextMessageArgs> OnTextMessageFromConsoleReceivedEvent;
        public event EventHandler<MessageByteArgs> OnUnknowFunctionFromConsoleReceivedEvent;

        public virtual void OnMessageFromConsoleCreated()
        {
            OnMessageFromConsoleCreatedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnLEDMessageFromConsoleReceived(MessageByteArgs e)
        {
            ushort nbLed = Convert.ToUInt16(e.msgPayload[0]);
            bool stateLed = Convert.ToBoolean(e.msgPayload[1]);
            OnLEDMessageFromConsoleReceivedEvent?.Invoke(this, new LEDMessageArgs(nbLed, stateLed));
        }

        public virtual void OnIRMessageFromConsoleReceived(MessageByteArgs e)
        {
            OnIRMessageFromConsoleReceivedEvent?.Invoke(this, new IRMessageArgs(e.msgPayload[0], e.msgPayload[1], e.msgPayload[2]));
        }

        public virtual void OnMotorMessageFromConsoleReceived(MessageByteArgs e)
        {
            OnMotorMessageFromConsoleReceivedEvent?.Invoke(this, new MotorMessageArgs((sbyte)e.msgPayload[0], (sbyte)e.msgPayload[1]));
        }

        public virtual void OnStateMessageFromConsoleReceived(MessageByteArgs e)
        {
            uint time = ((uint)(e.msgPayload[1] << 24) + (uint)(e.msgPayload[2] << 16) + (uint)(e.msgPayload[3] << 8) + (uint)(e.msgPayload[4] << 0));
            OnStateMessageFromConsoleReceivedEvent?.Invoke(this, new StateMessageArgs((ushort)e.msgPayload[0], time));
        }

        public virtual void OnPositionMessageFromConsoleReceived(MessageByteArgs e)
        {
            uint time = BitConverter.ToUInt32(e.msgPayload, 0);
            float xPos = BitConverter.ToSingle(e.msgPayload, 4);
            float yPos = BitConverter.ToSingle(e.msgPayload, 8); ;
            float angleRadiant = BitConverter.ToSingle(e.msgPayload, 12); ;
            float linearSpeed = BitConverter.ToSingle(e.msgPayload, 16); ;
            float angularSpeed = BitConverter.ToSingle(e.msgPayload, 20); ;
            OnPositionMessageFromConsoleReceivedEvent?.Invoke(this, new PositionMessageArgs(time, xPos, yPos, angleRadiant, linearSpeed, angularSpeed));
        }

        public virtual void OnTextMessageFromConsoleReceived(MessageByteArgs e)
        {
            string text = Encoding.UTF8.GetString(e.msgPayload, 0, e.msgPayload.Length);
            OnTextMessageFromConsoleReceivedEvent?.Invoke(this, new TextMessageArgs(text));
        }

        public virtual void OnUnknowFunctionFromConsoleReceived(MessageByteArgs e)
        {
            OnUnknowFunctionFromConsoleReceivedEvent?.Invoke(this, e);
        }
        #endregion
    }
}
