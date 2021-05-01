using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using Serial;
using EventArgsLibrary;
using MessageDecoder;
using MessageEncoder;
using MessageGenerator;
using MessageProcessor;
using InterfaceRobot;
using Constants;

namespace ConsoleRobot
{
    public class Program
    {
        public static serial serial;
        public static MsgDecoder msgDecoder;
        public static MsgEncoder msgEncoder;
        public static MsgGenerator msgGenerator;
        public static MsgProcessor msgProcessor;
        public static InterfaceRobot interfaceRobot;

        static object ExitLock = new object();
        public Program()
        {
            serial serial = new serial();
            MsgDecoder msgDecoder = new MsgDecoder();
            MsgEncoder msgEncoder = new MsgEncoder();
            MsgGenerator msgGenerator = new MsgGenerator();
            MsgProcessor msgProcessor = new MsgProcessor();
        }

        private static bool serial_viewer = true;
        private static bool hex_viewer = true;
        private static bool hex_error_viewer = true;
        private static bool hex_sender = true;
        private static bool hex_error_sender = true;
        private static bool function_received = true;

        static void Main()
        {
            /// Creation of links between modules, except from and to the graphical interface  
            ConsoleFormat.ConsoleInformationFormat(ConsoleTitleFormatConst.MAIN, "Begin Booting Sequence", true);

            msgDecoder.OnMessageDecoderCreatedEvent += ConsoleFormat.PrintMessageDecoderCreated;
            msgEncoder.OnMessageEncoderCreatedEvent += ConsoleFormat.PrintMessageEncoderCreated;
            msgGenerator.OnMessageGeneratorCreatedEvent += ConsoleFormat.PrintMessageProcessorCreated;
            msgProcessor.OnMessageProcessorCreatedEvent += ConsoleFormat.PrintMessageGeneratorCreated;

            #region Event
            #region Communication 

            #region Serial Viewer
            if (serial_viewer)
            {
                serial.OnSerialConnectedEvent += ConsoleFormat.PrintNoConnectionAvailableToCOM;
                serial.OnAutoConnectionLaunchedEvent += ConsoleFormat.PrintAutoConnectionStarted;
                serial.OnNewCOMAttemptEvent += ConsoleFormat.PrintSerialAttemptConnectionToCOM;
                serial.OnCOMAvailableListEvent += ConsoleFormat.PrintListOfAvailableCOM;
                serial.OnWrongCOMAvailableEvent += ConsoleFormat.PrintAvailableCOM;
                serial.OnCorrectCOMAvailableEvent += ConsoleFormat.PrintRigthCOM;
                serial.OnCOMAvailableEvent += ConsoleFormat.PrintWrongCOM;
                serial.OnNoConnectionAvailablEvent += ConsoleFormat.PrintNoConnectionAvailableToCOM;
                serial.OnErrorWhileWhileAttemptingCOMEvent += ConsoleFormat.PrintErrorWhileAttemptingCOM;
            }
            #endregion

            #region Hex Viewer
            if (hex_viewer)
            {
                msgDecoder.OnUnknowByteReceivedEvent += ConsoleFormat.PrintDecoderUnknowByte;
                msgDecoder.OnSOFByteReceivedEvent += ConsoleFormat.PrintDecoderSOF;
                msgDecoder.OnFunctionMSBByteReceivedEvent += ConsoleFormat.PrintDecoderFunctionMSB;
                msgDecoder.OnFunctionLSBByteReceivedEvent += ConsoleFormat.PrintDecoderFunctionLSB;
                msgDecoder.OnPayloadLenghtMSBByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadLengthMSB;
                msgDecoder.OnPayloadLenghtLSBByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadLengthLSB;
                msgDecoder.OnPayloadByteReceivedEvent += ConsoleFormat.PrintDecoderPayloadByte;
                msgDecoder.OnCorrectChecksumReceivedEvent += ConsoleFormat.PrintDecoderRigthChecksum;
                msgDecoder.OnWrongChecksumReceivedEvent += ConsoleFormat.PrintDecoderWrongChecksum;
            }
            #endregion

            #region Hex Viewer Error
            if (hex_error_viewer)
            {
                msgDecoder.OnUnknowFunctionEvent += ConsoleFormat.PrintUnknowFunctionWarning;
                msgDecoder.OnOverLenghtMessageEvent += ConsoleFormat.PrintOverLengthWarning;
                msgDecoder.OnWrongLenghtEvent += ConsoleFormat.PrintWrongPayloadLengthWarning;
                msgDecoder.OnWrongChecksumReceivedEvent += ConsoleFormat.PrintWrongChecksumWarning;
            }
            #endregion

            #region Hex Sender
            if (hex_sender)
            {
                msgEncoder.OnSendMessageEvent += ConsoleFormat.PrintEncoderSendMessage;
            }

            #endregion

            #region Hex Sender Error 
            if (hex_error_sender)
            {
                msgEncoder.OnSerialDeconnectedEvent += ConsoleFormat.PrintSerialDisconnectedWarning;
                msgEncoder.OnWrongPayloadSendEvent += ConsoleFormat.PrintWrongPayloadLengthSendWarning;
                msgEncoder.OnUnknowFunctionSentEvent += ConsoleFormat.PrintUnknowFunctionSendWarning;
            }
            #endregion

            #region Function Processor
            if (function_received)
            {
                msgProcessor.OnIRMessageReceivedEvent += ConsoleFormat.PrintProcessorIRMessageReceived;
                msgProcessor.OnLEDMessageReceivedEvent += ConsoleFormat.PrintProcessorLEDMessageReceived;
                msgProcessor.OnMotorMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorSpeedMessageReceived;
                msgProcessor.OnStateMessageReceivedEvent += ConsoleFormat.PrintProcessorStateMessageReceived;
                msgProcessor.OnPositionMessageReceivedEvent += ConsoleFormat.PrintProcessorPositionDateMessageReceived;
                msgProcessor.OnTextMessageReceivedEvent += ConsoleFormat.PrintProcessorTextMessageReceived;
                msgProcessor.OnUnknowFunctionReceivedEvent += ConsoleFormat.PrintUnknowFunctionWarning;
            }
            #endregion

            #endregion //End region Communication
            #endregion //End region Event

            bool isSerialConnected = serial.AutoConnectionSerial();
            msgDecoder.OnCorrectChecksumReceivedEvent += msgProcessor.MessageProcessor; // Obligatory

            ConsoleFormat.ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MAIN, "End  Booting Sequence", true);
            msgGenerator.GenerateMessageLEDSetStateConsigneToRobot(1, true);
            Console.ReadKey();

