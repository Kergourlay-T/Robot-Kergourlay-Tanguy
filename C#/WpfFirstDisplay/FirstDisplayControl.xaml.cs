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
    public partial class FirstDisplayControl : System.Windows.Controls.UserControl
    {
        #region Attributes
        int queueSize = 1;
        FixedSizedQueue<double> commandXList;
        FixedSizedQueue<double> commandYList;
        FixedSizedQueue<double> commandThetaList;
        FixedSizedQueue<double> commandM1List;
        FixedSizedQueue<double> commandM2List;
        FixedSizedQueue<double> commandAngleRadianList;
        FixedSizedQueue<double> commandAngularSpeedList;
        FixedSizedQueue<double> commandLinearSpeedList;

        FixedSizedQueue<double> consigneXList;
        FixedSizedQueue<double> consigneYList;
        FixedSizedQueue<double> consigneThetaList;
        FixedSizedQueue<double> consigneM1List;
        FixedSizedQueue<double> consigneM2List;
        FixedSizedQueue<double> consigneAngleRadianList;
        FixedSizedQueue<double> consigneAngularSpeedList;
        FixedSizedQueue<double> consigneLinearSpeedList;

        FixedSizedQueue<double> measuredXList;
        FixedSizedQueue<double> measuredYList;
        FixedSizedQueue<double> measuredThetaList;
        FixedSizedQueue<double> measuredM1List;
        FixedSizedQueue<double> measuredM2List;
        FixedSizedQueue<double> measuredAngleRadianList;
        FixedSizedQueue<double> measuredAngularSpeedList;
        FixedSizedQueue<double> measuredLinearSpeedList;

        FixedSizedQueue<double> errorXList;
        FixedSizedQueue<double> errorYList;
        FixedSizedQueue<double> errorThetaList;
        FixedSizedQueue<double> errorM1List;
        FixedSizedQueue<double> errorM2List;
        FixedSizedQueue<double> errorAngleRadianList;
        FixedSizedQueue<double> errorAngularSpeedList;
        FixedSizedQueue<double> errorLinearSpeedList;

        FixedSizedQueue<double> corrPXList;
        FixedSizedQueue<double> corrPYList;
        FixedSizedQueue<double> corrPThetaList;
        FixedSizedQueue<double> corrPM1List;
        FixedSizedQueue<double> corrPM2List;
        FixedSizedQueue<double> corrPAngleRadianList;
        FixedSizedQueue<double> corrPAngularSpeedList;
        FixedSizedQueue<double> corrPLinearSpeedList;

        FixedSizedQueue<double> corrIXList;
        FixedSizedQueue<double> corrIYList;
        FixedSizedQueue<double> corrIThetaList;
        FixedSizedQueue<double> corrIM1List;
        FixedSizedQueue<double> corrIM2List;
        FixedSizedQueue<double> corrIAngleRadianList;
        FixedSizedQueue<double> corrIAngularSpeedList;
        FixedSizedQueue<double> corrILinearSpeedList;

        FixedSizedQueue<double> corrDXList;
        FixedSizedQueue<double> corrDYList;
        FixedSizedQueue<double> corrDThetaList;
        FixedSizedQueue<double> corrDM1List;
        FixedSizedQueue<double> corrDM2List;
        FixedSizedQueue<double> corrDAngleRadianList;
        FixedSizedQueue<double> corrDAngularSpeedList;
        FixedSizedQueue<double> corrDLinearSpeedList;

        double corrLimitPX, corrLimitPY, corrLimitPTheta, corrLimitPM1, corrLimitPM2,
            corrLimitPAngleRadian, corrLimitPAngularSpeed, corrLimitPLinearSpeed;
        double corrLimitIX, corrLimitIY, corrLimitITheta, corrLimitIM1, corrLimitIM2,
            corrLimitIAngleRadian, corrLimitIAngularSpeed, corrLimitILinearSpeed;
        double corrLimitDX, corrLimitDY, corrLimitDTheta, corrLimitDM1, corrLimitDM2,
            corrLimitDAngleRadian, corrLimitDAngularSpeed, corrLimitDLinearSpeed;

        double KpX, KpY, KpTheta, KpM1, KpM2, KpAngleRadian, KpAngularSpeed, KpLinearSpeed;
        double KiX, KiY, KiTheta, KiM1, KiM2, KiAngleRadian, KiAngularSpeed, KiLinearSpeed;
        double KdX, KdY, KdTheta, KdM1, KdM2, KdAngleRadian, KdAngularSpeed, KdLinearSpeed;

        FixedSizedQueue<float> IRRigthEndList;
        FixedSizedQueue<float> IRRigthList;
        FixedSizedQueue<float> IRCenterList;
        FixedSizedQueue<float> IRLeftList;
        FixedSizedQueue<float> IRLeftEndList;
        FixedSizedQueue<float> robotStateList;
        FixedSizedQueue<float> timestampList;

        System.Timers.Timer displayTimer;

        bool currentLED1State;
        bool currentLED2State;
        bool currentLED3State;
        bool currentAutoControlState = true;

        private readonly KeyboardHookListener m_KeyboardHookManager;

        Enums.AsservissementMode asservissementMode = Enums.AsservissementMode.Disabled;
        #endregion

        public FirstDisplayControl()
        {
            InitializeComponent();

            commandXList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandYList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            commandM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            commandAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            commandLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            consigneXList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneYList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            consigneLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            measuredXList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredYList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            measuredLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            errorXList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorYList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            errorM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            errorAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            errorLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrPXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPThetaList = new Utilities.FixedSizedQueue<double>(queueSize); 
            corrPM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrPLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrIXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrIAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrILinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            corrDXList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDYList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDThetaList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM1List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDM2List = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDAngleRadianList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDAngularSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);
            corrDLinearSpeedList = new Utilities.FixedSizedQueue<double>(queueSize);

            IRRigthEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRRigthList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRCenterList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftList = new Utilities.FixedSizedQueue<float>(queueSize);
            IRLeftEndList = new Utilities.FixedSizedQueue<float>(queueSize);
            robotStateList = new Utilities.FixedSizedQueue<float>(queueSize);
            timestampList = new Utilities.FixedSizedQueue<float>(queueSize);

            consigneXList.Enqueue(0);
            consigneYList.Enqueue(0);
            consigneThetaList.Enqueue(0);
            consigneM1List.Enqueue(0);
            consigneM2List.Enqueue(0);
            consigneAngleRadianList.Enqueue(0);
            consigneAngularSpeedList.Enqueue(0);
            consigneLinearSpeedList.Enqueue(0);

            commandXList.Enqueue(0);
            commandYList.Enqueue(0);
            commandThetaList.Enqueue(0);
            commandM1List.Enqueue(0);
            commandM2List.Enqueue(0);
            commandAngleRadianList.Enqueue(0);
            commandAngularSpeedList.Enqueue(0);

            measuredXList.Enqueue(0);
            measuredYList.Enqueue(0);
            measuredThetaList.Enqueue(0);
            measuredM1List.Enqueue(0);
            measuredM2List.Enqueue(0);
            measuredAngleRadianList.Enqueue(0);
            measuredAngularSpeedList.Enqueue(0);
            measuredLinearSpeedList.Enqueue(0);

            errorXList.Enqueue(0);
            errorYList.Enqueue(0);
            errorThetaList.Enqueue(0);
            errorM1List.Enqueue(0);
            errorM2List.Enqueue(0);
            errorAngleRadianList.Enqueue(0);
            errorAngularSpeedList.Enqueue(0);
            errorLinearSpeedList.Enqueue(0);

            IRRigthEndList.Enqueue(0);
            IRRigthList.Enqueue(0);
            IRCenterList.Enqueue(0);
            IRLeftList.Enqueue(0);
            IRLeftEndList.Enqueue(0);
            robotStateList.Enqueue(0);
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
            LabelTitre.Content = titre;
        }

        public void SetAsservissementMode(Enums.AsservissementMode mode)
        {
            asservissementMode = mode;

            switch(asservissementMode)
            {
                case Enums.AsservissementMode.Disabled:
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
                case Enums.AsservissementMode.Polar:
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
                case Enums.AsservissementMode.Independant:
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
            LabelConsigneAngleRadian.Content = consigneAngleRadianList.Average().ToString("N2");
            LabelConsigneAngularSpeed.Content = consigneAngularSpeedList.Average().ToString("N2");
            LabelConsigneLinearSpeed.Content = consigneLinearSpeedList.Average().ToString("N2");

            LabelMeasureX.Content = measuredXList.Average().ToString("N2");
            LabelMeasureY.Content = measuredYList.Average().ToString("N2");
            LabelMeasureTheta.Content = measuredThetaList.Average().ToString("N2");
            LabelMeasureM1.Content = measuredM1List.Average().ToString("N2");
            LabelMeasureM2.Content = measuredM2List.Average().ToString("N2");
            LabelMeasureAngleRadian.Content = measuredAngleRadianList.Average().ToString("N2");
            LabelMeasureAngularSpeed.Content = measuredAngularSpeedList.Average().ToString("N2");
            LabelMeasureLinearSpeed.Content = measuredLinearSpeedList.Average().ToString("N2");

            LabelErreurX.Content = errorXList.Average().ToString("N2");
            LabelErreurY.Content = errorYList.Average().ToString("N2");
            LabelErreurTheta.Content = errorThetaList.Average().ToString("N2");
            LabelErreurM1.Content = errorM1List.Average().ToString("N2");
            LabelErreurM2.Content = errorM2List.Average().ToString("N2");
            LabelErreurAngleRadian.Content = errorAngleRadianList.Average().ToString("N2");
            LabelErreurAngularSpeed.Content = errorAngularSpeedList.Average().ToString("N2");
            LabelErreurLinearSpeed.Content = errorLinearSpeedList.Average().ToString("N2");

            LabelCommandX.Content = commandXList.Average().ToString("N2");
            LabelCommandY.Content = commandYList.Average().ToString("N2");
            LabelCommandTheta.Content = commandThetaList.Average().ToString("N2");
            LabelCommandM1.Content = commandM1List.Average().ToString("N2");
            LabelCommandM2.Content = commandM2List.Average().ToString("N2");
            LabelCommandAngleRadian.Content = commandAngleRadianList.Average().ToString("N2");
            LabelCommandAngularSpeed.Content = commandAngularSpeedList.Average().ToString("N2");
            LabelCommandLinearSpeed.Content = commandLinearSpeedList.Average().ToString("N2");

            LabelKpX.Content = KpX.ToString("N2");
            LabelKpY.Content = KpY.ToString("N2");
            LabelKpTheta.Content = KpTheta.ToString("N2");
            LabelKpM1.Content = KpM1.ToString("N2");
            LabelKpM2.Content = KpM2.ToString("N2");
            LabelKpAngleRadian.Content = KpAngleRadian.ToString("N2");
            LabelKpAngularSpeed.Content = KpAngularSpeed.ToString("N2");
            LabelKpLinearSpeed.Content = KpLinearSpeed.ToString("N2");

            LabelKiX.Content = KiX.ToString("N2");
            LabelKiY.Content = KiY.ToString("N2");
            LabelKiTheta.Content = KiTheta.ToString("N2");
            LabelKiM1.Content = KiM1.ToString("N2");
            LabelKiM2.Content = KiM2.ToString("N2");
            LabelKiAngleRadian.Content = KiAngleRadian.ToString("N2");
            LabelKiAngularSpeed.Content = KiAngularSpeed.ToString("N2");
            LabelKiLinearSpeed.Content = KiLinearSpeed.ToString("N2");

            LabelKdX.Content = KdX.ToString("N2");
            LabelKdY.Content = KdY.ToString("N2");
            LabelKdTheta.Content = KdTheta.ToString("N2");
            LabelKdM1.Content = KdM1.ToString("N2");
            LabelKdM2.Content = KdM2.ToString("N2");
            LabelKdAngleRadian.Content = KdAngleRadian.ToString("N2");
            LabelKdAngularSpeed.Content = KdAngularSpeed.ToString("N2");
            LabelKdLinearSpeed.Content = KdLinearSpeed.ToString("N2");

            LabelCorrMaxPX.Content = corrLimitPX.ToString("N2");
            LabelCorrMaxPY.Content = corrLimitPY.ToString("N2");
            LabelCorrMaxPTheta.Content = corrLimitPTheta.ToString("N2");
            LabelCorrMaxPM1.Content = corrLimitPM1.ToString("N2");
            LabelCorrMaxPM2.Content = corrLimitPM2.ToString("N2");
            LabelCorrMaxPAngleRadian.Content = corrLimitPAngleRadian.ToString("N2");
            LabelCorrMaxPAngularSpeed.Content = corrLimitPAngularSpeed.ToString("N2");
            LabelCorrMaxPLinearSpeed.Content = corrLimitPLinearSpeed.ToString("N2");

            LabelCorrMaxIX.Content = corrLimitIX.ToString("N2");
            LabelCorrMaxIY.Content = corrLimitIY.ToString("N2");
            LabelCorrMaxITheta.Content = corrLimitITheta.ToString("N2");
            LabelCorrMaxIM1.Content = corrLimitIM1.ToString("N2");
            LabelCorrMaxIM2.Content = corrLimitIM2.ToString("N2");
            LabelCorrMaxIAngleRadian.Content = corrLimitIAngleRadian.ToString("N2");
            LabelCorrMaxIAngularSpeed.Content = corrLimitIAngularSpeed.ToString("N2");
            LabelCorrMaxILinearSpeed.Content = corrLimitILinearSpeed.ToString("N2");

            LabelCorrMaxDX.Content = corrLimitDX.ToString("N2");
            LabelCorrMaxDY.Content = corrLimitDY.ToString("N2");
            LabelCorrMaxDTheta.Content = corrLimitDTheta.ToString("N2");
            LabelCorrMaxDM1.Content = corrLimitDM1.ToString("N2");
            LabelCorrMaxDM2.Content = corrLimitDM2.ToString("N2");
            LabelCorrMaxDAngleRadian.Content = corrLimitDAngleRadian.ToString("N2");
            LabelCorrMaxDAngularSpeed.Content = corrLimitDAngularSpeed.ToString("N2");
            LabelCorrMaxDLinearSpeed.Content = corrLimitDLinearSpeed.ToString("N2");


            if (corrPXList.Count > 0)
            {
                LabelCorrPX.Content = corrPXList.Average().ToString("N2");
                LabelCorrPY.Content = corrPYList.Average().ToString("N2");
                LabelCorrPTheta.Content = corrPThetaList.Average().ToString("N2");
                LabelCorrPAngleRadian.Content = corrPAngleRadianList.Average().ToString("N2");
                LabelCorrPAngularSpeed.Content = corrPAngularSpeedList.Average().ToString("N2");
                LabelCorrPLinearSpeed.Content = corrPLinearSpeedList.Average().ToString("N2");

                LabelCorrIX.Content = corrIXList.Average().ToString("N2");
                LabelCorrIY.Content = corrIYList.Average().ToString("N2");
                LabelCorrITheta.Content = corrIThetaList.Average().ToString("N2");
                LabelCorrIAngleRadian.Content = corrIAngleRadianList.Average().ToString("N2");
                LabelCorrIAngularSpeed.Content = corrIAngularSpeedList.Average().ToString("N2");
                LabelCorrILinearSpeed.Content = corrILinearSpeedList.Average().ToString("N2");

                LabelCorrDX.Content = corrDXList.Average().ToString("N2");
                LabelCorrDY.Content = corrDYList.Average().ToString("N2");
                LabelCorrDTheta.Content = corrDThetaList.Average().ToString("N2");
                LabelCorrDAngleRadian.Content = corrDAngleRadianList.Average().ToString("N2");
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

            if (robotStateList.Count > 0)
            {
                LabelRobotState.Content = (((Enums.Functions)robotStateList.Average()).ToString("N2"));
                LabelTimestamp.Content = timestampList.Average().ToString("N2");
            }

        }
#endregion

        #region UpdateValues
        public void UpdatePolarSpeedConsigneValues(double consigneX, double consigneY, double consigneTheta,
            double consigneAngleRadian, double consigneAngularSpeed, double consigneLinearSpeed)
        {
            consigneXList.Enqueue(consigneX);
            consigneYList.Enqueue(consigneY);
            consigneThetaList.Enqueue(consigneTheta);
            consigneAngleRadianList.Enqueue(consigneAngleRadian);
            consigneAngularSpeedList.Enqueue(consigneAngularSpeed);
            consigneLinearSpeedList.Enqueue(consigneLinearSpeed);

        }
        public void UpdateIndependantSpeedConsigneValues(double consigneM1, double consigneM2)
        {
            consigneM1List.Enqueue(consigneM1);
            consigneM2List.Enqueue(consigneM2);
        }

        public void UpdatePolarSpeedCommandValues(double commandX, double commandY, double commandTheta,
            double commandAngleRadian, double commandAngularSpeed, double commandLinearSpeed)
        {
            commandXList.Enqueue(commandX);
            commandYList.Enqueue(commandY);
            commandThetaList.Enqueue(commandTheta);
            commandAngleRadianList.Enqueue(commandAngleRadian);
            commandAngularSpeedList.Enqueue(commandAngularSpeed);
            commandLinearSpeedList.Enqueue(commandLinearSpeed);
        }
        public void UpdateIndependantSpeedCommandValues(double commandM1, double commandM2)
        {
            commandM1List.Enqueue(commandM1);
            commandM2List.Enqueue(commandM2);
        }

        public void UpdatePolarOdometrySpeed(double valueX, double valueY, double valueTheta,
            double valueRadianAngle, double valueAngularSpeed, double valueLinearSpeed)
        {
            measuredXList.Enqueue(valueX);
            measuredYList.Enqueue(valueY);
            measuredThetaList.Enqueue(valueTheta);
            measuredAngleRadianList.Enqueue(valueRadianAngle);
            measuredAngleRadianList.Enqueue(valueAngularSpeed);
            measuredLinearSpeedList.Enqueue(valueLinearSpeed);
        }
        public void UpdateIndependantOdometrySpeed(double valueM1, double valueM2)
        {
            measuredM1List.Enqueue(valueM1);
            measuredM2List.Enqueue(valueM2);
        }

        public void UpdatePolarSpeedErrorValues(double errorX, double errorY, double errorTheta,
            double errorAngleRadian, double errorAngularSpeed, double errorLinearSpeed)
        {
            errorXList.Enqueue(errorX);
            errorYList.Enqueue(errorY);
            errorThetaList.Enqueue(errorTheta);
            errorAngleRadianList.Enqueue(errorAngleRadian);
        }
        public void UpdateIndependantSpeedErrorValues(double errorM1, double errorM2)
        {
            errorM1List.Enqueue(errorM1);
            errorM2List.Enqueue(errorM2);
        }

        public void UpdatePolarSpeedCorrectionValues(double corrPX, double corrPTheta, double corrIX, 
            double corrITheta,double corrDX, double corrDTheta)
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
            double corrLimitIX, double corrLimitITheta,
            double corrLimitDX, double corrLimitDTheta)
        {
            this.corrLimitPX = corrLimitPX;
            this.corrLimitPTheta = corrLimitPTheta;
            this.corrLimitIX = corrLimitIX;
            this.corrLimitITheta = corrLimitITheta;
            this.corrLimitDX = corrLimitDX;
            this.corrLimitDTheta = corrLimitDTheta;
        }
        public void UpdateIndependantSpeedCorrectionLimits(double corrLimitPM1, double corrLimitPM2,
            double corrLimitIM1, double corrLimitIM2, 
            double corrLimitDM1, double corrLimitDM2)
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
        public void UpdateTelematersValues (float IRRigthEnd, float IRRigth, float IRCenter, 
            float IRLeft, float IRLeftEnd)
        {
            IRRigthEndList.Enqueue(IRRigthEnd);
            IRRigthList.Enqueue(IRRigth);
            IRCenterList.Enqueue(IRCenter);
            IRLeftList.Enqueue(IRLeft);
            IRLeftEndList.Enqueue(IRLeftEnd);
        }

        public void UpdateLEDState(ushort nbLED, bool LEDState)
        {
            if(nbLED == 1)
            {
                CheckBoxLED1.IsChecked = LEDState;
                currentLED1State = LEDState;
            }
            else if (nbLED == 2)
            {
                CheckBoxLED2.IsChecked = LEDState;
                currentLED2State = LEDState;
            }
            else if (nbLED == 3)
            {
                CheckBoxLED3.IsChecked = LEDState;
                currentLED3State = LEDState;
            }
        }

        public void UpdateRobotStateAndTimestamp(ushort robotState, uint timestamp)
        {
            robotStateList.Enqueue(0);
            timestampList.Enqueue(0);
        }

        public void UpdateTextBox(string content)
        {

        }

        public void OnCheckBoxLED1StateChange(object sender, EventArgs e)
        {
            if (CheckBoxLED1.IsChecked != currentLED1State)
            {
                OnSetLEDStateFromInterfaceGenerate(1, !currentLED1State);
            }
        }
        public void OnCheckBoxLED2StateChange(object sender, EventArgs e)
        {

            if (CheckBoxLED2.IsChecked != currentLED2State)
            {
                OnSetLEDStateFromInterfaceGenerate(2, !currentLED2State);
            }

        }
        public void OnCheckBoxLED3StateChange(object sender, EventArgs e)
        {
            if (CheckBoxLED3.IsChecked != currentLED3State)
            {
                OnSetLEDStateFromInterfaceGenerate(3, !currentLED3State);
            }
        }


        public event EventHandler<LEDMessageArgs> OnSetLEDStateFromInterfaceGenerateEvent;
        public void OnSetLEDStateFromInterfaceGenerate(ushort nbLED, bool LEDState)
        {
            OnSetLEDStateFromInterfaceGenerateEvent?.Invoke(this, new LEDMessageArgs(nbLED, LEDState));
        }


        public void OnCheckBoxAutoControlStateChange(object sender, EventArgs e)
        {
            if (CheckBoxAutoControl.IsChecked != currentAutoControlState)
            {
                OnSetAutoControlStateFromInterfaceGenerate(!currentAutoControlState);
            }
        }
        public EventHandler<StateAutoControlMessageArgs> OnSetAutoControlStateFromInterfaceGenerateEvent;
        public void OnSetAutoControlStateFromInterfaceGenerate (bool autoControlSet)
        {
            OnSetAutoControlStateFromInterfaceGenerateEvent?.Invoke(this, new StateAutoControlMessageArgs(autoControlSet));
        }

        public void OnCheckBoxAutoControlCheckChange(object sender, EventArgs e)
        {
            if (CheckBoxAutoControl.IsChecked != currentAutoControlState)
            {
                OnSetLEDStateFromInterfaceGenerate(3, !currentLED3State);
            }
        }

        public event EventHandler<MotorMessageArgs> OnSetMotorSpeedFromInterfaceGenerateEvent;
        public void OnSetMotorSpeedFromInterfaceGenerate(sbyte leftMotor, sbyte rigthMotor)
        {
            OnSetMotorSpeedFromInterfaceGenerateEvent?.Invoke(this, new MotorMessageArgs(leftMotor, rigthMotor));
        }


        public event EventHandler<StateMessageArgs> OnSetRobotStateFromInterfaceGenerateEvent;
        public void OnSetRobotStateFromInterfaceGenerate (ushort robotState)
        {
            OnSetRobotStateFromInterfaceGenerateEvent?.Invoke(this, new StateMessageArgs(robotState));
        }

        private void KeyboardHookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (currentAutoControlState == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        OnSetRobotStateFromInterfaceGenerate(stateRobot.);
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] { (byte)StateRobot.STATE_TOURNE_SUR_PLACE_GAUCHE });
                        break;

                    case Keys.Right:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] { (byte)StateRobot.STATE_TOURNE_SUR_PLACE_DROITE });
                        break;

                    case Keys.Up:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] { (byte)StateRobot.STATE_AVANCE });
                        break;

                    case Keys.Down:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] { (byte)StateRobot.STATE_RECULE });
                        break;

                    case Keys.PageDown:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] { (byte)StateRobot.STATE_ARRET });
                        break;
                }
            }
        }



        #endregion
    }
}

