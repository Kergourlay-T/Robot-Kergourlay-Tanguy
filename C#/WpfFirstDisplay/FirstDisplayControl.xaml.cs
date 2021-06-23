using Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using System.Windows.Forms;
using Utilities;
using EventArgsLibrary;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;


namespace WpfFirstDisplay
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    /// 
    public partial class FirstDisplayControl : Window
    {
        #region Attributes
        int queueSize = 1;

        FixedSizedQueue<double> posXList;
        FixedSizedQueue<double> posYList;
        FixedSizedQueue<double> angleRadianList;

        FixedSizedQueue<double> commandXList;
        FixedSizedQueue<double> commandThetaList;
        FixedSizedQueue<double> commandM1List;
        FixedSizedQueue<double> commandM2List;

        FixedSizedQueue<double> consigneXList;
        FixedSizedQueue<double> consigneThetaList;
        FixedSizedQueue<double> consigneM1List;
        FixedSizedQueue<double> consigneM2List;

        FixedSizedQueue<double> measuredXList;
        FixedSizedQueue<double> measuredThetaList;
        FixedSizedQueue<double> measuredM1List;
        FixedSizedQueue<double> measuredM2List;

        FixedSizedQueue<double> errorXList;
        FixedSizedQueue<double> errorThetaList;
        FixedSizedQueue<double> errorM1List;
        FixedSizedQueue<double> errorM2List;

        FixedSizedQueue<double> corrPXList;
        FixedSizedQueue<double> corrPThetaList;
        FixedSizedQueue<double> corrPM1List;
        FixedSizedQueue<double> corrPM2List;

        FixedSizedQueue<double> corrIXList;
        FixedSizedQueue<double> corrIThetaList;
        FixedSizedQueue<double> corrIM1List;
        FixedSizedQueue<double> corrIM2List;

        FixedSizedQueue<double> corrDXList;
        FixedSizedQueue<double> corrDThetaList;
        FixedSizedQueue<double> corrDM1List;
        FixedSizedQueue<double> corrDM2List;

        double corrLimitPX, corrLimitPTheta, corrLimitPM1, corrLimitPM2;
        double corrLimitIX, corrLimitITheta, corrLimitIM1, corrLimitIM2;
        double corrLimitDX, corrLimitDTheta, corrLimitDM1, corrLimitDM2;

        double KpX, KpTheta, KpM1, KpM2;
        double KiX, KiTheta, KiM1, KiM2;
        double KdX, KdTheta, KdM1, KdM2;

        FixedSizedQueue<float> IRRigthEndList;
        FixedSizedQueue<float> IRRigthList;
        FixedSizedQueue<float> IRCenterList;
        FixedSizedQueue<float> IRLeftList;
        FixedSizedQueue<float> IRLeftEndList;
        FixedSizedQueue<float> timestampList;

        System.Timers.Timer displayTimer;

        bool currentLED1State;
        bool currentLED2State;
        bool currentLED3State;
        bool currentAutoControlState = true;

        private readonly KeyboardHookListener m_KeyboardHookManager;

        ActiveMode activeMode = ActiveMode.Disabled;
        #endregion

        public FirstDisplayControl()
        {
            InitializeComponent();
            SetTitle(activeMode.ToString());

            posXList = new Utilities.FixedSizedQueue<double>(queueSize);
            posYList = new Utilities.FixedSizedQueue<double>(queueSize);
            angleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);


            commandXList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            consigneXList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            measuredXList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            errorXList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            corrPXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            corrIXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            corrDXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM2List = new Utilities.FixedSizedQueue<double>(queueSize);

            IRRigthEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRRigthList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRCenterList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            timestampList = new Utilities.FixedSizedQueue<float>(queueSize);


            posXList.Enqueue(0);
            posYList.Enqueue(0);
            angleRadianList.Enqueue(0);

            consigneXList.Enqueue(0);
            consigneThetaList.Enqueue(0);
            consigneM1List.Enqueue(0);
            consigneM2List.Enqueue(0);

            commandXList.Enqueue(0);
            commandThetaList.Enqueue(0);
            commandM1List.Enqueue(0);
            commandM2List.Enqueue(0);

            measuredXList.Enqueue(0);
            measuredThetaList.Enqueue(0);
            measuredM1List.Enqueue(0);
            measuredM2List.Enqueue(0);

            errorXList.Enqueue(0);
            errorThetaList.Enqueue(0);
            errorM1List.Enqueue(0);
            errorM2List.Enqueue(0);

            IRRigthEndList.Enqueue(0);
            IRRigthList.Enqueue(0);
            IRCenterList.Enqueue(0);
            IRLeftList.Enqueue(0);
            IRLeftEndList.Enqueue(0);
            timestampList.Enqueue(0);


            displayTimer = new System.Timers.Timer(100);
            displayTimer.Elapsed += DisplayTimer_Elapsed;
            displayTimer.Start();

            m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyDown += KeyboardHookManager_KeyDown;
        }

        public void SetTitle(string titre)
        {
            LabelTitre.Content += titre;
        }

        public void SetAsservissementMode(ActiveMode mode)
        {
            activeMode = mode;

            switch (activeMode)
            {
                case ActiveMode.Disabled:
                    LabelConsigneX.Visibility = Visibility.Hidden;
                    LabelConsigneTheta.Visibility = Visibility.Hidden;
                    LabelErreurX.Visibility = Visibility.Hidden;
                    LabelErreurTheta.Visibility = Visibility.Hidden;
                    LabelCommandX.Visibility = Visibility.Hidden;
                    LabelCommandTheta.Visibility = Visibility.Hidden;

                    LabelConsigneM1.Visibility = Visibility.Hidden;
                    LabelConsigneM2.Visibility = Visibility.Hidden;
                    LabelErreurM1.Visibility = Visibility.Hidden;
                    LabelErreurM2.Visibility = Visibility.Hidden;
                    LabelCommandM1.Visibility = Visibility.Hidden;
                    LabelCommandM2.Visibility = Visibility.Hidden;

                    LabelCorrPX.Visibility = Visibility.Hidden;
                    LabelCorrPTheta.Visibility = Visibility.Hidden;
                    LabelCorrIX.Visibility = Visibility.Hidden;
                    LabelCorrITheta.Visibility = Visibility.Hidden;
                    LabelCorrDX.Visibility = Visibility.Hidden;
                    LabelCorrDTheta.Visibility = Visibility.Hidden;

                    LabelCorrPM1.Visibility = Visibility.Hidden;
                    LabelCorrPM2.Visibility = Visibility.Hidden;
                    LabelCorrIM1.Visibility = Visibility.Hidden;
                    LabelCorrIM2.Visibility = Visibility.Hidden;
                    LabelCorrDM1.Visibility = Visibility.Hidden;
                    LabelCorrDM2.Visibility = Visibility.Hidden;

                    LabelInformationReceived.Visibility = Visibility.Visible;
                    LabelInformationSent.Visibility = Visibility.Visible;
                    LabelIRLeftEnd.Visibility = Visibility.Visible;
                    LabelIRLeft.Visibility = Visibility.Visible;
                    LabelIRCenter.Visibility = Visibility.Visible;
                    LabelIRRigth.Visibility = Visibility.Visible;
                    LabelIRRigthEnd.Visibility = Visibility.Visible;
                    LabelTimestamp.Visibility = Visibility.Visible;
                    LabelRobotState.Visibility = Visibility.Visible;

                    break;
                case ActiveMode.Polar:
                    LabelConsigneX.Visibility = Visibility.Visible;
                    LabelConsigneTheta.Visibility = Visibility.Visible;
                    LabelErreurX.Visibility = Visibility.Visible;
                    LabelErreurTheta.Visibility = Visibility.Visible;
                    LabelCommandX.Visibility = Visibility.Visible;
                    LabelCommandTheta.Visibility = Visibility.Visible;

                    LabelConsigneM1.Visibility = Visibility.Hidden;
                    LabelConsigneM2.Visibility = Visibility.Hidden;
                    LabelErreurM1.Visibility = Visibility.Hidden;
                    LabelErreurM2.Visibility = Visibility.Hidden;
                    LabelCommandM1.Visibility = Visibility.Hidden;
                    LabelCommandM2.Visibility = Visibility.Hidden;

                    LabelCorrPX.Visibility = Visibility.Visible;
                    LabelCorrPTheta.Visibility = Visibility.Visible;
                    LabelCorrIX.Visibility = Visibility.Visible;
                    LabelCorrITheta.Visibility = Visibility.Visible;
                    LabelCorrDX.Visibility = Visibility.Visible;
                    LabelCorrDTheta.Visibility = Visibility.Visible;

                    LabelCorrPM1.Visibility = Visibility.Hidden;
                    LabelCorrPM2.Visibility = Visibility.Hidden;
                    LabelCorrIM1.Visibility = Visibility.Hidden;
                    LabelCorrIM2.Visibility = Visibility.Hidden;
                    LabelCorrDM1.Visibility = Visibility.Hidden;
                    LabelCorrDM2.Visibility = Visibility.Hidden;

                    LabelInformationReceived.Visibility = Visibility.Visible;
                    LabelInformationSent.Visibility = Visibility.Visible;
                    LabelIRLeftEnd.Visibility = Visibility.Visible;
                    LabelIRLeft.Visibility = Visibility.Visible;
                    LabelIRCenter.Visibility = Visibility.Visible;
                    LabelIRRigth.Visibility = Visibility.Visible;
                    LabelIRRigthEnd.Visibility = Visibility.Visible;
                    LabelTimestamp.Visibility = Visibility.Visible;
                    LabelRobotState.Visibility = Visibility.Visible;
                    break;
                case ActiveMode.Independant:
                    LabelConsigneX.Visibility = Visibility.Hidden;
                    LabelConsigneTheta.Visibility = Visibility.Hidden;
                    LabelErreurX.Visibility = Visibility.Hidden;
                    LabelErreurTheta.Visibility = Visibility.Hidden;
                    LabelCommandX.Visibility = Visibility.Hidden;
                    LabelCommandTheta.Visibility = Visibility.Hidden;

                    LabelConsigneM1.Visibility = Visibility.Visible;
                    LabelConsigneM2.Visibility = Visibility.Visible;
                    LabelErreurM1.Visibility = Visibility.Visible;
                    LabelErreurM2.Visibility = Visibility.Visible;
                    LabelCommandM1.Visibility = Visibility.Visible;
                    LabelCommandM2.Visibility = Visibility.Visible;

                    LabelCorrPX.Visibility = Visibility.Hidden;
                    LabelCorrPTheta.Visibility = Visibility.Hidden;
                    LabelCorrIX.Visibility = Visibility.Hidden;
                    LabelCorrITheta.Visibility = Visibility.Hidden;
                    LabelCorrDX.Visibility = Visibility.Hidden;
                    LabelCorrDTheta.Visibility = Visibility.Hidden;

                    LabelCorrPM1.Visibility = Visibility.Visible;
                    LabelCorrPM2.Visibility = Visibility.Visible;
                    LabelCorrIM1.Visibility = Visibility.Visible;
                    LabelCorrIM2.Visibility = Visibility.Visible;
                    LabelCorrDM1.Visibility = Visibility.Visible;
                    LabelCorrDM2.Visibility = Visibility.Visible;

                    LabelInformationReceived.Visibility = Visibility.Visible;
                    LabelInformationSent.Visibility = Visibility.Visible;
                    LabelIRLeftEnd.Visibility = Visibility.Visible;
                    LabelIRLeft.Visibility = Visibility.Visible;
                    LabelIRCenter.Visibility = Visibility.Visible;
                    LabelIRRigth.Visibility = Visibility.Visible;
                    LabelIRRigthEnd.Visibility = Visibility.Visible;
                    LabelTimestamp.Visibility = Visibility.Visible;
                    LabelRobotState.Visibility = Visibility.Visible;
                    break;
            }
        }

        #region UpdateDisplay
        private void DisplayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                UpdateDisplay();
            }));
        }

        public void UpdateDisplay()
        {
            LabelConsigneX.Content = consigneXList.Average().ToString("N2");
            LabelConsigneTheta.Content = consigneThetaList.Average().ToString("N2");
            LabelConsigneM1.Content = consigneM1List.Average().ToString("N2");
            LabelConsigneM2.Content = consigneM2List.Average().ToString("N2");

            LabelMeasureX.Content = measuredXList.Average().ToString("N2");
            LabelMeasureTheta.Content = measuredThetaList.Average().ToString("N2");
            LabelMeasureM1.Content = measuredM1List.Average().ToString("N2");
            LabelMeasureM2.Content = measuredM2List.Average().ToString("N2");

            LabelErreurX.Content = errorXList.Average().ToString("N2");
            LabelErreurTheta.Content = errorThetaList.Average().ToString("N2");
            LabelErreurM1.Content = errorM1List.Average().ToString("N2");
            LabelErreurM2.Content = errorM2List.Average().ToString("N2");

            LabelCommandX.Content = commandXList.Average().ToString("N2");
            LabelCommandTheta.Content = commandThetaList.Average().ToString("N2");
            LabelCommandM1.Content = commandM1List.Average().ToString("N2");
            LabelCommandM2.Content = commandM2List.Average().ToString("N2");

            LabelKpX.Content = KpX.ToString("N2");
            LabelKpTheta.Content = KpTheta.ToString("N2");
            LabelKpM1.Content = KpM1.ToString("N2");
            LabelKpM2.Content = KpM2.ToString("N2");

            LabelKiX.Content = KiX.ToString("N2");
            LabelKiTheta.Content = KiTheta.ToString("N2");
            LabelKiM1.Content = KiM1.ToString("N2");
            LabelKiM2.Content = KiM2.ToString("N2");

            LabelKdX.Content = KdX.ToString("N2");
            LabelKdTheta.Content = KdTheta.ToString("N2");
            LabelKdM1.Content = KdM1.ToString("N2");
            LabelKdM2.Content = KdM2.ToString("N2");

            LabelCorrMaxPX.Content = corrLimitPX.ToString("N2");
            LabelCorrMaxPTheta.Content = corrLimitPTheta.ToString("N2");
            LabelCorrMaxPM1.Content = corrLimitPM1.ToString("N2");
            LabelCorrMaxPM2.Content = corrLimitPM2.ToString("N2");

            LabelCorrMaxIX.Content = corrLimitIX.ToString("N2");
            LabelCorrMaxITheta.Content = corrLimitITheta.ToString("N2");
            LabelCorrMaxIM1.Content = corrLimitIM1.ToString("N2");
            LabelCorrMaxIM2.Content = corrLimitIM2.ToString("N2");

            LabelCorrMaxDX.Content = corrLimitDX.ToString("N2");
            LabelCorrMaxDTheta.Content = corrLimitDTheta.ToString("N2");
            LabelCorrMaxDM1.Content = corrLimitDM1.ToString("N2");
            LabelCorrMaxDM2.Content = corrLimitDM2.ToString("N2");

            if (posXList.Count > 0)
            {
                LabelPosX.Content = posXList.Average().ToString("N2");
                LabelPosY.Content = posYList.Average().ToString("N2");
                LabelAngleRadian.Content = angleRadianList.Average().ToString("N2");
            }

            if (corrPXList.Count > 0)
            {
                LabelCorrPX.Content = corrPXList.Average().ToString("N2");
                LabelCorrPTheta.Content = corrPThetaList.Average().ToString("N2");

                LabelCorrIX.Content = corrIXList.Average().ToString("N2");
                LabelCorrITheta.Content = corrIThetaList.Average().ToString("N2");

                LabelCorrDX.Content = corrDXList.Average().ToString("N2");
                LabelCorrDTheta.Content = corrDThetaList.Average().ToString("N2");
            }

            if (corrPM1List.Count > 0)
            {
                LabelCorrPM1.Content = corrPM1List.Average().ToString("N2");
                LabelCorrPM2.Content = corrPM2List.Average().ToString("N2");

                LabelCorrIM1.Content = corrIM1List.Average().ToString("N2");
                LabelCorrIM2.Content = corrIM2List.Average().ToString("N2");

                LabelCorrDM1.Content = corrDM1List.Average().ToString("N2");
                LabelCorrDM2.Content = corrDM2List.Average().ToString("N2");
            }

            if (IRRigthEndList.Count > 0)
            {
                LabelIRRigthEnd.Content = IRRigthEndList.Average().ToString("N2");
                LabelIRRigth.Content = IRRigthList.Average().ToString("N2");
                LabelIRCenter.Content = IRCenterList.Average().ToString("N2");
                LabelIRLeft.Content = IRLeftList.Average().ToString("N2");
                LabelIRLeftEnd.Content = IRLeftEndList.Average().ToString("N2");
            }

            if (timestampList.Count > 0)
                LabelTimestamp.Content = timestampList.Average().ToString("N2");

        }
        #endregion

        #region UpdateValues

        public void UpdatePositionFromOdometry(object sender, PositionMessageArgs e)
        {
            posXList.Enqueue(e.xPos_a);
            posYList.Enqueue(e.yPos_a);
            angleRadianList.Enqueue(e.theta_a);
        }

        #region PolarSpeed

        public void UpdatePolarSpeedOdometryValues(object sender, PolarSpeedOdometryData e)
        {
            consigneXList.Enqueue(e.xConsigne_a);
            consigneThetaList.Enqueue(e.thetaConsigne_a);
            commandXList.Enqueue(e.xCommand_a);
            commandThetaList.Enqueue(e.thetaCommand_a);
            measuredXList.Enqueue(e.xMeasured_a);
            measuredThetaList.Enqueue(e.thetaMeasured_a);
            errorXList.Enqueue(e.xError_a);
            errorThetaList.Enqueue(e.thetaError_a);
        }

        public void UpdatePolarSpeedCorrectionValues(object sender, PolarSpeedPidData e)
        {
            corrPXList.Enqueue(e.Px_a);
            corrPThetaList.Enqueue(e.Ptheta_a);
            corrIXList.Enqueue(e.Ix_a);
            corrIThetaList.Enqueue(e.Itheta_a);
            corrDXList.Enqueue(e.Dx_a);
            corrDThetaList.Enqueue(e.Dtheta_a);
        }

        public void UpdatePolarSpeedCorrectionGains(object sender, PolarSpeedPidData e)
        {
            KpX = e.Px_a;
            KpTheta = e.Ptheta_a;
            KiX = e.Ix_a;
            KiTheta = e.Itheta_a;
            KdX = e.Dx_a;
            KdTheta = e.Dtheta_a;
        }

        public void UpdatePolarSpeedCorrectionLimits(object sender, PolarSpeedPidData e)
        {
            corrLimitPX = e.Px_a;
            corrLimitPTheta = e.Ptheta_a;
            corrLimitIX = e.Ix_a;
            corrLimitITheta = e.Itheta_a;
            corrLimitDX = e.Dx_a;
            corrLimitDTheta = e.Dtheta_a;
        }
        #endregion


        #region IndependantSpeed
        public void UpdateIndependantSpeedConsigneValues(object receiver, MotorMessageArgs e)
        {
            consigneM1List.Enqueue(e.leftMotor_a);
            consigneM2List.Enqueue(e.rightMotor_a);
        }


        public void UpdateIndependantSpeedCommandValues(object receiver, MotorMessageArgs e)
        {
            commandM1List.Enqueue(e.leftMotor_a);
            commandM2List.Enqueue(e.rightMotor_a);
        }

        public void UpdateIndependantOdometrySpeed(object sender, MotorMessageArgs e)
        {
            measuredM1List.Enqueue(e.leftMotor_a);
            measuredM2List.Enqueue(e.rightMotor_a);
        }


        public void UpdateIndependantSpeedErrorValues(object sender, MotorMessageArgs e)
        {
            errorM1List.Enqueue(e.leftMotor_a);
            errorM2List.Enqueue(e.rightMotor_a);
        }


        #region Not used
        public void UpdateIndependantSpeedCorrectionValues(double corrPM1, double corrPM2, double corrIM1,
            double corrIM2, double corrDM1, double corrDM2)
        {
            corrPM1List.Enqueue(corrPM1);
            corrPM2List.Enqueue(corrPM2);
            corrIM1List.Enqueue(corrIM1);
            corrIM2List.Enqueue(corrIM2);
            corrDM1List.Enqueue(corrDM1);
            corrDM2List.Enqueue(corrDM2);
        }

        public void UpdateIndependantSpeedCorrectionGains(double KpM1, double KpM2, double KiM1,
            double KiM2, double KdM1, double KdM2)
        {
            this.KpM1 = KpM1;
            this.KpM2 = KpM2;
            this.KiM1 = KiM1;
            this.KiM2 = KiM2;
            this.KdM1 = KdM1;
            this.KdM2 = KdM2;
        }

        public void UpdateIndependantSpeedCorrectionLimits(double corrLimitPM1, double corrLimitPM2,
            double corrLimitIM1, double corrLimitIM2, double corrLimitDM1, double corrLimitDM2)
        {
            this.corrLimitPM1 = corrLimitPM1;
            this.corrLimitPM2 = corrLimitPM2;
            this.corrLimitIM1 = corrLimitIM1;
            this.corrLimitIM2 = corrLimitIM2;
            this.corrLimitDM1 = corrLimitDM1;
            this.corrLimitDM2 = corrLimitDM2;
        }
        #endregion
        #endregion

        #endregion

        #region Update Robot Informations
        public void UpdateCheckInstruction(object receiver, MessageDecodedArgs e)
        {
            LabelInformationReceived.Content = (((Commands)e.MsgFunction).ToString("N2"));
        }

        public void UpdateTelematersValues(object receiver, IRMessageArgs e)
        {
            IRRigthEndList.Enqueue(e.rigthEndIR_a);
            IRRigthList.Enqueue(e.rigthIR_a);
            IRCenterList.Enqueue(e.centerIR_a);
            IRLeftList.Enqueue(e.leftIR_a);
            IRLeftEndList.Enqueue(e.leftEndIR_a);
        }

        public void UpdateLEDState(object receiver, LEDMessageArgs e)
        {
            if (e.nbLed_a == 1)
            {
                CheckBoxLED1.IsChecked = e.stateLed_a;
                currentLED1State = e.stateLed_a;
            }
            else if (e.nbLed_a == 2)
            {
                CheckBoxLED2.IsChecked = e.stateLed_a;
                currentLED2State = e.stateLed_a;
            }
            else if (e.nbLed_a == 3)
            {
                CheckBoxLED3.IsChecked = e.stateLed_a;
                currentLED3State = e.stateLed_a;
            }
        }
        public void UpdateRobotStateAndTimestamp(object receiver, StateMessageArgs e)
        {
            LabelRobotState.Content = (((Commands)e.state_a).ToString("N2"));
            timestampList.Enqueue(e.time_a);
        }

        public void UpdateManualControl(object receiver, BoolEventArgs e)
        {
            CheckBoxAutoControl.IsChecked = e.Value;
        }
        public void UpdateTextBoxReception(object receiver, TextMessageArgs e)
        {
            TextBoxReception.Text += e.text_a;
        }
        #endregion

        #region Set Robot Values
        private void OnTextBoxEmissionKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string message = TextBoxEmission.Text;
                OnSentTextMessageFromInterfaceGenerate(message);
                TextBoxEmission.Text = "";
            }
        }
        private void OnCheckBoxLED1CheckChange(object sender, EventArgs e)
        {
            if (CheckBoxLED1.IsChecked != currentLED1State)
                OnSetLEDStateFromInterfaceGenerate(1, !currentLED1State);
        }
        private void OnCheckBoxLED2CheckChange(object sender, EventArgs e)
        {
            if (CheckBoxLED2.IsChecked != currentLED2State)
                OnSetLEDStateFromInterfaceGenerate(2, !currentLED2State);
        }
        private void OnCheckBoxLED3CheckChange(object sender, EventArgs e)
        {
            if (CheckBoxLED3.IsChecked != currentLED3State)
                OnSetLEDStateFromInterfaceGenerate(3, !currentLED3State);
        }
        private void OnCheckBoxAutoControlCheckChange(object sender, EventArgs e)
        {
            if (CheckBoxAutoControl.IsChecked != currentAutoControlState)
                OnSetAutoControlStateFromInterfaceGenerate(!currentAutoControlState);
        }

        private void OnButtonResetClick(object sender, RoutedEventArgs e)
        {
            OnResetPositionFromInterfaceGenerate(true);
        }

        private void OnButtonClearClick(object sender, RoutedEventArgs e)
        {
            TextBoxReception.Text = "";
        }

        private void KeyboardHookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (currentAutoControlState == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        OnSetRobotStateFromInterfaceGenerate((ushort)stateRobot.STATE_TOURNE_SUR_PLACE_GAUCHE);
                        break;

                    case Keys.Right:
                        OnSetRobotStateFromInterfaceGenerate((ushort)stateRobot.STATE_TOURNE_SUR_PLACE_DROITE);
                        break;

                    case Keys.Up:
                        OnSetRobotStateFromInterfaceGenerate((ushort)stateRobot.STATE_AVANCE);
                        break;

                    case Keys.Down:
                        OnSetRobotStateFromInterfaceGenerate((ushort)stateRobot.STATE_RECULE);
                        break;

                    case Keys.PageDown:
                        OnSetRobotStateFromInterfaceGenerate((ushort)stateRobot.STATE_ARRET);
                        break;
                }
            }
        }
        #endregion

        #region Output Events
        public event EventHandler<LEDMessageArgs> OnSetLEDStateFromInterfaceGenerateEvent;
        public void OnSetLEDStateFromInterfaceGenerate(ushort nbLED, bool stateLed)
            => OnSetLEDStateFromInterfaceGenerateEvent?.Invoke(this, new LEDMessageArgs
            {
                nbLed_a = nbLED,
                stateLed_a = stateLed
            });


        public EventHandler<BoolEventArgs> OnSetAutoControlStateFromInterfaceGenerateEvent;
        public void OnSetAutoControlStateFromInterfaceGenerate(bool autoControlSet)
            => OnSetAutoControlStateFromInterfaceGenerateEvent?.Invoke(this, new BoolEventArgs { Value = autoControlSet });

        public event EventHandler<MotorMessageArgs> OnSetMotorSpeedFromInterfaceGenerateEvent;
        public void OnSetMotorSpeedFromInterfaceGenerate(sbyte leftMotor, sbyte rigthMotor)
          => OnSetMotorSpeedFromInterfaceGenerateEvent?.Invoke(this, new MotorMessageArgs
          {
              leftMotor_a = leftMotor,
              rightMotor_a = rigthMotor
          });

        public event EventHandler<UshortEventArgs> OnSetRobotStateFromInterfaceGenerateEvent;
        public void OnSetRobotStateFromInterfaceGenerate(ushort robotState)
        {
            LabelInformationSent.Content = (((Commands)robotState).ToString("N2"));
            OnSetRobotStateFromInterfaceGenerateEvent?.Invoke(this, new UshortEventArgs { Value = robotState });
        }

        public event EventHandler<SetPositionMessageArgs> OnSetPositionFromInterfaceGenrateEvent;
        public void OnSetPositionFromInterfaceGenrate(float xPos, float yPos, float angleRadian)
            => OnSetPositionFromInterfaceGenrateEvent?.Invoke(this, new SetPositionMessageArgs
            {
                xPos_a = xPos,
                yPos_a = yPos,
                angleRadian_a = angleRadian
            });

        public event EventHandler<BoolEventArgs> OnResetPositionFromInterfaceGenerateEvent;
        public void OnResetPositionFromInterfaceGenerate(bool isResetPos)
            => OnResetPositionFromInterfaceGenerateEvent?.Invoke(this, new BoolEventArgs { Value = isResetPos });

        public event EventHandler<TextMessageArgs> OnSentTextMessageFromInterfaceGenerateEvent;
        public void OnSentTextMessageFromInterfaceGenerate(string content)
           => OnSentTextMessageFromInterfaceGenerateEvent?.Invoke(this, new TextMessageArgs { text_a = content });
        #endregion

        #region Test
        private void OnButtonTestClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion 
    }
}