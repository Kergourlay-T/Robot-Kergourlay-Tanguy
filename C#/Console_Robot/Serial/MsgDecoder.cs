using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Robot.Serial
{
    class MsgDecoder
    {
        public MsgDecoder()
        {
            OnMessageDecoderCreated();
        }

        private enum State
        {
            Waiting,
            FunctionMSB,
            FunctionLSB,
            PayloadLengthMSB,
            PayloadLengthLSB,
            Payload,
            CheckSum
        }
        static State actualState = State.Waiting;

        private static byte functionMSB;
        private static byte functionLSB;
        private static byte payloadLenghtMSB;
        private static byte payloadLenghtLSB;

        private static ushort msgFunction;
        private static ushort msgPayloadLenght;
        private static byte[] msgPayload;
        private static byte msgChecksum;

        private static int msgPayloadIndex = 0;
        private static byte CalculateChecksum()
        {
            byte checksum = Protocol.SOF;
            checksum ^= functionMSB;
            checksum ^= functionLSB;
            checksum ^= payloadLenghtMSB;
            checksum ^= payloadLenghtLSB;
            foreach (byte b in msgPayload)
            {
                checksum ^= b;
            }
            return checksum;
        }

        public void ByteReceived(byte b)
        {
            switch (actualState)
            {
                case State.Waiting:
                    if(b == Protocol.SOF)
                    {
                        OnSOFReceived(b);
                    }
                    else
                    {
                        
                    }
                    break;

                case State.FunctionMSB:
                    OnFunctionMSBReceived(b);
                    break;

                case State.FunctionLSB:
                    OnFunctionLSBReceived(b);
                    break;

                case State.PayloadLengthMSB:
                    OnFunctionMSBReceived(b);
                    break;

                case State.PayloadLengthLSB:
                    OnFunctionLSBReceived(b);
                    break;

                case State.Payload:
                    OnPayloadByteReceived(b);
                    break;

                case State.CheckSum:
                    OnChecksumByteReceived(b);
                    break;

                default:
                    actualState = State.Waiting;
                    break;
            }
        }

        #region Event
        public event EventHandler<EventArgs> OnMessageDecoderCreatedEvent;
        public event EventHandler<DecodeByteArgs> OnSOFByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnUnknowByteEvent;
        public event EventHandler<DecodeByteArgs> OnFunctionMSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnFunctionLSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadLenghtMSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadLenghtLSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadByteReceivedEvent;
        public event EventHandler<DecodePayloadArgs> OnPayloadReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnChecksumByteReceivedEvent;
        public event EventHandler<> OnCorrectChecksumEvent;
        public event EventHandler<> OnWrongChecksumEvent;
        public event EventHandler<EventArgs> OnOverLenghtMessageEvent;
        public event EventHandler<EventArgs> OnUnknowFunctionEvent;
        public event EventHandler<EventArgs> OnWrongLenghtFunctionEvent;



        public virtual void OnMessageDecoderCreated()
        {
            OnMessageDecoderCreatedEvent?.Invoke(this, new EventArgs());
        }

        public virtual void OnSOFReceived(byte e)
        {
            actualState = State.FunctionMSB;
            OnSOFByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }

        public virtual void OnUnknowReceived(byte e)
        {
            OnUnknowByteEvent?.Invoke(this, new DecodeByteArgs(e));
        }

        public virtual void OnFunctionMSBReceived(byte e)
        {
            functionMSB = e;
            msgFunction = (ushort)(e << 8);
            actualState = State.FunctionLSB;
            OnFunctionMSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }

        public virtual void OnFunctionLSBReceived(byte e)
        {
            functionLSB = e;
            msgFunction += (ushort)(e << 0);
            OnFunctionLSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));


        }

        public virtual void OnPayloadLenghtMSBReceided(byte e)
        {
            payloadLenghtMSB = e;
            msgPayloadLenght = (ushort)(e << 8);
            actualState = State.PayloadLengthLSB;
            OnPayloadLenghtMSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }

        public virtual void OnPayloadLenghtLSBReceided(byte e)
        {
            payloadLenghtLSB = e;
            msgPayloadLenght += (ushort)(e << 0);
            actualState = State.Waiting;
            OnPayloadLenghtLSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));        


        }

        public virtual void OnPayloadByteReceived (byte e)
        {
            msgPayload[msgPayloadIndex] = e;
            msgPayloadIndex++;
            if(msgPayloadIndex == msgPayloadLenght)
            {
                OnPayloadReceived(msgPayload);
            }
            OnPayloadByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }

        public virtual void OnPayloadReceived (byte[] e)
        {
            actualState = State.CheckSum;
            OnPayloadReceivedEvent?.Invoke(this, new DecodePayloadArgs(e));
        }


        public virtual void OnChecksumByteReceived(byte e)
        {
            msgChecksum = e;
            if(msgChecksum == CalculateChecksum())
            {
                OnCorrectChecksumReceived();
            }
            else
            {
                OnWrongChecksumReceived();
            }
            actualState = State.Waiting;
            OnChecksumByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }


        public virtual void OnCorrectChecksumReceived()
        {

        }

        public virtual void OnWrongChecksumReceived()
        {

        }

        public class DecodeByteArgs : EventArgs
        {
            public DecodeByteArgs (byte b_a)
            {
                b = b_a;
            }
            public byte b { get; set; }
        }

        public class DecodePayloadArgs : EventArgs
        {
            public DecodePayloadArgs(byte[] payload_a)
            {
                payload = payload_a;
            }
            public byte[] payload { get; set; }
        }

        #endregion


    }//End Class MsgDecoder
}//End namespace Console_Robot.Serial
