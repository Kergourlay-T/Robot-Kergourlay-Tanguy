using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRobot
{
    public class Program
    {
        static Serial.Serial serial;

        private static bool serial_viewer = true;
        private static bool hex_viewer = true;
        private static bool hex_error_viewer = true;
        private static bool hex_sender = true;
        private static bool hex_error_sender = true;
        private static bool function_received = true;
        static void Main()
        {
            ConsoleFormat.ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MAIN, "Begin Booting Sequence", true);

            Serial.Serial serial = new Serial.Serial();
            MessageDecoder.MsgDecoder msgDecoder = new MessageDecoder.MsgDecoder();

            Serial.Serial.msgDecoder.OnMessageDecoderCreatedEvent += ConsoleFormat.PrintMessageDecoderCreated;
            Serial.Serial.msgEncoder.OnMessageEncoderCreatedEvent += ConsoleFormat.PrintMessageEncoderCreated;
            Serial.Serial.msgGenerator.OnMessageGeneratorCreatedEvent += ConsoleFormat.PrintMessageProcessorCreated;
            Serial.Serial.msgProcessor.OnMessageProcessorCreatedEvent += ConsoleFormat.PrintMessageGeneratorCreated;

            #region Event
            #region Communication 

            #region Serial Viewer
            if (serial_viewer)
            {
                serial.OnSerialConnectedEvent += ConsoleFormat.PrintNoConnectionAvailableToCOM;
                serial.OnAutoConnectionLaunchedEvent += ConsoleFormat.PrintAutoConnectionStarted;
                serial.OnNewSerialAttemptEvent += ConsoleFormat.PrintSerialAttemptConnectionToCOM;
                serial.OnSerialAvailableListEvent += ConsoleFormat.PrintListOfAvailableCOM;
                serial.OnWrongCOMAvailableEvent += ConsoleFormat.PrintAvailableCOM;
                serial.OnCorrectCOMAvailableEvent += ConsoleFormat.PrintRigthCOM;
                serial.OnSerialAvailableEvent += ConsoleFormat.PrintWrongCOM;
                serial.OnNoConnectionAvailablEvent += ConsoleFormat.PrintNoConnectionAvailableToCOM;
                serial.OnErrorWhileWhileAttemptingCOMEvent += ConsoleFormat.PrintErrorWhileAttemptingCOM;
            }
            #endregion

            #region Hex Viewer
            if (hex_viewer)
            {
                Serial.Serial.msgDecoder.OnUnknowByteReceivedEvent += ConsoleFormat.PrintDecoderUnknowByte;
                Serial.Serial.msgDecoder.OnSOFByteReceivedEvent += ConsoleFormat.PrintDecoderSOF;
                Serial.Serial.msgDecoder.OnFunctionMSBByteReceivedEvent += ConsoleFormat.PrintDecoderFunctionMSB;
                Serial.Serial.msgDecoder.OnFunctionLSBByteReceivedEvent += ConsoleFormat.PrintDecoderFunctionLSB;
                Serial.Serial.msgDecoder.OnPayloadLenghtMSBByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadLengthMSB;
                Serial.Serial.msgDecoder.OnPayloadLenghtLSBByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadLengthLSB;
                Serial.Serial.msgDecoder.OnPayloadByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadByte;
                Serial.Serial.msgDecoder.OnCorrectChecksumReceivedEvent += ConsoleFormat.PrintDecoderRigthChecksum;
                Serial.Serial.msgDecoder.OnWrongChecksumReceivedEvent += ConsoleFormat.PrintDecoderWrongChecksum;
            }
            #endregion

            #region Hex Viewer Error
            if (hex_error_viewer)
            {
                Serial.Serial.msgDecoder.OnUnknowFunctionEvent += ConsoleFormat.PrintUnknowFunctionWarning;
                Serial.Serial.msgDecoder.OnOverLenghtMessageEvent += ConsoleFormat.PrintOverLengthWarning;
                Serial.Serial.msgDecoder.OnWrongLenghtEvent += ConsoleFormat.PrintWrongPayloadLengthWarning;
                Serial.Serial.msgDecoder.OnWrongChecksumReceivedEvent += ConsoleFormat.PrintWrongChecksumWarning;
            }
            #endregion

            #region Hex Sender
            if (hex_sender)
            {
                Serial.Serial.msgEncoder.OnSendMessageEvent += ConsoleFormat.PrintEncoderSendMessage;
            }

            #endregion

            #region Hex Sender Error 
            if (hex_error_sender)
            {
                Serial.Serial.msgEncoder.OnSerialDeconnectedEvent += ConsoleFormat.PrintSerialDisconnectedWarning;
                Serial.Serial.msgEncoder.OnWrongPayloadSendEvent += ConsoleFormat.PrintWrongPayloadLengthSendWarning;
                Serial.Serial.msgEncoder.OnUnknowFunctionSentEvent += ConsoleFormat.PrintUnknowFunctionSendWarning;
            }
            #endregion

            #region Function Processor
            if (function_received)
            {
                Serial.Serial.msgProcessor.OnIRMessageReceivedEvent += ConsoleFormat.PrintProcessorIRMessageReceived;
                Serial.Serial.msgProcessor.OnLEDMessageReceivedEvent += ConsoleFormat.PrintProcessorLEDMessageReceived;
                Serial.Serial.msgProcessor.OnMotorMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorSpeedMessageReceived;
                Serial.Serial.msgProcessor.OnStateMessageReceivedEvent += ConsoleFormat.PrintProcessorStateMessageReceived;
                Serial.Serial.msgProcessor.OnPositionMessageReceivedEvent += ConsoleFormat.PrintProcessorPositionDateMessageReceived;
                Serial.Serial.msgProcessor.OnTextMessageReceivedEvent += ConsoleFormat.PrintProcessorTextMessageReceived;
                Serial.Serial.msgProcessor.OnUnknowFunctionReceivedEvent += ConsoleFormat.PrintUnknowFunctionWarning;
            }
            #endregion

            #endregion //End region Communication
            #endregion //End region Event

            bool isSerialConnected = serial.AutoConnectSerial();
            Serial.Serial.msgDecoder.OnCorrectChecksumReceivedEvent += Serial.Serial.msgProcessor.MessageProcessor; // Obligatory

            ConsoleFormat.ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MAIN, "End  Booting Sequence", true);
            Serial.Serial.msgGenerator.GenerateMessageLEDSetStateConsigneToRobot(1, true);
            Console.ReadKey();

        }
    }


    }//End Program
}//End ConsolerOBOT
    
