using MessageDecoder;
using MessageEncoder;
using MessageGenerator;
using MessageProcessor;
using SerialAutoConnection;
using ConsoleFormat;
using WpfFirstDisplay;
using System;
using System.Threading;
using EventArgsLibrary;
using System.Runtime.InteropServices;
using Utilities;
using System.Collections.Generic;

namespace RobotConsole
{

    class Program
    {
        static bool usingRobotInterface = true;

        static MsgDecoder msgDecoder;
        static MsgEncoder msgEncoder;
        static MsgGenerator msgGenerator;
        static MsgProcessor msgProcessor;
        static SerialAutoConnect serialPort;
        static FirstDisplayControl interfaceRobot;

        static object ExitLock = new object();

        static void Main(string[] args)
        {
            /// We add an event handler to detect the closing of the application
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            msgDecoder = new MsgDecoder();
            msgEncoder = new MsgEncoder();
            msgGenerator = new MsgGenerator();
            msgProcessor = new MsgProcessor();
            serialPort = new SerialAutoConnect();


            /// Creation of links between modules, except from and to the graphical interface  
            Console.WriteLine("[MAIN] Begin Booting Sequence");

            /// Creation of link between projects
            msgDecoder.OnMessageDecodedEvent += msgProcessor.ProcessRobotDecodedMessage;

            /// Management of messages sent by the robot
            msgGenerator.OnMessageToRobotGeneratedEvent += msgEncoder.EncodeMessageToRobot;
            msgEncoder.OnMessageEncodedEvent += serialPort.SendMessageToRobot;

            /// Management of messages received by the robot
            serialPort.OnDataReceivedEvent += msgDecoder.DecodeMsgReceived;
            msgDecoder.OnMessageDecodedEvent += msgProcessor.ProcessRobotDecodedMessage;

            /// management of messages to be displayed in the console
            serialPort.OnSerialConnectedEvent += ConsolePrint.OnPrintEvent;
            RegisterConsolePrintEvents();

            /// Launching GUI
            if (usingRobotInterface)
                StartRobotInterface();

            Console.Write("[MAIN] End Booting Sequence");

            while (!exitSystem)
            {
                Thread.Sleep(500);
            }
        }

        #region RobotInterface
        static Thread t1;
        static void StartRobotInterface()
        {
            t1 = new Thread(() =>
            {
                //Please note that it is necessary to add PresentationFramework,
                //PresentationCore, WindowBase and your wpf window application to the resources.
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
            msgProcessor.OnPositionMessageReceivedEvent += interfaceRobot.UpdatePositionFromOdometry;
            msgProcessor.OnTextMessageReceivedEvent += interfaceRobot.UpdateTextBoxReception;
            msgProcessor.OnPolarSpeedOdometryDataReceivedEvent += interfaceRobot.UpdatePolarSpeedOdometryValues;
            msgProcessor.OnPolarSpeedCorrectionsDataReceivedEvent += interfaceRobot.UpdatePolarSpeedCorrectionValues;
            msgProcessor.OnPolarSpeedLimitGainsReceivedEvent += interfaceRobot.UpdatePolarSpeedCorrectionGains;
            msgProcessor.OnPolarSpeedPidGainsDataReceivedEvent += interfaceRobot.UpdatePolarSpeedCorrectionLimits;

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
        #endregion

        static void RegisterConsolePrintEvents()
        {
            /// Error that can occur in msgDecoder
            msgDecoder.OnMessageDecodedErrorEvent += ConsolePrint.OnPrintDecodedMessage;
            msgDecoder.OnUnknowFunctionEvent += ConsolePrint.OnPrintCorruptedMessage;
            msgDecoder.OnNoPayloadEvent += ConsolePrint.OnPrintCorruptedMessage;
            msgDecoder.OnOverLengthSizeEvent += ConsolePrint.OnPrintCorruptedMessage;
            msgDecoder.OnWrongPayloadLenghtEvent += ConsolePrint.OnPrintCorruptedMessage;

            /// Message correctly decoded
            msgDecoder.OnMessageDecodedEvent += ConsolePrint.OnPrintDecodedMessage;

            /// Message encoded
            msgEncoder.OnMessageEncodedEvent += ConsolePrint.OnPrintEncodedMessage;
        }


        /******************************************* Trap app termination ***************************************/
        #region Gestion Arret Console (Do not Modify)
        static bool exitSystem = false;
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 3,
            CTRL_SHUTDOWN_EVENT = 4
        }

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;
        //Gestion de la terminaison de l'application de manière propre
        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Existing on CTRL+C or process kill or shutdown...");

            //Nettoyage des process à faire ici
            //serialPort1.Close();

            Console.WriteLine("Nettoyage effectué");
            exitSystem = true;

            //Sortie
            Environment.Exit(-1);
            return true;
        }
        #endregion
    }
}