using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;
using EventArgsLibrary;
using ExtendedSerialPort;
using MessageDecoder;

namespace Serial
{
    public class serial
    {
        #region Attributes
        public  static ReliableSerialPort serialPort;
        public  MsgDecoder msgDecoder;
        #endregion

        #region Constructor 
        public serial()
        {
            msgDecoder = new MsgDecoder();
        }
        #endregion

        #region Method
        public bool AutoConnectionSerial (uint timestamp = 1000, uint trial_max = 255)
        {
            OnAutoConnectionLaunched();
            byte i = 0;
            do
            {
                i++;
                OnNewCOMAttempt(i);
                string AvailableCOM = GetSerialPort();

                if (AvailableCOM != "")
                {
                    OnCOMAvailable(AvailableCOM);
                }
                else
                {
                    OnNoConnectionAvailable();
                }
                System.Threading.Thread.Sleep((int)timestamp); // Not Good 
            } while (serialPort == null && i < trial_max);
            return (serialPort != null);
        }

        #endregion

        private string GetSerialPort()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                OnCOMAvailableList();
                string AvailableCOM = "";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj != null && queryObj["Caption"] != null)
                    {
                        if (queryObj["Caption"].ToString().Contains("(COM"))
                        {
                            string queryString = queryObj["Caption"].ToString();
                            string[] queryStringArray = queryString.Split(' ');
                            string COM_Name = queryStringArray[queryStringArray.Length - 1]; // Get (COMx)
                            string COM_Description = queryString.Remove(queryString.IndexOf(COM_Name) - 1);
                            COM_Name = COM_Name.Remove(COM_Name.Length - 1).Remove(0, 1); // Remove ( ) From COM
                            if (COM_Description == "USB Serial Port")
                            {
                                AvailableCOM = COM_Name;
                                OnCorrectCOMAvailable(COM_Name);
                            }
                            else
                            {
                                OnWrongCOMAvailable(COM_Name);
                            }
                        }
                    }
                }
                return (AvailableCOM);
            }
            catch (ManagementException)
            {
                OnErrorWhileAttemptingCOM();
                return "";
            }
        }

        #region Event
        public event EventHandler<COMEventArgs> OnSerialConnectedEvent;
        public event EventHandler<EventArgs> OnAutoConnectionLaunchedEvent;
        public event EventHandler<AttemptsEventArgs> OnNewCOMAttemptEvent;
        public event EventHandler<EventArgs> OnCOMAvailableListEvent;
        public event EventHandler<COMEventArgs> OnWrongCOMAvailableEvent;
        public event EventHandler<COMEventArgs> OnCorrectCOMAvailableEvent;
        public event EventHandler<COMEventArgs> OnCOMAvailableEvent;
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
            foreach (byte b in e.Data)
            {
                msgDecoder.ByteReceived(b);
            }
        }

        public virtual void OnAutoConnectionLaunched()
        {
            OnAutoConnectionLaunchedEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnNewCOMAttempt(byte attempt)
        {
            OnNewCOMAttemptEvent?.Invoke(this, new AttemptsEventArgs(attempt));
        }
        public virtual void OnCOMAvailableList()
        {
            OnCOMAvailableListEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnWrongCOMAvailable(string COM)
        {
            OnWrongCOMAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnCorrectCOMAvailable(string COM)
        {
            OnCorrectCOMAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnCOMAvailable(string COM)
        {
            OnSerialAutoConnected(COM);
            OnCOMAvailableEvent?.Invoke(this, new COMEventArgs(COM));
        }
        public virtual void OnNoConnectionAvailable()
        {
            OnNoConnectionAvailablEvent?.Invoke(this, new EventArgs());
        }
        public virtual void OnErrorWhileAttemptingCOM()
        {
            OnErrorWhileWhileAttemptingCOMEvent?.Invoke(this, new EventArgs());
        }
        #endregion

    }//End class Serial
}//End Serial
