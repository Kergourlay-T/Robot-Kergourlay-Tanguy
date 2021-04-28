using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessor
{
    public class MsgProcessor
    {
        public MsgProcessor()
        {
            OnMessageProcessorCreated();
        }

        public void MessageProcessor(object sender, e)
        {
            switch (e.)
            {
                case (ushort)Protocol.Functions.GET_IR:
                    OnIRMessageReceived(e);
                    break;

                case (ushort)Protocol.Functions.GET_STATE:
                    OnStateMessageReceived(e);
                    break;
                case (ushort)Protocol.Functions.GET_POSITION:
                    OnPositionMessageReceived(e);
                    break;
                case (ushort)Protocol.Functions.GET_TEXT:
                    OnTextMessageReceived(e);
                    break;
                default:
                    OnUnknowFunctionReceived(e);
                    break;
            }
        }

        #region Event

        public event EventHandler<EventArgs> OnMessageProcessorCreatedEvent;
        public event EventHandler<> OnIRMessageReceivedEvent;
        public event EventHandler<> OnMotorMessageReceivedEvent;
        public event EventHandler<> OnStateMessageReceivedEvent;
        public event EventHandler<> OnPositionMessageReceivedEvent;
        public event EventHandler<> OnTextMessageReceivedEvent;
        public event EventHandler<> OnUnknowFunctionReceivedEvent;


        public virtual void OnMessageProcessorCreated()
        {
            OnMessageProcessorCreatedEvent?.Invoke(this, new EventArgs());
        }

        public virtual void OnIRMessageReceived()
        {
            OnIRMessageReceivedEvent?.Invoke(this, new );
        }

        public virtual void OnMotorMessageReceived()
        {
            OnMotorMessageReceivedEvent?.Invoke(this, new );
        }

        public virtual void OnStateMessageReceived()
        {
            OnStateMessageReceivedEvent?.Invoke(this, new);
        }

        public virtual void OnPositionMessageReceived()
        {
            OnPositionMessageReceivedEvent?.Invoke(this, new);
        }

        public virtual void OnTextMessageReceived()
        {
            OnTextMessageReceivedEvent?.Invoke(this, new );
        }

        public virtual void OnUnknowFunctionReceived()
        {
            OnUnknowFunctionReceivedEvent?.Invoke(this, new);
        }
        #endregion
    }//End MsgProcessor
}//End MessageProcessir
