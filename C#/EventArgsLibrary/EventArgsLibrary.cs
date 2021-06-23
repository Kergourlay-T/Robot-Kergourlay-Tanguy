using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventArgsLibrary
{
    #region Generic Events
    public class StringEventArgs : EventArgs
    {
        public string Value { get; set; }
    }

    public class BoolEventArgs : EventArgs
    {
        public bool Value { get; set; }
    }

    public class ByteEventArgs : EventArgs
    {
        public byte Value { get; set; }
    }
    public class IntEventArgs : EventArgs
    {
        public int Value { get; set; }
    }

    public class DoubleEventArgs : EventArgs
    {
        public double Value { get; set; }
    }

    public class UshortEventArgs : EventArgs
    {
        public ushort Value { get; set; }
    }


    #endregion

    #region MessageDecoder
    public class DataReceivedArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }

    public class MessageDecodedArgs : EventArgs
    {
        public ushort MsgFunction { get; set; }
        public ushort MsgPayloadLength { get; set; }
        public byte[] MsgPayload { get; set; }
        public bool ChecksumCorrect { get; set; }
    }
    #endregion

    #region MesssageEncoder
    public class MessageEncodedArgs : EventArgs
    {
        public byte[] Msg { get; set; }
    }

    public class MessageToRobotArgs : EventArgs
    {
        public ushort MsgFunction { get; set; }
        public ushort MsgPayloadLength { get; set; }
        public byte[] MsgPayload { get; set; }
    }
    #endregion




    #region MessageGenerator and MessageProcessor
    public class SpeedConsigneToMotorArgs : EventArgs
    {
        public double V { get; set; }
        public byte MotorNumber { get; set; }
    }


    public class MessageDisplayArgs : EventArgs
    {
        public int RobotId { get; set; }

        public string Message { get; set; }
    }
    #endregion


    public class LEDMessageArgs : EventArgs
    {
        public ushort nbLed_a;
        public bool stateLed_a;
    }

    public class MotorMessageArgs : EventArgs
    {
        public sbyte leftMotor_a;
        public sbyte rightMotor_a;
    }

    public class IRMessageArgs : EventArgs
    {
        public byte leftIR_a;
        public byte leftEndIR_a;
        public byte centerIR_a;
        public byte rigthIR_a;
        public byte rigthEndIR_a;
    }

    public class StateMessageArgs : EventArgs
    {
        public ushort state_a;
        public uint time_a;
    }
    public class PositionMessageArgs : EventArgs
    {
        public uint time_a;
        public float xPos_a;
        public float yPos_a;
        public float theta_a;
        public float linearSpeed_a;
        public float angularSpeed_a;
    }

    public class SetPositionMessageArgs : EventArgs
    {
        public float xPos_a;
        public float yPos_a;
        public float angleRadian_a;
    }

    public class TextMessageArgs : EventArgs
    {
        public string text_a;
    }

    public class PolarSpeedOdometryData : EventArgs
    {
        public float xConsigne_a;
        public float thetaConsigne_a;
        public float xError_a;
        public float thetaError_a;
        public float xMeasured_a;
        public float thetaMeasured_a;
        public float xCommand_a;
        public float thetaCommand_a;
    }

    public class PolarSpeedPidData : EventArgs
    {
        public float Px_a;
        public float Ix_a;
        public float Dx_a;
        public float Ptheta_a;
        public float Itheta_a;
        public float Dtheta_a;
    }


    #region Serial Events
    public class AttemptsEventArgs : EventArgs
    {
        public byte Attempts { get; set; }
    }

    public class SerialEventArgs : EventArgs
    {
        public string Serial { get; set; }
    }
    #endregion

}
