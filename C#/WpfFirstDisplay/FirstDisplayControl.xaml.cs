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
        FixedSizedQueue<double> commandXList;
        FixedSizedQueue<double> commandYList;
        FixedSizedQueue<double> commandThetaList;
        FixedSizedQueue<double> commandM1List;
        FixedSizedQueue<double> commandM2List;
        FixedSizedQueue<double> commandAngularSpeedList;
        FixedSizedQueue<double> commandLinearSpeedList;

        FixedSizedQueue<double> consigneXList;
        FixedSizedQueue<double> consigneYList;
        FixedSizedQueue<double> consigneThetaList;
        FixedSizedQueue<double> consigneM1List;
        FixedSizedQueue<double> consigneM2List;
        FixedSizedQueue<double> consigneAngularSpeedList;
        FixedSizedQueue<double> consigneLinearSpeedList;

        FixedSizedQueue<double> measuredXList;
        FixedSizedQueue<double> measuredYList;
        FixedSizedQueue<double> measuredThetaList;
        FixedSizedQueue<double> measuredM1List;
        FixedSizedQueue<double> measuredM2List;
        FixedSizedQueue<double> measuredAngularSpeedList;
        FixedSizedQueue<double> measuredLinearSpeedList;

        FixedSizedQueue<double> errorXList;
        FixedSizedQueue<double> errorYList;
        FixedSizedQueue<double> errorThetaList;
        FixedSizedQueue<double> errorM1List;
        FixedSizedQueue<double> errorM2List;
        FixedSizedQueue<double> errorAngularSpeedList;
        FixedSizedQueue<double> errorLinearSpeedList;

        FixedSizedQueue<double> corrPXList;
        FixedSizedQueue<double> corrPYList;
        FixedSizedQueue<double> corrPThetaList;
        FixedSizedQueue<double> corrPM1List;
        FixedSizedQueue<double> corrPM2List;
        FixedSizedQueue<double> corrPAngularSpeedList;
        FixedSizedQueue<double> corrPLinearSpeedList;

        FixedSizedQueue<double> corrIXList;
        FixedSizedQueue<double> corrIYList;
        FixedSizedQueue<double> corrIThetaList;
        FixedSizedQueue<double> corrIM1List;
        FixedSizedQueue<double> corrIM2List;
        FixedSizedQueue<double> corrIAngularSpeedList;
        FixedSizedQueue<double> corrILinearSpeedList;

        FixedSizedQueue<double> corrDXList;
        FixedSizedQueue<double> corrDYList;
        FixedSizedQueue<double> corrDThetaList;
        FixedSizedQueue<double> corrDM1List;
        FixedSizedQueue<double> corrDM2List;
        FixedSizedQueue<double> corrDAngularSpeedList;
        FixedSizedQueue<double> corrDLinearSpeedList;

        double corrLimitPX, corrLimitPY, corrLimitPTheta, corrLimitPM1, corrLimitPM2,
            corrLimitPAngularSpeed, corrLimitPLinearSpeed;
        double corrLimitIX, corrLimitIY, corrLimitITheta, corrLimitIM1, corrLimitIM2,
            corrLimitIAngularSpeed, corrLimitILinearSpeed;
        double corrLimitDX, corrLimitDY, corrLimitDTheta, corrLimitDM1, corrLimitDM2,
            corrLimitDAngularSpeed, corrLimitDLinearSpeed;

        double KpX, KpY, KpTheta, KpM1, KpM2, KpAngularSpeed, KpLinearSpeed;
        double KiX, KiY, KiTheta, KiM1, KiM2, KiAngularSpeed, KiLinearSpeed;
        double KdX, KdY, KdTheta, KdM1, KdM2, KdAngularSpeed, KdLinearSpeed;

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

        AsservissementMode asservissementMode = AsservissementMode.Disabled;
        #endregion

        public FirstDisplayControl()
        {
            InitializeComponent();
            SetTitle(asservissementMode.ToString());

            commandXList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandYList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            commandAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            consigneXList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneYList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            measuredXList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredYList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            errorXList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorYList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            errorAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrPXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrIXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrILinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrDXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            IRRigthEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRRigthList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRCenterList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            timestampList = new Utilities.FixedSizedQueue<float>(queueSize);

            consigneXList.Enqueue(0);
            consigneYList.Enqueue(0);
            consigneThetaList.Enqueue(0);
            consigneM1List.Enqueue(0);
            consigneM2List.Enqueue(0);
            consigneAngularSpeedList.Enqueue(0);
            consigneLinearSpeedList.Enqueue(0);

            commandXList.Enqueue(0);
            commandYList.Enqueue(0);
            commandThetaList.Enqueue(0);
            commandM1List.Enqueue(0);
            commandM2List.Enqueue(0);
            commandAngularSpeedList.Enqueue(0);
            commandLinearSpeedList.Enqueue(0);

            measuredXList.Enqueue(0);
            measuredYList.Enqueue(0);
            measuredThetaList.Enqueue(0);
            measuredM1List.Enqueue(0);
            measuredM2List.Enqueue(0);
            measuredAngularSpeedList.Enqueue(0);
            measuredLinearSpeedList.Enqueue(0);

            errorXList.Enqueue(0);
            errorYList.Enqueue(0);
            errorThetaList.Enqueue(0);
            errorM1List.Enqueue(0);
            errorM2List.Enqueue(0);
            errorAngularSpeedList.Enqueue(0);
            errorLinearSpeedList.Enqueue(0);

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

        public void SetAsservissementMode(AsservissementMode mode)
        {
            asservissementMode = mode;

            switch (asservissementMode)
            {
                case AsservissementMode.Disabled:
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
                    break;
                case AsservissementMode.Polar:
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
                    break;
                case AsservissementMode.Independant:
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
            LabelConsigneY.Content = consigneYList.Average().ToString("N2");
            LabelConsigneTheta.Content = consigneThetaList.Average().ToString("N2");
            LabelConsigneM1.Content = consigneM1List.Average().ToString("N2");
            LabelConsigneM2.Content = consigneM2List.Average().ToString("N2");
            LabelConsigneAngularSpeed.Content = consigneAngularSpeedList.Average().ToString("N2");
            LabelConsigneLinearSpeed.Content = consigneLinearSpeedList.Average().ToString("N2");

            LabelMeasureX.Content = measuredXList.Average().ToString("N2");
            LabelMeasureY.Content = measuredYList.Average().ToString("N2");
            LabelMeasureTheta.Content = measuredThetaList.Average().ToString("N2");
            LabelMeasureM1.Content = measuredM1List.Average().ToString("N2");
            LabelMeasureM2.Content = measuredM2List.Average().ToString("N2");
            LabelMeasureAngularSpeed.Content = measuredAngularSpeedList.Average().ToString("N2");
            LabelMeasureLinearSpeed.Content = measuredLinearSpeedList.Average().ToString("N2");

            LabelErreurX.Content = errorXList.Average().ToString("N2");
            LabelErreurY.Content = errorYList.Average().ToString("N2");
            LabelErreurTheta.Content = errorThetaList.Average().ToString("N2");
            LabelErreurM1.Content = errorM1List.Average().ToString("N2");
            LabelErreurM2.Content = errorM2List.Average().ToString("N2");
            LabelErreurAngularSpeed.Content = errorAngularSpeedList.Average().ToString("N2");
            LabelErreurLinearSpeed.Content = errorLinearSpeedList.Average().ToString("N2");

            LabelCommandX.Content = commandXList.Average().ToString("N2");
            LabelCommandY.Content = commandYList.Average().ToString("N2");
            LabelCommandTheta.Content = commandThetaList.Average().ToString("N2");
            LabelCommandM1.Content = commandM1List.Average().ToString("N2");
            LabelCommandM2.Content = commandM2List.Average().ToString("N2");
            LabelCommandAngularSpeed.Content = commandAngularSpeedList.Average().ToString("N2");
            LabelCommandLinearSpeed.Content = commandLinearSpeedList.Average().ToString("N2");

            LabelKpX.Content = KpX.ToString("N2");
            LabelKpY.Content = KpY.ToString("N2");
            LabelKpTheta.Content = KpTheta.ToString("N2");
            LabelKpM1.Content = KpM1.ToString("N2");
            LabelKpM2.Content = KpM2.ToString("N2");
            LabelKpAngularSpeed.Content = KpAngularSpeed.ToString("N2");
            LabelKpLinearSpeed.Content = KpLinearSpeed.ToString("N2");

            LabelKiX.Content = KiX.ToString("N2");
            LabelKiY.Content = KiY.ToString("N2");
            LabelKiTheta.Content = KiTheta.ToString("N2");
            LabelKiM1.Content = KiM1.ToString("N2");
            LabelKiM2.Content = KiM2.ToString("N2");
            LabelKiAngularSpeed.Content = KiAngularSpeed.ToString("N2");
            LabelKiLinearSpeed.Content = KiLinearSpeed.ToString("N2");

            LabelKdX.Content = KdX.ToString("N2");
            LabelKdY.Content = KdY.ToString("N2");
            LabelKdTheta.Content = KdTheta.ToString("N2");
            LabelKdM1.Content = KdM1.ToString("N2");
            LabelKdM2.Content = KdM2.ToString("N2");
            LabelKdAngularSpeed.Content = KdAngularSpeed.ToString("N2");
            LabelKdLinearSpeed.Content = KdLinearSpeed.ToString("N2");

            LabelCorrMaxPX.Content = corrLimitPX.ToString("N2");
            LabelCorrMaxPY.Content = corrLimitPY.ToString("N2");
            LabelCorrMaxPTheta.Content = corrLimitPTheta.ToString("N2");
            LabelCorrMaxPM1.Content = corrLimitPM1.ToString("N2");
            LabelCorrMaxPM2.Content = corrLimitPM2.ToString("N2");
            LabelCorrMaxPAngularSpeed.Content = corrLimitPAngularSpeed.ToString("N2");
            LabelCorrMaxPLinearSpeed.Content = corrLimitPLinearSpeed.ToString("N2");

            LabelCorrMaxIX.Content = corrLimitIX.ToString("N2");
            LabelCorrMaxIY.Content = corrLimitIY.ToString("N2");
            LabelCorrMaxITheta.Content = corrLimitITheta.ToString("N2");
            LabelCorrMaxIM1.Content = corrLimitIM1.ToString("N2");
            LabelCorrMaxIM2.Content = corrLimitIM2.ToString("N2");
            LabelCorrMaxIAngularSpeed.Content = corrLimitIAngularSpeed.ToString("N2");
            LabelCorrMaxILinearSpeed.Content = corrLimitILinearSpeed.ToString("N2");

            LabelCorrMaxDX.Content = corrLimitDX.ToString("N2");
            LabelCorrMaxDY.Content = corrLimitDY.ToString("N2");
            LabelCorrMaxDTheta.Content = corrLimitDTheta.ToString("N2");
            LabelCorrMaxDM1.Content = corrLimitDM1.ToString("N2");
            LabelCorrMaxDM2.Content = corrLimitDM2.ToString("N2");
            LabelCorrMaxDAngularSpeed.Content = corrLimitDAngularSpeed.ToString("N2");
            LabelCorrMaxDLinearSpeed.Content = corrLimitDLinearSpeed.ToString("N2");


            if (corrPXList.Count > 0)
            {
                LabelCorrPX.Content = corrPXList.Average().ToString("N2");
                LabelCorrPY.Content = corrPYList.Average().ToString("N2");
                LabelCorrPTheta.Content = corrPThetaList.Average().ToString("N2");
                LabelCorrPAngularSpeed.Content = corrPAngularSpeedList.Average().ToString("N2");
                LabelCorrPLinearSpeed.Content = corrPLinearSpeedList.Average().ToString("N2");

                LabelCorrIX.Content = corrIXList.Average().ToString("N2");
                LabelCorrIY.Content = corrIYList.Average().ToString("N2");
                LabelCorrITheta.Content = corrIThetaList.Average().ToString("N2");
                LabelCorrIAngularSpeed.Content = corrIAngularSpeedList.Average().ToString("N2");
                LabelCorrILinearSpeed.Content = corrILinearSpeedList.Average().ToString("N2");

                LabelCorrDX.Content = corrDXList.Average().ToString("N2");
                LabelCorrDY.Content = corrDYList.Average().ToString("N2");
                LabelCorrDTheta.Content = corrDThetaList.Average().ToString("N2");
                LabelCorrDAngularSpeed.Content = corrDAngularSpeedList.Average().ToString("N2");
                LabelCorrDLinearSpeed.Content = corrDLinearSpeedList.Average().ToString("N2");
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
                LabelIRigth.Content = IRRigthList.Average().ToString("N2");
                LabelIRCenter.Content = IRCenterList.Average().ToString("N2");
                LabelIRLeft.Content = IRLeftList.Average().ToString("N2");
                LabelIRLeftCenter.Content = IRLeftEndList.Average().ToString("N2");
            }

            if (timestampList.Count > 0)
                LabelTimestamp.Content = timestampList.Average().ToString("N2");

        }
        #endregion

        #region UpdateValues
        public void UpdatePolarSpeedConsigneValues(double consigneX, double consigneY, double consigneTheta,
            double consigneAngularSpeed, double consigneLinearSpeed)
        {
            consigneXList.Enqueue(consigneX);
            consigneYList.Enqueue(consigneY);
            consigneThetaList.Enqueue(consigneTheta);
            consigneAngularSpeedList.Enqueue(consigneAngularSpeed);
            consigneLinearSpeedList.Enqueue(consigneLinearSpeed);
        }
        public void UpdateIndependantSpeedConsigneValues(object receiver, MotorMessageArgs e)
        {
            consigneM1List.Enqueue(e.leftMotor);
            consigneM2List.Enqueue(e.rightMotor);
        }

        public void UpdatePolarSpeedCommandValues(double commandX, double commandY, double commandTheta,
            double commandAngularSpeed, double commandLinearSpeed)
        {
            commandXList.Enqueue(commandX);
            commandYList.Enqueue(commandY);
            commandThetaList.Enqueue(commandTheta);
            commandAngularSpeedList.Enqueue(commandAngularSpeed);
            commandLinearSpeedList.Enqueue(commandLinearSpeed);
        }
        public void UpdateIndependantSpeedCommandValues(object receiver, MotorMessageArgs e)
        {
            commandM1List.Enqueue(e.leftMotor);
            commandM2List.Enqueue(e.rightMotor);
        }

        public void UpdatePolarOdometrySpeed(object sender, PositionMessageArgs e)
        {
            measuredXList.Enqueue(e.xPos);
            measuredYList.Enqueue(e.yPos);
            measuredThetaList.Enqueue(e.theta);
            measuredAngularSpeedList.Enqueue(e.angularSpeed);
            measuredLinearSpeedList.Enqueue(e.linearSpeed);
        }
        public void UpdateIndependantOdometrySpeed(object sender, MotorMessageArgs e)
        {
            measuredM1List.Enqueue(e.leftMotor);
            measuredM2List.Enqueue(e.rightMotor);
        }

        public void UpdatePolarSpeedErrorValues(double errorX, double errorY, double errorTheta,
            double errorAngularSpeed, double errorLinearSpeed)
        {
            errorXList.Enqueue(errorX);
            errorYList.Enqueue(errorY);
            errorThetaList.Enqueue(errorTheta);
            errorAngularSpeedList.Enqueue(errorAngularSpeed);
            errorLinearSpeedList.Enqueue(errorLinearSpeed);
        }
        public void UpdateIndependantSpeedErrorValues(object sender, MotorMessageArgs e)
        {
            errorM1List.Enqueue(e.leftMotor);
            errorM2List.Enqueue(e.rightMotor);
        }

        public void UpdatePolarSpeedCorrectionValues(double corrPX, double corrPTheta, double corrIX,
            double corrITheta, double corrDX, double corrDTheta)
        {
            corrPXList.Enqueue(corrPX);
            corrPThetaList.Enqueue(corrPTheta);
            corrIXList.Enqueue(corrIX);
            corrIThetaList.Enqueue(corrITheta);
            corrDXList.Enqueue(corrDX);
            corrDThetaList.Enqueue(corrDTheta);
        }
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

        public void UpdatePolarSpeedCorrectionGains(double KpX, double KpTheta, double KiX,
            double KiTheta, double KdX, double KdTheta)
        {
            this.KpX = KpX;
            this.KpTheta = KpTheta;
            this.KiX = KiX;
            this.KiTheta = KiTheta;
            this.KdX = KdX;
            this.KdTheta = KdTheta;
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

        public void UpdatePolarSpeedCorrectionLimits(double corrLimitPX, double corrLimitPTheta,
            double corrLimitIX, double corrLimitITheta, double corrLimitDX, double corrLimitDTheta)
        {
            this.corrLimitPX = corrLimitPX;
            this.corrLimitPTheta = corrLimitPTheta;
            this.corrLimitIX = corrLimitIX;
            this.corrLimitITheta = corrLimitITheta;
            this.corrLimitDX = corrLimitDX;
            this.corrLimitDTheta = corrLimitDTheta;
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

        #region Update Robot Informations
        public void UpdateCheckInstruction(object receiver, MessageByteArgs e)
        {
            LabelInformationReceived.Content = (((Functions)e.msgFunction).ToString("N2"));
        }

        public void UpdateTelematersValues(object receiver, IRMessageArgs e)
        {
            IRRigthEndList.Enqueue(e.rigthEndIR);
            IRRigthList.Enqueue(e.rigthIR);
            IRCenterList.Enqueue(e.centerIR);
            IRLeftList.Enqueue(e.leftIR);
            IRLeftEndList.Enqueue(e.leftEndIR);
        }

        public void UpdateLEDState(object receiver, LEDMessageArgs e)
        {
            if (e.LEDNumber == 1)
            {
                CheckBoxLED1.IsChecked = e.LEDState;
                currentLED1State = e.LEDState;
            }
            else if (e.LEDNumber == 2)
            {
                CheckBoxLED2.IsChecked = e.LEDState;
                currentLED2State = e.LEDState;
            }
            else if (e.LEDNumber == 3)
            {
                CheckBoxLED3.IsChecked = e.LEDState;
                currentLED3State = e.LEDState;
            }
        }
        public void UpdateRobotStateAndTimestamp(object receiver, StateMessageArgs e)
        {
            LabelRobotState.Content = (((Functions)e.state).ToString("N2"));
            timestampList.Enqueue(e.time);
        }
        public void UpdateManualControl(object receiver, StateAutoControlMessageArgs e)
        {
            CheckBoxAutoControl.IsChecked = e.stateAutoControl;
        }
        public void UpdateTextBoxReception(object receiver, TextMessageArgs e)
        {
            TextBoxReception.Text += e.text;
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
        private void OnCheckBoxAutoControlSCheckChange(object sender, EventArgs e)
        {
            if (CheckBoxAutoControl.IsChecked != currentAutoControlState)
                OnSetAutoControlStateFromInterfaceGenerate(!currentAutoControlState);
        }
        private void OnCheckBoxAutoControlCheckChange(object sender, EventArgs e)
        {
            if (CheckBoxAutoControl.IsChecked != currentAutoControlState)
                OnSetLEDStateFromInterfaceGenerate(3, !currentLED3State);
        }

        private void OnButtonResetClick(object sender, RoutedEventArgs e)
        {
            OnResetPositionFromInterfaceGenerate();
        }

        private void OnButtonClearClick (object sender, RoutedEventArgs e)
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

        #region Events
        public event EventHandler<LEDMessageArgs> OnSetLEDStateFromInterfaceGenerateEvent;
        public void OnSetLEDStateFromInterfaceGenerate(ushort nbLED, bool LEDState)
        {
            OnSetLEDStateFromInterfaceGenerateEvent?.Invoke(this, new LEDMessageArgs(nbLED, LEDState));
        }

        public EventHandler<StateAutoControlMessageArgs> OnSetAutoControlStateFromInterfaceGenerateEvent;
        public void OnSetAutoControlStateFromInterfaceGenerate(bool autoControlSet)
        {
            OnSetAutoControlStateFromInterfaceGenerateEvent?.Invoke(this, new StateAutoControlMessageArgs(autoControlSet));
        }

        public event EventHandler<MotorMessageArgs> OnSetMotorSpeedFromInterfaceGenerateEvent;
        public void OnSetMotorSpeedFromInterfaceGenerate(sbyte leftMotor, sbyte rigthMotor)
        {
            OnSetMotorSpeedFromInterfaceGenerateEvent?.Invoke(this, new MotorMessageArgs(leftMotor, rigthMotor));
        }

        public event EventHandler<StateMessageArgs> OnSetRobotStateFromInterfaceGenerateEvent;
        public void OnSetRobotStateFromInterfaceGenerate(ushort robotState)
        {
            LabelInformationSent.Content = (((Functions)robotState).ToString("N2"));
            OnSetRobotStateFromInterfaceGenerateEvent?.Invoke(this, new StateMessageArgs(robotState));
        }

        public event EventHandler<SetPositionMessageArgs> OnSetPositionFromInterfaceGenrateEvent;
        public void OnSetPositionFromInterfaceGenrate(float xPos, float yPos, float angleRadian)
        {
            OnSetPositionFromInterfaceGenrateEvent?.Invoke(this, new SetPositionMessageArgs(xPos, yPos, angleRadian));
        }

        public event EventHandler<EventArgs> OnResetPositionFromInterfaceGenerateEvent;
        public void OnResetPositionFromInterfaceGenerate()
        {
            OnResetPositionFromInterfaceGenerateEvent?.Invoke(this, new EventArgs());
        }

        public event EventHandler<TextMessageArgs> OnSentTextMessageFromInterfaceGenerateEvent;
        public void OnSentTextMessageFromInterfaceGenerate(string content)
        {
            OnSentTextMessageFromInterfaceGenerateEvent?.Invoke(this, new TextMessageArgs(content));
        }
        #endregion

        #region Test
        private void OnButtonTestClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion 
    }
}