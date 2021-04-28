﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;
using ExtendedSerialPort;

namespace Console_Robot.Serial
{
    class MsgEncoder
    {
        public MsgEncoder()
        {
            OnMessageEncoderCreated();
        }

        public bool UartEncodeAndSendMessage (ushort msgFunction, byte[] msgPayload)
        {
            short msgPayloadLengthTest = Protocol.CheckFunctionLenght(msgFunction);
            ushort msgPayloadLength = (ushort) msgPayload.Length;
            if(msgPayloadLengthTest != -2)
            {
                if(msgPayloadLengthTest != -1)
                {
                    msgPayloadLength = (ushort) msgPayloadLengthTest;
                }
            }

            if(msgPayloadLength == msgPayload.Length)
            {
                byte[] msg = EncodeWithoutChecksum(msgFunction, msgPayloadLength, msgPayload);
                byte checksum = CalculateChecksum(msgFunction, msgPayloadLength, msgPayload);
                msg[msg.Length - 1] = checksum;

                if(Serial.serialPort!= null)
                {
                    Serial.serialPort.WWrite(msg, 0, msg.Length);
                    OnSendMessage(msgFunction, msgPayloadLength, msgPayload, checksum);
                    return true;
                }
                else
                {
                    OnSerialDeconnected();
                }
            }
            else
            {
                OnWrongPayloadSend();
            }
            if(msgPayloadLengthTest == -2)
            {
                OnUnknowFunctionSent();
            }            
            return false;
        }

        private static byte[] EncodeWithoutChecksum (ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload)
        {
            //Convert msgFunction into byte
            byte functionMSB = (byte)(msgFunction >> 0);
            byte functionLSB = (byte)(msgFunction >> 8);

            //Convert msgPayloadLength into byte
            byte payloadLenghtMSB = (byte)(msgPayloadLength >> 0);
            byte payloadLenghtLSB = (byte)(msgPayloadLength >> 8);

            byte[] msg = new byte[6 + msgPayload.Length];
            ushort i;
            msg[0] = Protocol.SOF;
            msg[1] = functionMSB;
            msg[2] = functionMSB;
            msg[3] = payloadLenghtMSB;
            msg[4] = payloadLenghtLSB;
            for (i = 0; i < msgPayload.Length; i++)
            {
                msg[5 + i] = msgPayload[i];
            }
            return msg;
        }

        private static byte CalculateChecksum (ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload)
        {
            byte[] msg = EncodeWithoutChecksum(msgFunction, msgPayloadLength, msgPayload);

            byte checksum = msg[0];
            for (int i=1; i < msgPayload.Length; i++)
            {
                checksum ^= msgPayload[i];
            }
            return checksum;
        }

        #region Event
        public event EventHandler<EventArgs> OnMessageEncoderCreatedEvent;
        public event EventHandler<MessageByteArgs> OnSendMessageEvent;
        public event EventHandler<LEDMessageArgs> OnSetLEDStateEvent;
        public event EventHandler<MotorMessageArgs> OnMotorSetSpeedEvent;
        public event EventHandler<SetStateArgs> OnSetStateEvent;


        public event EventHandler<EventArgs> OnSerialDeconnectedEvent;
        public event EventHandler<EventArgs> OnWrongPayloadSendEvent;
        public event EventHandler<EventArgs> OnUnknowFunctionSentEvent;

        //Functions associated with events
        public virtual void OnMessageEncoderCreated()
        {
            OnMessageEncoderCreatedEvent?.Invoke(this, new EventArgs());
        }

        public virtual void OnSendMessage(ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload, byte checksum)
        {
            OnSendMessageEvent?.Invoke(this, new MessageByteArgs());
            switch (msgFunction)
            {
                case (ushort)Protocol.Functions.LED_PROTOCOL:
                    OnSetLEDState(msgPayload[0], msgPayload[1] == 0x00 ? false : true);
                    break;
                case (ushort)Protocol.Functions.MOTOR_PORTOCOL:
                    OnMotorSetSpeed(msgPayload[0], msgPayload[1]);
                    break;
                case (ushort)Protocol.Functions.SET_ROBOT_STATE:
                    OnSetState(msgPayload[0]);
                    break;  
            }
        }

        public virtual void OnSetLEDState(ushort LEDNumber, bool LEDState)
        {
            OnSetLEDStateEvent?.Invoke(this, new LEDMessageArgs(LEDNumber, LEDState));
        }
        public virtual void OnMotorSetSpeed(byte leftMotorSpeed, byte rigthMotorSpeed)
        {
            OnMotorSetSpeedEvent?.Invoke(this, new MotorMessageArgs(leftMotorSpeed, rigthMotorSpeed));
        }
        public virtual void OnSetState(byte State)
        {
            OnMotorSetSpeedEvent?.Invoke(this, new SetStateArgs(State));
        }

        public virtual void OnSerialDeconnected()
        {
            OnSerialDeconnectedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnWrongPayloadSend()
        {
            OnWrongPayloadSendEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnUnknowFunctionSent()
        {
            OnUnknowFunctionSentEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}