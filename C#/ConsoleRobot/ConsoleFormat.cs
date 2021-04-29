using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;

namespace ConsoleRobot
{
    class ConsoleFormat
    {

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
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.DECODER, "Message Decoder is launched", true);
        }

        static public void PrintMessageEncoderCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.ENCODER, "Message Encoder is launched", true);
        }

        static public void PrintMessageProcessorCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.PROCESSOR, "Message Processor is launched", true);
        }

        static public void PrintMessageGeneratorCreated(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.GENERATOR, "Message Generator is launched", true);
        }
        #endregion

        #region Serial
        static public void PrintAutoConnectionStarted(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "Auto-connection started", true);
        }
        static public void PrintListOfAvailableCOM(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "List of COM availables :", true);
        }
        static public void PrintAvailableCOM(object sender, COMEventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "- COM " + e.COM);
        }
        static public void PrintSerialAttemptConnectionToCOM(object sender, AttemptsEventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "Attempt #" + e.attempts, true);
        }
        static public void PrintRigthCOM(object sender, COMEventArgs e)
        {
            ConsoleListFormat(e.COM, ConsoleColor.Green);
        }
        static public void PrintWrongCOM(object sender, COMEventArgs e)
        {
            ConsoleListFormat(e.COM, ConsoleColor.Red);
        }
        static public void PrintNoConnectionAvailableToCOM(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "No connection enable", false);
        }
        static public void PrintConnectionAvailableToCOM(object sender, COMEventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "COM available : " + e.COM, true);
        }
        static public void PrintErrorWhileAttemptingCOM(object sender, EventArgs e)
        {
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.SERIAL, "ERROR while attempting COM", false);
        }
        #endregion

        #region Decoder : Reconstruct Message
        static public void PrintDecoderUnknowByte(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("0x" + e.b.ToString("X2") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderSOF(object sender, DecodeByteArgs e)
        {
            Console.WriteLine();
            Console.ResetColor();
            Console.Write(message_received_index++ + " : ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }

        static public void PrintDecoderFunctionMSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderFunctionLSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }

        static public void PrintDecoderPayloadLengthMSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderPayloadLengthLSB(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderRigthChecksum(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
        }
        static public void PrintDecoderWrongChecksum(object sender, DecodeByteArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("0x" + e.b.ToString("2X") + " ");
            Console.ResetColor();
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
        static public void PrintOverLengthWarning(object sender, EventArgs e)
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
        static public void PrintWrongChecksumWarning(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("WARNING : A MESSAGE HAS BEEN CORRUPTED");
            Console.ResetColor();
        }
        #endregion

        #region Encoder : Construct Message
        static public void PrintEncoderSendMessage(object sender, MessageByteArgs e)
        {
            Console.ResetColor();
            Console.Write(message_received_index++ + " : ");
            Console.ForegroundColor = ConsoleColor.Black;

            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("0x" + e.SOF.ToString("2X"));
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + e.functionMSB.ToString("2X") + " 0x" + e.functionLSB.ToString("2X") + " ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("0x" + e.payloadMSB.ToString("2X") + " 0x" + e.payloadLSB.ToString("2X") + " ");
            Console.BackgroundColor = ConsoleColor.White;
            foreach (byte b in e.msgPayload)
            {
                ConsoleListFormat("0x" + b.ToString("2X"));
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("0x" + e.checksum.ToString("2X"));
            Console.ResetColor();
        }
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

        #region Processor
        static public void PrintProcessorIRMessageReceived(object sender, IRMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.IR, "IR received :", true);
            ConsoleListFormat("left IR   : " + e.leftIR);
            ConsoleListFormat("center IR : " + e.centerIR);
            ConsoleListFormat("rigth IR  : " + e.rigthIR);
        }

        static public void PrintProcessorLEDMessageReceived(object sender, LEDMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.LED, "LED" + e.LEDNumber + "state received", true);
            ConsoleListFormat("LED" + e.LEDNumber + "State : " + e.LEDState);
        }

        static public void PrintProcessorMotorSpeedMessageReceived(object sender, MotorMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MOTOR, "motor speed :", true);
            ConsoleListFormat("left motor speed : " + e.leftMotorSpeed);
            ConsoleListFormat("rigth motor speed : " + e.rightMotorSpeed);
        }

        static public void PrintProcessorTextMessageReceived(object sender, TextMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.TEXT, "text received :", true);
            ConsoleListFormat(e.text);
        }

        static public void PrintprocessorStateMessageReceived(object sender, StateMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.STATE_ROBOT, "Actual State: " + e.state + " - " + e.time, true);
        }

        static public void PrintProcessorPositionDateMessageReceived(object sender, PositionMessageArgs e)
        {
            Console.ResetColor();
            ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.POSITIONDATA, "Actual Position :", true);
            ConsoleListFormat("timestamp : " + e.timestamp);
            ConsoleListFormat("psotion x : " + e.xPos);
            ConsoleListFormat("psotion y : " + e.yPos);
            ConsoleListFormat("angle radiant : " + e.angleRadiant);
            ConsoleListFormat("linear speed : " + e.linearSpeed);
            ConsoleListFormat("angular speed : " + e.angularSpeed);
        }
        #endregion

    }
}
