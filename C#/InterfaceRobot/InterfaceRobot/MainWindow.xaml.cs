using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO.Ports;
using System.Windows.Forms;
using Constants;
using ConsoleRobot;
using EventArgsLibrary;
using ExtendedSerialPort;
using WpfAsservissementDisplay;
using Utilities;
using MouseKeyboardActivityMonitor.WinApi;
using MouseKeyboardActivityMonitor;

namespace InterfaceRobot
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReliableSerialPort serialPort1;
        DispatcherTimer timerAffichage;
        Robot robot = new Robot();
        private readonly KeyboardHookListener m_KeyboardHookManager;

        int i;
        public MainWindow()
        {
            InitializeComponent();
            serialPort1 = new ReliableSerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            serialPort1.DataReceived += SerialPort1_DataReceived;
            serialPort1.Open();

            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick;
            timerAffichage.Start();

            m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyDown += M_KeyboardHookManager_KeyDown;
        }

        private void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            throw new NotImplementedException();
        }

        private void M_KeyboardHookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TimerAffichage_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        #region Events
        public event EventHandler<EventArgs> OnMessageFromConsoleCreatedEvent;
        public event EventHandler<LEDMessageArgs> OnLEDMessageFromConsoleReceivedEvent;
        public event EventHandler<IRMessageArgs> OnIRMessageFromConsoleReceivedEvent;
        public event EventHandler<MotorMessageArgs> OnMotorMessageFromConsoleReceivedEvent;
        public event EventHandler<StateMessageArgs> OnStateMessageFromConsoleReceivedEvent;
        public event EventHandler<PositionMessageArgs> OnPositionMessageFromConsoleReceivedEvent;
        public event EventHandler<TextMessageArgs> OnTextMessageFromConsoleReceivedEvent;
        public event EventHandler<MessageByteArgs> OnUnknowFunctionFromConsoleReceivedEvent;

        public virtual void OnMessageFromConsoleCreated()
        {
            OnMessageFromConsoleCreatedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnLEDMessageFromConsoleReceived(MessageByteArgs e)
        {
            ushort nbLed = Convert.ToUInt16(e.msgPayload[0]);
            bool stateLed = Convert.ToBoolean(e.msgPayload[1]);
            OnLEDMessageFromConsoleReceivedEvent?.Invoke(this, new LEDMessageArgs(nbLed, stateLed));
        }

        public virtual void OnIRMessageFromConsoleReceived(MessageByteArgs e)
        {
            OnIRMessageFromConsoleReceivedEvent?.Invoke(this, new IRMessageArgs(e.msgPayload[0], e.msgPayload[1], e.msgPayload[2]));
        }

        public virtual void OnMotorMessageFromConsoleReceived(MessageByteArgs e)
        {
            OnMotorMessageFromConsoleReceivedEvent?.Invoke(this, new MotorMessageArgs((sbyte)e.msgPayload[0], (sbyte)e.msgPayload[1]));
        }

        public virtual void OnStateMessageFromConsoleReceived(MessageByteArgs e)
        {
            uint time = ((uint)(e.msgPayload[1] << 24) + (uint)(e.msgPayload[2] << 16) + (uint)(e.msgPayload[3] << 8) + (uint)(e.msgPayload[4] << 0));
            OnStateMessageFromConsoleReceivedEvent?.Invoke(this, new StateMessageArgs((ushort)e.msgPayload[0], time));
        }

        public virtual void OnPositionMessageFromConsoleReceived(MessageByteArgs e)
        {
            uint time = BitConverter.ToUInt32(e.msgPayload, 0);
            float xPos = BitConverter.ToSingle(e.msgPayload, 4);
            float yPos = BitConverter.ToSingle(e.msgPayload, 8); ;
            float angleRadiant = BitConverter.ToSingle(e.msgPayload, 12); ;
            float linearSpeed = BitConverter.ToSingle(e.msgPayload, 16); ;
            float angularSpeed = BitConverter.ToSingle(e.msgPayload, 20); ;
            OnPositionMessageFromConsoleReceivedEvent?.Invoke(this, new PositionMessageArgs(time, xPos, yPos, angleRadiant, linearSpeed, angularSpeed));
        }

        public virtual void OnTextMessageFromConsoleReceived(MessageByteArgs e)
        {
            string text = Encoding.UTF8.GetString(e.msgPayload, 0, e.msgPayload.Length);
            OnTextMessageFromConsoleReceivedEvent?.Invoke(this, new TextMessageArgs(text));
        }

        public virtual void OnUnknowFunctionFromConsoleReceived(MessageByteArgs e)
        {
            OnUnknowFunctionFromConsoleReceivedEvent?.Invoke(this, e);
        }
        #endregion
    }
}
