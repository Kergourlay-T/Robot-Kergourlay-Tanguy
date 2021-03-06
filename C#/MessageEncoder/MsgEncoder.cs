﻿using EventArgsLibrary;
using System;

namespace MessageEncoder
{
    /// <summary>
    /// Takes as input the message generated by the interface from MessageGenerator
    /// Then encodes the message and sends it to Serial in order to send it to the robot
    /// </summary>
    public class MsgEncoder
    {
        #region Constructor
        public MsgEncoder()
        {
            OnMessageEncoderCreated();
        }
        public event EventHandler<EventArgs> OnMessageEncoderCreatedEvent;
        public virtual void OnMessageEncoderCreated() => OnMessageEncoderCreatedEvent?.Invoke(this, new EventArgs());

        #endregion

        #region Main Method
        public void EncodeMessageToRobot(object sender, MessageToRobotArgs e)
        {
            byte[] message = new byte[e.MsgPayloadLength + 6];
            int pos = 0;
            message[pos++] = 0xFE;
            message[pos++] = (byte)(e.MsgFunction >> 8);
            message[pos++] = (byte)(e.MsgFunction >> 0);
            message[pos++] = (byte)(e.MsgPayloadLength >> 8);
            message[pos++] = (byte)(e.MsgPayloadLength >> 0);
            for (int i = 0; i < e.MsgPayloadLength; i++)
            {
                message[pos++] = e.MsgPayload[i];
            }
            message[pos++] = CalculateChecksum(e.MsgFunction, e.MsgPayloadLength, e.MsgPayload);
            OnMessageEncoded(message);
        }
        #endregion //Main Method

        #region Sub Method
        byte CalculateChecksum(int msgFunction,
                int msgPayloadLength, byte[] msgPayload)
        {
            byte checksum = 0;
            checksum ^= (byte)(msgFunction >> 8);
            checksum ^= (byte)(msgFunction >> 0);
            checksum ^= (byte)(msgPayloadLength >> 8);
            checksum ^= (byte)(msgPayloadLength >> 0);
            for (int i = 0; i < msgPayloadLength; i++)
            {
                checksum ^= msgPayload[i];
            }
            return checksum;
        }
        #endregion //Sub Method

        #region Output Event
        public event EventHandler<MessageEncodedArgs> OnMessageEncodedEvent;
        public virtual void OnMessageEncoded(byte[] msg) => OnMessageEncodedEvent?.Invoke(this, new MessageEncodedArgs { Msg = msg });
        #endregion //Output Event
    }
}