            StartRobotInterface();

            while (!exitSystem)
            {
                Thread.Sleep(500);
            }
        }//End Main



        static Thread t1;
        static void StartRobotInterface()
        {
            t1 = new Thread(() =>
            {
                //Attention, il est nécessaire d'ajouter PresentationFramework, PresentationCore, WindowBase and your wpf window application aux ressources.
                interfaceRobot = new InterfaceRobot();
                interfaceRobot.Loaded += RegisterRobotInterfaceEvents;
                interfaceRobot.ShowDialog();
            });
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        static void RegisterRobotInterfaceEvents(object sender, EventArgs e)
        {
            /// Display of events from the microcontroller
            msgProcessor.OnIRMessageReceivedEvent += ConsoleFormat.PrintProcessorIRMessageReceived;
            msgProcessor.OnLEDMessageReceivedEvent += ConsoleFormat.PrintProcessorLEDMessageReceived;
            msgProcessor.OnMotorMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorSpeedMessageReceived;
            msgProcessor.OnStateMessageReceivedEvent += ConsoleFormat.PrintProcessorStateMessageReceived;
            msgProcessor.OnPositionMessageReceivedEvent += ConsoleFormat.PrintProcessorPositionDateMessageReceived;
            msgProcessor.OnTextMessageReceivedEvent += ConsoleFormat.PrintProcessorTextMessageReceived;
            msgProcessor.OnUnknowFunctionReceivedEvent += ConsoleFormat.PrintUnknowFunctionWarning;



            /// Sending orders from the GUI


            /// Affichage des infos en provenance du décodeur de message
            msgDecoder.OnMessageDecodedEvent += interfaceRobot.DisplayMessageDecoded;
            msgDecoder.OnMessageDecodedErrorEvent += interfaceRobot.DisplayMessageDecodedError;

        }

        /******************************************* Trap app termination ***************************************/
        static bool exitSystem = false;


    }//End Program
}//End ConsolerOBOT
    
