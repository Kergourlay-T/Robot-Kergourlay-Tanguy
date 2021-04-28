using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Robot
{
    class ConsoleFormat
    {
        static public class ConsoleTitleFormatConst
        {
            public const string decoder = "DECODER";
            public const string encoder = "ENCODER";
            public const string generator = "GENERATOR";
            public const string processor = "PROCESSOR";
            public const string serial = "SERIAL";
        }

        private static long message_received_index = 0;
        private static long message_sender_index = 0;

        #region General Method
        static public void ConsoleTitleFormat(string title, bool isCorrect)
        {
            if (Console.CursorLeft != 0)
            {
                Console.WriteLine();
            }
            Console.Write("[");
            if (isCorrect)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(title);
            Console.ResetColor();
            Console.Write("] ");
        }
        #endregion

        static public void ConsoleInformationFormat(string format, string message, bool isCorrect = true)
        {
            ConsoleTitleFormat(format, isCorrect);
            Console.Write(message);
        }
        static public void ConsoleListFormat(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write("   -" + message);
        }


        #region Serial Init
        static public void PrintMessageDecoderCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.decoder, "Message Decoder is launched", true);
        }

        static public void PrintMessageEncoderCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.encoder, "Message Encoder is launched", true);
        }

        static public void PrintMessageProcessorCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.processor, "Message Processor is launched", true);
        }

        static public void PrintMessageGeneratorCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.generator, "Message Generator is launched", true);
        }
        #endregion


        #region Serial
        static public void PrintAutoConnectionStarted (object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "Auto-connection started", true);
        }
        static public void PrintListOfAvailableCOM (object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "List of COM availables :", true);
        }
        static public void PrintAvailableCOM (object sender, COMEventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "- COM " + e.COM);
        }
        static public void PrintSerialAttemptConnectionToCOM(object sender, AttemptsEventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "Attempt #" + e.attempts, true);
        }
        static public void PrintRigthCOM(object sender, COMEventArgs e)
        {
            ConsoleListFormat(e.COM, ConsoleColor.Green);
        }
        static public void PrintWrongCOM (object sender, COMEventArgs e)
        {
            ConsoleListFormat(e.COM, ConsoleColor.Red);
        }
        static public void PrintNoConnectionAvailableToCOM (object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "No connection enable", false);
        }
        static public void PrintConnectionAvailableToCOM (object sender, COMEventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "COM available : "+e.COM , true);
        }
        static public void PrintErrorWhileAttemptingCOM (object sender, EventArgs e)
        {
            ConsoleInformationFormat(ConsoleTitleFormatConst.serial, "ERROR while attempting COM", false);
        }
        #endregion


        #region Deocder Errors 
        static public void PrintUnknowFunctionWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : A UNKNOW FUNCTION HAS BEEN RECEIVED");
            Console.ResetColor();
        }
        static public void PrintOverLengthWarning (object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : A MESSAGE HAS EXCEDED THE MAX LENGTH");
            Console.ResetColor();
        }
        static public void PrintWrongPayloadLengthWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : THE PAYLOAD LENGTH IS INAPPROPIATE FOR THIS FUNCTION");
            Console.ResetColor();
        }
        static public void PrintWrongChecksumWarning (object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : A MESSAGE HAS BEEN CORRUPTED");
            Console.ResetColor();
        }
        #endregion

        #region Decoder : Reconstruct Message
        static public void PrintDecoderUnknowByte(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("0x" + e.ToString("X2") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderSOF (object sender, DecodeByteArgs e)
        {
            Console.WriteLine();
            Console.ResetColor();
            Console.Write(message_received_index++ + " : ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0x" + e.ToString("2X") +" ");
            Console.ResetColor();
        }

        static public void PrintDecoderFunctionMSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderFunctionLSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }

        static public void PrintDecoderPayloadLengthMSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderPayloadLengthLSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderRigthChecksum(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderWrongChecksum(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("0x" + e.ToString("2X") + " ");
            Console.ResetColor();
        }

        #endregion

        #region Encoder : Construct Message
        static public void EncodeAndSendMessage()

        #endregion

        #region Encoder Errors
        static public void PrintSerialDisconnectedWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : MESSAGE CAN'T BE SEND BECAUSE SERAIL IS DISCONNECTED");
            Console.ResetColor();
        }
        static public void PrintUknowFunctionSendhWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : AN UNKNOW FUNCTION WAS TRIED TO BE SENT");
            Console.ResetColor();
        }
        static public void PrintWrongPayloadLengthSendhWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : A WRONG PAYLOAD LENGTH WAS TRIED TO BE SENT");
            Console.ResetColor();
        }
        #endregion

        #region Class Args
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
            public string COM {get;set;}
            public COMEventArgs(string COM_a)
            {
                COM = COM_a;
            }

        }
        #endregion

    }
}
