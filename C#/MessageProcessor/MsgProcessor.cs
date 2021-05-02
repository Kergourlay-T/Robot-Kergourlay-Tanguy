using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;
using Constants;

namespace MessageProcessor
{
    public class MsgProcessor
    {
        public MsgProcessor()
        {
            OnMessageProcessorCreated();
        }

        public void MessageProcessor(object sender, MessageByteArgs e)
        {
            switch (e.msgFunction)
            {
                case (ushort)Enums.Functions.CHECK_INSTRUCTION_ROBOT_TO_GUI:
                    OnCheckInstructionReceived(e);
                    break;

                case (ushort)Enums.Functions.LED_ROBOT_TO_GUI:
                    OnLEDMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.TELEMETER_ROBOT_TO_GUI:
                    OnIRMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.MOTOR_CONSIGNE_ROBOT_TO_GUI:
                    OnMotorConsigneMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.MOTOR_MEASURED_ROBOT_TO_GUI:
                    OnMotorMeasuredMeassgeReceived(e);
                    break;

                case (ushort)Enums.Functions.MOTOR_ERROR_ROBOT_TO_GUI:
                    OnMotorErrorMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.ROBOT_STATE_ROBOT_TO_GUI:
                    OnStateMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.MANUAL_CONTROL_ROBOT_TO_GUI:
                    OnManualControlStateReceived(e);
                    break;

                case (ushort)Enums.Functions.POSITION_DATA_ROBOT_TO_GUI:
                    OnPositionMessageReceived(e);
                    break;

                case (ushort)Enums.Functions.MESSAGE_ROBOT_TO_GUI:
                    OnTextMessageReceived(e);
                    break;

                default:
                    OnUnknowFunctionReceived(e);
                    break;
            }
        }

        #region Event

        public event EventHandler<MessageByteArgs> OnCheckInstructionReceivedEvent;
        public event EventHandler<EventArgs> OnMessageProcessorCreatedEvent;
        public event EventHandler<LEDMessageArgs> OnLEDMessageReceivedEvent;
        public event EventHandler<IRMessageArgs> OnIRMessageReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorConsigneMessageReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorMeasuredMessageReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorErrorMessageReceivedEvent;
        public event EventHandler<StateMessageArgs> OnStateMessageReceivedEvent;
        public event EventHandler<StateAutoControlMessageArgs> OnManualControlStateReceivedEvent;
        public event EventHandler<PositionMessageArgs> OnPositionMessageReceivedEvent;
        public event EventHandler<TextMessageArgs> OnTextMessageReceivedEvent;
        public event EventHandler<MessageByteArgs> OnUnknowFunctionReceivedEvent;

        public virtual void OnCheckInstructionReceived(MessageByteArgs e)
        {
            OnCheckInstructionReceivedEvent?.Invoke(this, new
                MessageByteArgs(e.msgFunction, e.msgPayloadLength, e.msgPayload, e.checksum));
        }

        public virtual void OnMessageProcessorCreated()
        {
            OnMessageProcessorCreatedEvent?.Invoke(this, new EventArgs());
        }

        public virtual void OnLEDMessageReceived(MessageByteArgs e)
        {
            ushort nbLed = Convert.ToUInt16(e.msgPayload[0]);
            bool stateLed = Convert.ToBoolean(e.msgPayload[1]);
            OnLEDMessageReceivedEvent?.Invoke(this, new LEDMessageArgs(nbLed, stateLed));
        }

        public virtual void OnIRMessageReceived(MessageByteArgs e)
        {
            OnIRMessageReceivedEvent?.Invoke(this, new IRMessageArgs(e.msgPayload[0], 
                e.msgPayload[1], e.msgPayload[2], e.msgPayload[3], e.msgPayload[4]));
        }

        public virtual void OnMotorConsigneMessageReceived(MessageByteArgs e)
        {
            OnMotorConsigneMessageReceivedEvent?.Invoke(this, new
                MotorMessageArgs((sbyte)e.msgPayload[0], (sbyte)e.msgPayload[1]));
        }
        
        public virtual void OnMotorMeasuredMeassgeReceived(MessageByteArgs e)
        {
            OnMotorMeasuredMessageReceivedEvent?.Invoke(this, new
                MotorMessageArgs((sbyte)e.msgPayload[0], (sbyte)e.msgPayload[1]));
        }
         
        public virtual void OnMotorErrorMessageReceived(MessageByteArgs e)
        {
            OnMotorErrorMessageReceivedEvent?.Invoke(this, new
                MotorMessageArgs((sbyte)e.msgPayload[0], (sbyte)e.msgPayload[1]));
        }

        public virtual void OnStateMessageReceived(MessageByteArgs e)
        {
            uint time = ((uint)(e.msgPayload[1] << 24) + (uint)(e.msgPayload[2] << 16) 
                + (uint)(e.msgPayload[3] << 8) + (uint)(e.msgPayload[4] << 0));
            OnStateMessageReceivedEvent?.Invoke(this, new StateMessageArgs ((ushort)e.msgPayload[0], time));
        }

        public virtual void OnManualControlStateReceived(MessageByteArgs e)
        {
            OnManualControlStateReceivedEvent?.Invoke(this, new StateAutoControlMessageArgs(Convert.ToBoolean(e.msgPayload[0])));
        }

        public virtual void OnPositionMessageReceived(MessageByteArgs e)
        {
            uint time = BitConverter.ToUInt32(e.msgPayload,0);
            float xPos = BitConverter.ToSingle(e.msgPayload, 4);
            float yPos = BitConverter.ToSingle(e.msgPayload, 8); ;
            float theta = BitConverter.ToSingle(e.msgPayload, 12); ;
            float linear = BitConverter.ToSingle(e.msgPayload, 16); ;
            float angular = BitConverter.ToSingle(e.msgPayload, 20); ;
            OnPositionMessageReceivedEvent?.Invoke(this, new 
                PositionMessageArgs(time, xPos, yPos, theta,linear,angular));
        }

        public virtual void OnTextMessageReceived(MessageByteArgs e)
        {
            string text = Encoding.UTF8.GetString(e.msgPayload, 0, e.msgPayload.Length);
            OnTextMessageReceivedEvent?.Invoke(this, new TextMessageArgs(text));
        }

        public virtual void OnUnknowFunctionReceived(MessageByteArgs e)
        {
            OnUnknowFunctionReceivedEvent?.Invoke(this, e);
        }
        #endregion
    }//End MsgProcessor
}//End MessageProcessir
