using System;
using System.IO.Ports;
using System.Management;
using EventArgsLibrary;
using ExtendedSerialPort;

namespace SerialAutoConnection
{
    public class SerialAutoConnect
    {
        #region Attributes
        public static ReliableSerialPort serialPort;
        #endregion

        #region Constructor 
        public SerialAutoConnect()
        {
            OnSerialAutoConnectionCreated();
        }
        public event EventHandler<EventArgs> OnSerialAutoConnectionCreatedEvent;
        public virtual void OnSerialAutoConnectionCreated() => OnSerialAutoConnectionCreatedEvent?.Invoke(this, new EventArgs());
        #endregion

        #region Main Method
        public bool AutoConnectionSerial(uint timestamp = 1000, uint trial_max = 255)
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
                    SerialAvailable(AvailableCOM);
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

        #region Sub Method
        private string GetSerialPort()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                OnListOfSerialAvailable();
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
                                OnCorrectSerialAvailable(COM_Name);
                                ConnectToSerialAvailable(COM_Name);
                            }
                            else
                            {
                                OnWrongSerialAvailable(COM_Name);
                            }
                        }
                    }
                }
                return (AvailableCOM);
            }
            catch (ManagementException)
            {
                OnErrorWhileAttemptingConnection();
                return "";
            }
        }

        public void ConnectToSerialAvailable(string COM)
        {
            serialPort = new ReliableSerialPort(COM, 115200, Parity.None, 8, StopBits.One);
            serialPort.OnDataReceivedEvent += SerialPort_OnDataReceived;
            serialPort.Open();
            OnSerialConnected(COM);
        }

        public event EventHandler<DataReceivedArgs> OnDataReceivedEvent;
        private void SerialPort_OnDataReceived(object sender, DataReceivedArgs e)
        {
            byte[] dataReceived = new byte[e.Data.Length];
            foreach (byte b in e.Data)
                dataReceived[b] = e.Data[b];
            OnDataReceivedEvent?.Invoke(this, new DataReceivedArgs { Data = dataReceived });
        }


        public void SendMessageToRobot(object sender, MessageEncodedArgs e)
        {
            serialPort.SendMessage(this, e);
        }

        public event EventHandler<StringEventArgs> OnSerialAvailableEvent;
        public void SerialAvailable(string COM) => OnSerialAvailableEvent?.Invoke(this, new StringEventArgs { Value = COM });

        public event EventHandler<StringEventArgs> OnSerialConnectedEvent;
        public virtual void OnSerialConnected(string COM) => OnSerialConnectedEvent?.Invoke(this, new StringEventArgs { Value = COM });
        #endregion

        #region Event
        public event EventHandler<EventArgs> OnAutoConnectionLaunchedEvent;
        public virtual void OnAutoConnectionLaunched() => OnAutoConnectionLaunchedEvent?.Invoke(this, new EventArgs());

        public event EventHandler<ByteEventArgs> OnNewSerialAttemptEvent;
        public virtual void OnNewSerialAttempt(byte attempt) => OnNewSerialAttemptEvent?.Invoke(this, new ByteEventArgs { Value = attempt });

        public event EventHandler<EventArgs> OnListOfSerialAvailableEvent;
        public virtual void OnListOfSerialAvailable() => OnListOfSerialAvailableEvent?.Invoke(this, new EventArgs());

        public event EventHandler<StringEventArgs> OnWrongSerialAvailableEvent;
        public virtual void OnWrongSerialAvailable(string COM) => OnWrongSerialAvailableEvent?.Invoke(this, new StringEventArgs { Value = COM });

        public event EventHandler<StringEventArgs> OnCorrectSerialAvailableEvent;
        public virtual void OnCorrectSerialAvailable(string COM) => OnCorrectSerialAvailableEvent?.Invoke(this, new StringEventArgs { Value = COM });

        public event EventHandler<EventArgs> OnNoConnectionAvailablEvent;
        public virtual void OnNoConnectionAvailable() => OnNoConnectionAvailablEvent?.Invoke(this, new EventArgs());

        public event EventHandler<EventArgs> OnErrorWhileWhileAttemptingConnectionEvent;
        public virtual void OnErrorWhileAttemptingConnection() => OnErrorWhileWhileAttemptingConnectionEvent?.Invoke(this, new EventArgs());
        #endregion

    }
}