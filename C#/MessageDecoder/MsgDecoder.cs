using System;
using System.Text;
using EventArgsLibrary;
using Protocol;


namespace MessageDecoder
{
    public class MsgDecoder
    {
        #region Input Callback   
        public void DecodeMsgReceived(object sender, DataReceivedArgs e)
        {
            foreach (var b in e.Data)
                DecodeMessage(b);
        }
        #endregion
        public MsgDecoder()
        {

        }

        #region Attributes
        private enum StateReception
        {
            Waiting,
            FunctionMSB,
            FunctionLSB,
            PayloadLengthMSB,
            PayloadLengthLSB,
            Payload,
            CheckSum
        }

        static StateReception rcvState = StateReception.Waiting;

        private static ushort msgDecodedFunction;
        private static ushort msgDecodedPayloadLenght;
        private static byte[] msgDecodedPayload;
        private static int msg_decoded_payload_index = 0;
        #endregion

        #region Main Method
        public void DecodeMessage(byte b)
        {
            switch (rcvState)
            {
                case StateReception.Waiting:
                    if (b == ConstsProtocol.START_OF_FRAME)
                        rcvState = StateReception.FunctionMSB;
                    break;

                case StateReception.FunctionMSB:
                    msgDecodedFunction = (ushort)(b << 8);
                    rcvState = StateReception.FunctionLSB;
                    break;

                case StateReception.FunctionLSB:
                    msgDecodedFunction += (ushort)(b << 0);
                    if (IsFunctionExist(msgDecodedFunction))
                        rcvState = StateReception.PayloadLengthMSB;
                    else
                        OnUnknowFunction(msgDecodedFunction);
                    break;

                case StateReception.PayloadLengthMSB:
                    msgDecodedPayloadLenght = (ushort)(b << 8);
                    rcvState = StateReception.PayloadLengthLSB;
                    break;

                case StateReception.PayloadLengthLSB:
                    msgDecodedPayloadLenght += (ushort)(b << 0);
                    if (!IsPayloadExist(msgDecodedPayloadLenght))
                        OnNoPayload();
                    if (!IsLessThanMaximalLenghtSize(msgDecodedFunction))
                        OnOverLengthSize();
                    if (!IsPayloadLengthCorrect(msgDecodedFunction, msgDecodedPayloadLenght))
                        OnWrongPayloadLenght();
                    else
                    {
                        rcvState = StateReception.Payload;
                        msg_decoded_payload_index = 0;
                        msgDecodedPayload = new byte[msgDecodedPayloadLenght];
                    }
                    break;

                case StateReception.Payload:
                    msgDecodedPayload[msg_decoded_payload_index++] = b;
                    if (msg_decoded_payload_index == msgDecodedPayloadLenght)
                        rcvState = StateReception.CheckSum;
                    break;

                case StateReception.CheckSum:
                    byte receivedChecksum = b;
                    if (receivedChecksum == CalculateChecksum(msgDecodedFunction,
                        msgDecodedPayloadLenght, msgDecodedPayload))
                        OnMessageDecoded(msgDecodedFunction, msgDecodedPayloadLenght, msgDecodedPayload);
                    else
                        OnMessageDecodedError(msgDecodedFunction, msgDecodedPayloadLenght, msgDecodedPayload);
                    rcvState = StateReception.Waiting;
                    break;
            }
        }
        #endregion

        #region Condition Statements
        public static bool IsLessThanMaximalLenghtSize(ushort x) => x < ConstsProtocol.MAX_MSG_LENGTH;
        public static bool IsPayloadExist(ushort x) => x > 0;
        public static bool IsFunctionExist(ushort x) => PayloadCommands.CommandToPayload.ContainsKey(x);
        public static bool IsPayloadLengthCorrect(ushort x, ushort y)
            => PayloadCommands.CommandToPayload[x] == -1 || PayloadCommands.CommandToPayload[x] == y;
        #endregion

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
        #endregion

        #region Error Events 
        public event EventHandler<StringEventArgs> OnUnknowFunctionEvent;
        public virtual void OnUnknowFunction(ushort msgFunctionError)
            => OnUnknowFunctionEvent?.Invoke(this, new StringEventArgs { Value = msgFunctionError.ToString() });

        public event EventHandler<StringEventArgs> OnNoPayloadEvent;
        public virtual void OnNoPayload() => OnNoPayloadEvent?.Invoke(this, new StringEventArgs
        {
            Value = "Message without payload"
        });

        public event EventHandler<StringEventArgs> OnOverLengthSizeEvent;
        public virtual void OnOverLengthSize() => OnOverLengthSizeEvent?.Invoke(this, new StringEventArgs
        {
            Value = "Message beyond the maximum value"
        });

        public event EventHandler<StringEventArgs> OnWrongPayloadLenghtEvent;
        public virtual void OnWrongPayloadLenght() => OnWrongPayloadLenghtEvent?.Invoke(this, new StringEventArgs
        {
            Value = "PayloadLength isn't suitable for this function"
        });
        public event EventHandler<MessageDecodedArgs> OnMessageDecodedErrorEvent;
        public virtual void OnMessageDecodedError(ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload, bool checksumCorrect = false)
           => OnMessageDecodedErrorEvent?.Invoke(this, new MessageDecodedArgs
           {
               MsgFunction = msgFunction,
               MsgPayloadLength = msgPayloadLength,
               MsgPayload = msgPayload,
               ChecksumCorrect = checksumCorrect
           });
        #endregion

        #region Output Events
        public event EventHandler<MessageDecodedArgs> OnMessageDecodedEvent;
        public virtual void OnMessageDecoded(ushort msgFunction, ushort msgPayloadLength, byte[] msgPayload, bool checksumCorrect = true)
            => OnMessageDecodedEvent?.Invoke(this, new MessageDecodedArgs
            {
                MsgFunction = msgFunction,
                MsgPayloadLength = msgPayloadLength,
                MsgPayload = msgPayload,
                ChecksumCorrect = checksumCorrect
            });
        #endregion


    }
}
