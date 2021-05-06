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
using WpfFirstDisplay;
using Constants;
using ConsoleRobot;


namespace ConsoleRobot
{
    class Program
    {
        static serial serial;
        static MsgDecoder msgDecoder;
        static MsgEncoder msgEncoder;
        static MsgGenerator msgGenerator;
        static MsgProcessor msgProcessor;
        static FirstDisplayControl interfaceRobot;

        static object ExitLock = new object();

        private static bool serial_viewer = true;
        private static bool hex_viewer = true;
        private static bool hex_error_viewer = true;
        private static bool hex_sender = true;
        private static bool hex_error_sender = true;
        private static bool function_received = true;

        static void Main()
        {
            msgDecoder = new MsgDecoder();
            msgEncoder = new MsgEncoder();
            msgGenerator = new MsgGenerator();
            msgProcessor = new MsgProcessor();
            serial = new serial();


            #region Assign Events
            /// Creation of links between modules, except from and to the graphical interface  
            ConsoleFormat.ConsoleInformationFormat(ConsoleTitleFormatConst.MAIN, "Begin Booting Sequence", true);

            /// Creation of link between projects

            msgDecoder.OnCorrectChecksumReceivedEvent += msgProcessor.MessageProcessor; // Obligatory


            #region Console
            msgDecoder.OnMessageDecoderCreatedEvent += ConsoleFormat.PrintMessageDecoderCreated;
            msgEncoder.OnMessageEncoderCreatedEvent += ConsoleFormat.PrintMessageEncoderCreated;
            msgGenerator.OnMessageGeneratorCreatedEvent += ConsoleFormat.PrintMessageProcessorCreated;
            msgProcessor.OnMessageProcessorCreatedEvent += ConsoleFormat.PrintMessageGeneratorCreated;


            #region Serial Viewer
            if (serial_viewer)
            {
                serial.OnSerialConnectedEvent += ConsoleFormat.PrintConnectionAvailableToCOM;
                serial.OnAutoConnectionLaunchedEvent += ConsoleFormat.PrintAutoConnectionStarted;
                serial.OnNewCOMAttemptEvent += ConsoleFormat.PrintSerialAttemptConnectionToCOM;
                serial.OnCOMAvailableListEvent += ConsoleFormat.PrintListOfAvailableCOM;
                serial.OnWrongCOMAvailableEvent += ConsoleFormat.PrintWrongCOM;
                serial.OnCorrectCOMAvailableEvent += ConsoleFormat.PrintRigthCOM;
                serial.OnCOMAvailableEvent += ConsoleFormat.PrintAvailableCOM;
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
                msgProcessor.OnCheckInstructionReceivedEvent += ConsoleFormat.PrintProcessorCheckInstructionReceived;
                msgProcessor.OnIRMessageReceivedEvent += ConsoleFormat.PrintProcessorIRMessageReceived;
                msgProcessor.OnLEDMessageReceivedEvent += ConsoleFormat.PrintProcessorLEDMessageReceived;
                msgProcessor.OnMotorConsigneMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorConsigneMessageReceived;
                msgProcessor.OnMotorMeasuredMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorMeasuredMessageReceived;
                msgProcessor.OnMotorErrorMessageReceivedEvent += ConsoleFormat.PrintProcessorMotorErrorMessageReceived;
                msgProcessor.OnStateMessageReceivedEvent += ConsoleFormat.PrintProcessorStateMessageReceived;
                msgProcessor.OnManualControlStateReceivedEvent += ConsoleFormat.OnManualControlStateReceived;
                msgProcessor.OnPositionMessageReceivedEvent += ConsoleFormat.PrintProcessorPositionDataMessageReceived;
                msgProcessor.OnTextMessageReceivedEvent += ConsoleFormat.PrintProcessorTextMessageReceived;
                msgProcessor.OnUnknowFunctionReceivedEvent += ConsoleFormat.PrintUnknowFunctionWarning;
            }
            #endregion

            #endregion //End region Communication
            #endregion //End region Event

            bool isSerialConnected = serial.AutoConnectionSerial();
            StartRobotInterface();
            ConsoleFormat.ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MAIN, "End  Booting Sequence", true);
            Console.ReadKey();

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
                /*Attention, il est nécessaire d'ajouter PresentationFramework, PresentationCore, 
                 * WindowBase et votre wpf window application aux ressources. */
                interfaceRobot = new FirstDisplayControl();
                interfaceRobot.Loaded += RegisterRobotInterfaceEvents;
                interfaceRobot.ShowDialog();
            });
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        static void RegisterRobotInterfaceEvents(object sender, EventArgs e)
        {
            /// Display of events from the microcontroller
            msgProcessor.OnCheckInstructionReceivedEvent += interfaceRobot.UpdateCheckInstruction;
            msgProcessor.OnIRMessageReceivedEvent += interfaceRobot.UpdateTelematersValues;
            msgProcessor.OnLEDMessageReceivedEvent += interfaceRobot.UpdateLEDState;
            msgProcessor.OnMotorConsigneMessageReceivedEvent += interfaceRobot.UpdateIndependantSpeedConsigneValues;
            msgProcessor.OnMotorMeasuredMessageReceivedEvent += interfaceRobot.UpdateIndependantOdometrySpeed;
            msgProcessor.OnMotorErrorMessageReceivedEvent += interfaceRobot.UpdateIndependantSpeedErrorValues;
            msgProcessor.OnStateMessageReceivedEvent += interfaceRobot.UpdateRobotStateAndTimestamp;
            msgProcessor.OnManualControlStateReceivedEvent += interfaceRobot.UpdateManualControl;
            msgProcessor.OnPositionMessageReceivedEvent += interfaceRobot.UpdatePolarOdometrySpeed;
            msgProcessor.OnTextMessageReceivedEvent += interfaceRobot.UpdateTextBoxReception;
            // msgProcessor.OnUnknowFunctionReceivedEvent += ConsoleFormat.PrintUnknowFunctionWarning;           

            /// Sending orders from the GUI            
            interfaceRobot.OnSetLEDStateFromInterfaceGenerateEvent += msgGenerator.GenerateMessageLEDSetStateConsigneToRobot;
            interfaceRobot.OnSetAutoControlStateFromInterfaceGenerateEvent += msgGenerator.GenerateMessageSetAutoControlStateToRobot;
            interfaceRobot.OnSetMotorSpeedFromInterfaceGenerateEvent += msgGenerator.GenerateMessageMotorSetSpeedToRobot;
            interfaceRobot.OnSetRobotStateFromInterfaceGenerateEvent += msgGenerator.GenerateMessageSetStateToRobot;
            interfaceRobot.OnSetAutoControlStateFromInterfaceGenerateEvent += msgGenerator.GenerateMessageSetAutoControlStateToRobot;
            interfaceRobot.OnSetPositionFromInterfaceGenrateEvent += msgGenerator.GenerateMessageSetPositionToRobot;
            interfaceRobot.OnResetPositionFromInterfaceGenerateEvent += msgGenerator.GenerateMessageResetPositionToRobot;
            interfaceRobot.OnSentTextMessageFromInterfaceGenerateEvent += msgGenerator.GenerateMessageTextToRobot;
        }

        /******************************************* Trap app termination ***************************************/
        static bool exitSystem = false;
    }
}
