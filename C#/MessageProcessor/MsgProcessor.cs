﻿using System;
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

        public void MessageProcessor(object sender, MessageByteArgs e )
        {
            switch (e.msgFunction)
            {
                case (ushort)Constants.Enums.Functions.LED_PROTOCOL:
                    OnIRMessageReceived(e);
                    break;

                case (ushort)Constants.Enums.Functions.STATE_PROTOCOL:
                    OnStateMessageReceived(e);
                    break;
                case (ushort)Constants.Enums.Functions.POSITION_DATA:
                    OnPositionMessageReceived(e);
                    break;
                case (ushort)Constants.Enums.Functions.TEXT_PROTOCOL:
                    OnTextMessageReceived(e);
                    break;
                default:
                    OnUnknowFunctionReceived(e);
                    break;
            }
        }

        #region Event

        public event EventHandler<EventArgs> OnMessageProcessorCreatedEvent;
        public event EventHandler<IRMessageArgs> OnIRMessageReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorMessageReceivedEvent;
        public event EventHandler<StateMessageArgs> OnStateMessageReceivedEvent;
        public event EventHandler<PositionMessageArgs> OnPositionMessageReceivedEvent;
        public event EventHandler<TextMessageArgs> OnTextMessageReceivedEvent;
        public event EventHandler<MessageByteArgs> OnUnknowFunctionReceivedEvent;


        public virtual void OnMessageProcessorCreated()
        {
            OnMessageProcessorCreatedEvent?.Invoke(this, new EventArgs());
        }

        public virtual void OnIRMessageReceived(MessageByteArgs e)
        {
            OnIRMessageReceivedEvent?.Invoke(this, new IRMessageArgs(e.msgPayload[0], e.msgPayload[1], e.msgPayload[2]));
        }

        public virtual void OnMotorMessageReceived(MessageByteArgs e)
        {
            OnMotorMessageReceivedEvent?.Invoke(this, new MotorMessageArgs(e.msgPayload[0], e.msgPayload[1]));
        }

        public virtual void OnStateMessageReceived(MessageByteArgs e)
        {
            uint time = ((uint)(e.msgPayload[1] << 24) + (uint)(e.msgPayload[2] << 16) + (uint)(e.msgPayload[3] << 8) + (uint)(e.msgPayload[4] << 0));
            OnStateMessageReceivedEvent?.Invoke(this, new StateMessageArgs ((ushort)e.msgPayload[0], time));
        }

        public virtual void OnPositionMessageReceived(MessageByteArgs e)
        {
            uint time = BitConverter.ToUInt32(e.msgPayload,0);
            float xPos = BitConverter.ToSingle(e.msgPayload, 4);
            float yPos = BitConverter.ToSingle(e.msgPayload, 8); ;
            float angleRadiant = BitConverter.ToSingle(e.msgPayload, 12); ;
            float linearSpeed = BitConverter.ToSingle(e.msgPayload, 16); ;
            float angularSpeed = BitConverter.ToSingle(e.msgPayload, 20); ;
            OnPositionMessageReceivedEvent?.Invoke(this, new PositionMessageArgs (time, xPos, yPos, angleRadiant,linearSpeed, angularSpeed));
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