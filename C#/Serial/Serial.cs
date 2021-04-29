using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;
using ExtendedSerialPort;

namespace Serial
{
    public class Serial
    {
        #region Attributes
        public static ReliableSerialPort serialPort;

        public static MessageDecoder.MsgDecoder msgDecoder;
        public static MessageEncoder.MsgEncoder msgENcoder;
        public static MessageGenerator.MsgGenerator msgGenerator;
        public static MessageProcessor.MsgProcessor msgProcessor; 
        #endregion

        #region Constructor 
        public Serial()
        {
            MessageDecoder.MsgDecoder msgDecoder = new MessageDecoder.MsgDecoder();
            MessageEncoder.MsgEncoder msgENcoder = new MessageEncoder.MsgEncoder();
            MessageGenerator.MsgGenerator msgGenerator = new MessageGenerator.MsgGenerator();
            MessageProcessor.MsgProcessor msgProcessor = new MessageProcessor.MsgProcessor();
        }
        #endregion

        #region Method
        public bool AutoConnectionSerial (uint timestamp, uint trial_max = 255)
        {
            OnAutoConnectionLaunched();
            byte i = 0;
            do
            {
                i++;
                OnNewSerialAttempt(i);
                string AvailableCOM = GetSerialPort();

                if (AvailableCOM != "")
                {
                    OnSerialAvailable(COM);
                }
                else
                {
                    OnNoConnectionAvailable();
                }
                

            } while ( serialPort == null && i < trial_max);
            return (serialPort != null);
        }

        #endregion

        private string GetSerialPort()
        {
            try
            {

            }
        }


        #region Event
        public event EventHandler<COMEventArgs> OnSerialConnectedEvent;
        public event EventHandler<EventArgs> OnAutoConnectionLaunchedEvent;
        public event EventHandler<AttemptsEventArgs> OnNewSerialAttemptEvent;
        public event EventHandler<EventArgs> OnSerialAvailableListEvent;
        public event EventHandler<COMEventArgs> OnWrongCOMAvailableEvent;
        public event EventHandler<COMEventArgs> OnCorrectCOMAvailableEvent;
        public event EventHandler<COMEventArgs> OnSerialAvailableEvent;
        public event EventHandler<EventArgs> OnNoConnectionAvailablEvent;
        public event EventHandler<EventArgs> OnErrorWhileWhileAttemptingCOMEvent;

        public virtual void OnSerialAutoConnected(string COM)
        {
            serialPort = new ReliableSerialPort(COM, 115200, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
            OnSerialConnectedEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public void SerialPort_DataReceived(object sender, DataReceivedArgs e)
        {
            for (int i = 0; i < e.Data.Length; i++)
            {
                msgDecoder.ByteReceived(e.Data[i]);
            }
        }

        public virtual void OnAutoConnectionLaunched()
        {
            OnAutoConnectionLaunchedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnNewSerialAttempt(byte attempt)
        {
            OnNewSerialAttemptEvent?.Invoke(this, new AttemptsEventArgs(attempt));
        }
        public virtual void OnSerialAvailableList(string COM)
        {
            OnSerialAvailableListEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnWrongCOMAvailable(string COM)
        {
            OnWrongCOMAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnCorrectCOMAvailable(string COM)
        {
            OnCorrectCOMAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnSerialAvailable(string COM)
        {
            OnSerialAutoConnected(COM);
            OnSerialAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnNoConnectionAvailable()
        {
            OnNoConnectionAvailablEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnErrorWhileWhileAttemptingCOM()
        {
            OnErrorWhileWhileAttemptingCOMEvent?.Invoke(this, new EventArgs());
        }
        #endregion

    }
}
