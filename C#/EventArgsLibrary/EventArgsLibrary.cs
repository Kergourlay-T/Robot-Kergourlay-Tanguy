﻿using System;

namespace EventArgsLibrary
{
    public class MessageByteArgs : EventArgs
    {
        public MessageByteArgs(ushort msgFunction_a, ushort msgPayloadLength_a, 
            byte[] msgPayload_a, byte checksum_a)
        {
            msgFunction = msgFunction_a;
            msgPayloadLength = msgPayloadLength_a;
            msgPayload = msgPayload_a;
            checksum = checksum_a;
            ConvertUshortToByte();
        }
        private void ConvertUshortToByte()
        {
            functionMSB = (byte)(msgFunction >> 0);
            functionLSB = (byte)(msgFunction >> 8);

            payloadMSB = (byte)(msgPayloadLength >> 0);
            payloadLSB = (byte)(msgPayloadLength >> 8);
        }
        public MessageByteArgs(byte SOF_a, byte functionMSB_a, byte functionLSB_a, 
            byte payloadMSB_a, byte payloadLSB_a, byte[] msgPayload_a, byte checksum_a)
        {
            SOF = SOF_a;
            functionMSB = functionMSB_a;
            functionLSB = functionLSB_a;
            payloadMSB = payloadMSB_a;
            payloadLSB = payloadLSB_a;
            msgPayload = msgPayload_a;
            checksum = checksum_a;
            ConvertByteToUshort();
        }
        private void ConvertByteToUshort()
        {
            msgFunction = (ushort)(functionMSB << 8 + functionLSB << 0);
            msgPayloadLength = (ushort)(payloadMSB << 8 + payloadLSB << 0);
        }
        public byte SOF { get; set; }
        public byte functionMSB { get; set; }
        public byte functionLSB { get; set; }
        public byte payloadMSB { get; set; }
        public byte payloadLSB { get; set; }
        public ushort msgFunction { get; set; }
        public ushort msgPayloadLength { get; set; }
        public byte[] msgPayload { get; set; }
        public byte checksum { get; set; }
    }

    public class LEDMessageArgs : EventArgs
    {
        public LEDMessageArgs(ushort LEDNumber_a, bool LEDState_a)
        {
            LEDNumber = LEDNumber_a;
            LEDState = LEDState_a;
        }
        public ushort LEDNumber { get; set; }
        public bool LEDState { get; set; }
    }

    public class MotorMessageArgs : EventArgs
    {
        public MotorMessageArgs(sbyte leftMotor_a, sbyte rightMotor_a)
        {
            leftMotor = leftMotor_a;
            rightMotor = rightMotor_a;
        }
        public sbyte leftMotor { get; set; }
        public sbyte rightMotor { get; set; }
    }

    public class StateMessageArgs : EventArgs
    {
        public StateMessageArgs(ushort state_a)
        {
            state = state_a;
        }
        public StateMessageArgs(ushort state_a, uint time_a)
        {
            state = state_a;
            time = time_a;
        }
        public ushort state { get; set; }
        public uint time { get; set; }
    }

    public class StateAutoControlMessageArgs : EventArgs
    {
        public StateAutoControlMessageArgs(bool stateAutoControl_a)
        {
            stateAutoControl = stateAutoControl_a;
        }
        public bool stateAutoControl { get; set; }
    }

    public class IRMessageArgs : EventArgs
    {
        public IRMessageArgs(byte leftIR_a, byte leftEndIR_a, byte centerIR_a, 
            byte rigthIR_a, byte rigthEndIR_a)
        {
            leftEndIR = leftEndIR_a;
            leftIR = leftIR_a;
            centerIR = centerIR_a;
            rigthIR = rigthIR_a;
            rigthEndIR = rigthEndIR_a;
        }
        public byte leftIR { get; set; }
        public byte leftEndIR { get; set; }
        public byte centerIR { get; set; }
        public byte rigthIR { get; set; }
        public byte rigthEndIR { get; set; }
    }

    public class PositionMessageArgs : EventArgs
    {
        public PositionMessageArgs(uint timestamp_a, float xPos_a, float yPos_a, 
            float theta_a, float linearSpeed_a, float angularSpeed_a)
        {
            timestamp = timestamp_a;
            xPos = xPos_a;
            yPos = yPos_a;
            theta = theta_a;
            linearSpeed = linearSpeed_a;
            angularSpeed = angularSpeed_a;
        }
        public uint timestamp { get; set; }
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float theta { get; set; }
        public float linearSpeed { get; set; }
        public float angularSpeed { get; set; }
    }

    public class SetPositionMessageArgs : EventArgs
    {
        public SetPositionMessageArgs(float xPos_a, float yPos_a, float angleRadian_a)
        {
            xPos = xPos_a;
            yPos = yPos_a;
            angleRadian = angleRadian_a;
        }
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float angleRadian { get; set; }
    }

    public class TextMessageArgs : EventArgs
    {
        public TextMessageArgs(string text_a)
        {
            text = text_a;
        }
        public string text { get; set; }
    }

    #region MsgDecoder Events
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
    #endregion


    #region Serial Events
    public class AttemptsEventArgs : EventArgs
    {
        public byte attempts { get; set; }

        public AttemptsEventArgs(byte attempts_a)
        {
            attempts = attempts_a;
        }
    }

    public class COMEventArgs : EventArgs
    {
        public string COM { get; set; }
        public COMEventArgs(string COM_a)
        {
            COM = COM_a;
        }

    }
    #endregion

}//End EventArgsLibrary
