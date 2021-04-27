using System;

namespace EventArgsLibrary
{
    public class MessageByteArgs : EventArgs
    {

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
        public MotorMessageArgs(byte leftMotorSpeed_a, byte rigthMotorSpeed_a)
        {
            leftMotorSpeed = leftMotorSpeed_a;
            rightMotorSpeed = rigthMotorSpeed_a;
        }
        public byte leftMotorSpeed { get; set; }
        public byte rightMotorSpeed { get; set; }
    }

    public class SetStateArgs : EventArgs
    {

    }

    public class IRMessageArgs : EventArgs
    {
        public IRMessageArgs(byte leftIR_a, byte centerIR_a, byte rigthIR_a)
        {
            leftIR = leftIR_a;
            centerIR = centerIR_a;
            rigthIR = rigthIR_a;
        }
        public byte leftIR;
        public byte centerIR;
        public byte rigthIR;
    }

    public class PositionMessageArgs : EventArgs
    {
        public PositionMessageArgs(uint timestamp_a, float xPos_a, float yPos_a, float angleRadiant_a, float linearSpeed_a, float angularSpeed_a)
        {
            timestamp = timestamp_a;
            xPos = xPos_a;
            yPos = yPos_a;
            angleRadiant = angleRadiant_a;
            linearSpeed = linearSpeed_a;
            angularSpeed = angularSpeed_a;
        }
        public uint timestamp { get; set; }
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float angleRadiant { get; set; }
        public float linearSpeed { get; set; }
        public float angularSpeed { get; set; }
    }

    public class TextMessageArgs : EventArgs
    {
        TextMessageArgs(string Text_a)
        {
            Text = Text_a;
        }
        public string Text { get; set; }
    }

}
