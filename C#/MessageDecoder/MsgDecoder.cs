﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDecoder
{
    public class MsgDecoder
    {
        public MsgDecoder()
        {
            OnMessageDecoderCreated();
        }

        /***************************************************************************************************/
        //  Declaration of ByteReceived
        /****************************************************************************************************/
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
                    if (b == Protocol.SOF)
                    {
                        OnSOFReceived(b);
                    }
                    else
                    {
                        OnUnknowByteReceived(b);
                    }
                    break;

                case State.FunctionMSB:
                    OnFunctionMSBReceived(b);
                    break;

                case State.FunctionLSB:
                    OnFunctionLSBReceived(b);
                    break;

                case State.PayloadLengthMSB:
                    OnPayloadLenghtMSBReceived(b);
                    break;

                case State.PayloadLengthLSB:
                    OnPayloadLenghtLSBReceived(b);
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
        /***************************************************************************************************/
        //  Declaration of events
        /****************************************************************************************************/

        //Events related to the execution of ByteReceived function
        public event EventHandler<EventArgs> OnMessageDecoderCreatedEvent;
        public event EventHandler<DecodeByteArgs> OnSOFByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnFunctionMSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnFunctionLSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadLenghtMSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadLenghtLSBByteReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnPayloadByteReceivedEvent;
        public event EventHandler<DecodePayloadArgs> OnPayloadReceivedEvent;
        public event EventHandler<DecodeByteArgs> OnChecksumByteReceivedEvent;
        public event EventHandler<MessageByteArgs> OnCorrectChecksumReceivedEvent;
        public event EventHandler<MessageByteArgs> OnWrongChecksumReceivedEvent;

        //Events related to errors that can occur on ByteReceived function
        public event EventHandler<DecodeByteArgs> OnUnknowByteReceivedEvent;
        public event EventHandler<EventArgs> OnUnknowFunctionEvent;
        public event EventHandler<EventArgs> OnOverLenghtMessageEvent;
        public event EventHandler<EventArgs> OnWrongLenghtEvent;

        /***************************************************************************************************/
        //  Declaration of event-related functions
        /****************************************************************************************************/

        #region Execution
        public virtual void OnMessageDecoderCreated()
        {
            OnMessageDecoderCreatedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnSOFReceived(byte e)
        {
            actualState = State.FunctionMSB;
            OnSOFByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
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
            if (Protocol.CheckFunctionLenght(msgFunction) != -2)
            {
                actualState = State.PayloadLengthMSB;
            }
            else
            {
                actualState = State.Waiting;
                OnUnknowFunction();
            }
        }
        public virtual void OnPayloadLenghtMSBReceived(byte e)
        {
            payloadLenghtMSB = e;
            msgPayloadLenght = (ushort)(e << 8);
            actualState = State.PayloadLengthLSB;
            OnPayloadLenghtMSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }
        public virtual void OnPayloadLenghtLSBReceived(byte e)
        {
            payloadLenghtLSB = e;
            msgPayloadLenght += (ushort)(e << 0);
            actualState = State.Waiting;
            OnPayloadLenghtLSBByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
            if (msgPayloadLenght <= Protocol.MAX_MSG_LENGTH)
            {
                short PayloadLengthTest = Protocol.CheckFunctionLenght(msgFunction);
                if (PayloadLengthTest != -1)
                {
                    if (PayloadLengthTest == -1 || PayloadLengthTest == msgPayloadLenght)
                    {
                        actualState = State.CheckSum;
                        msgPayloadIndex = 0;
                        byte[] msgPayload = new byte[msgPayloadLenght];
                    }
                    else
                    {
                        OnWrongLenght();
                    }
                }
            }
            else
            {
                OnOverLenghtMessage();
            }
        }
        public virtual void OnPayloadByteReceived(byte e)
        {
            msgPayload[msgPayloadIndex] = e;
            msgPayloadIndex++;
            if (msgPayloadIndex == msgPayloadLenght)
            {
                OnPayloadReceived(msgPayload);
            }
            OnPayloadByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }
        public virtual void OnPayloadReceived(byte[] e)
        {
            actualState = State.CheckSum;
            OnPayloadReceivedEvent?.Invoke(this, new DecodePayloadArgs(e));
        }
        public virtual void OnChecksumByteReceived(byte e)
        {
            msgChecksum = e;
            if (msgChecksum == CalculateChecksum())
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
            OnCorrectChecksumReceivedEvent?.Invoke(this, new MessageByteArgs(msgFunction, msgPayloadLenght, msgPayload, msgChecksum));
        }
        public virtual void OnWrongChecksumReceived()
        {
            OnWrongChecksumReceivedEvent?.Invoke(this, new MessageByteArgs(msgFunction, msgPayloadLenght, msgPayload, msgChecksum));
        }

        #endregion //Endregion Execution

        #region Errors
        public virtual void OnUnknowByteReceived(byte e)
        {
            OnUnknowByteReceivedEvent?.Invoke(this, new DecodeByteArgs(e));
        }
        public virtual void OnOverLenghtMessage()
        {
            OnOverLenghtMessageEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnWrongLenght()
        {
            OnWrongLenghtEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnUnknowFunction()
        {
            OnUnknowFunctionEvent?.Invoke(this, new EventArgs());
        }
        #endregion //EndRegion Errors

        /***************************************************************************************************/
        //  Declaration of MsgDecoder specific overloads
        /****************************************************************************************************/
        public class DecodeByteArgs : EventArgs
        {
            public DecodeByteArgs(byte b_a)
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

        #endregion //Endregion Event
    }//End MsgDecoder
}//End MessageDecoder