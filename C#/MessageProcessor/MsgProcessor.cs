using System;
using System.Text;
using EventArgsLibrary;
using Protocol;
using Utilities;

/// <summary>
/// Takes as input the message decoded by MessageDecoder, then processes it.
/// Once processed, the message will be transformed into an outgoing event according to the command
/// </summary>

namespace MessageProcessor
{
    public class MsgProcessor
    {
        #region Input CallBack        
        public void ProcessRobotDecodedMessage(object sender, MessageDecodedArgs e)
        {
            ProcessDecodedMessage((ushort)e.MsgFunction, (ushort)e.MsgPayloadLength, e.MsgPayload);
        }
        #endregion

        #region Constructor
        public MsgProcessor()
        {
            OnMessageProcessorCreated();
        }
        public event EventHandler<EventArgs> OnMessageProcessorCreatedEvent;
        public virtual void OnMessageProcessorCreated() => OnMessageProcessorCreatedEvent?.Invoke(this, new EventArgs());
        #endregion

        #region Main Method
        public void ProcessDecodedMessage(ushort command, ushort payloadLength, byte[] payload)
        {
            switch (command)
            {
                case (ushort)Commands.CHECK_INSTRUCTION_ROBOT_TO_GUI:
                    {
                        OnCheckInstructionReceived(command, payloadLength, payload);
                    }
                    break;

                case (ushort)Commands.LED_ROBOT_TO_GUI:
                    {
                        ushort nbLed = Convert.ToUInt16(payload[0]);
                        bool stateLed = Convert.ToBoolean(payload[1]);
                        OnLEDMessageReceived(nbLed, stateLed);
                    }
                    break;

                case (ushort)Commands.TELEMETER_ROBOT_TO_GUI:
                    {
                        byte leftIR = payload[0];
                        byte leftEndIR = payload[1];
                        byte centerIR = payload[2];
                        byte rigthIR = payload[3];
                        byte rigthEndIR = payload[4];
                        OnIRMessageReceived(leftIR, leftEndIR, centerIR, rigthIR, rigthEndIR);
                    }
                    break;

                case (ushort)Commands.MOTOR_CONSIGNE_ROBOT_TO_GUI:
                    {
                        sbyte leftMotorConsigne = Convert.ToSByte(payload[0]);
                        sbyte rightMotorConsigne = Convert.ToSByte(payload[1]);
                        OnMotorConsigneMessageReceived(leftMotorConsigne, rightMotorConsigne);
                    }
                    break;

                case (ushort)Commands.MOTOR_MEASURED_ROBOT_TO_GUI:
                    {
                        sbyte leftMotorMeasured = Convert.ToSByte(payload[0]);
                        sbyte rightMotorMeasured = Convert.ToSByte(payload[1]);
                        OnMotorMeasuredMeassgeReceived(leftMotorMeasured, rightMotorMeasured);
                    }
                    break;

                case (ushort)Commands.MOTOR_ERROR_ROBOT_TO_GUI:
                    {
                        sbyte leftMotorError = Convert.ToSByte(payload[0]);
                        sbyte rightMotorError = Convert.ToSByte(payload[1]);
                        OnMotorMeasuredMeassgeReceived(leftMotorError, rightMotorError);
                    }
                    break;

                case (ushort)Commands.ROBOT_STATE_ROBOT_TO_GUI:
                    {
                        uint time = BitConverter.ToUInt32(payload, 0);
                        ushort robotState = payload[4];
                    }
                    break;

                case (ushort)Commands.MANUAL_CONTROL_ROBOT_TO_GUI:
                    {
                        bool manulControl = Convert.ToBoolean(payload[0]);
                        OnManualControlStateReceived(manulControl);
                    }

                    break;

                case (ushort)Commands.POSITION_DATA_ROBOT_TO_GUI:
                    {
                        uint time = BitConverter.ToUInt32(payload, 0);
                        float xPos = BitConverter.ToSingle(payload, 4);
                        float yPos = BitConverter.ToSingle(payload, 8); ;
                        float theta = BitConverter.ToSingle(payload, 12);
                        float linear = BitConverter.ToSingle(payload, 16);
                        float angular = BitConverter.ToSingle(payload, 20);
                        OnPositionMessageReceived(time, xPos, yPos, theta, linear, angular);
                    }
                    break;

                case (ushort)Commands.MESSAGE_ROBOT_TO_GUI:
                    {
                        string text = Encoding.UTF8.GetString(payload, 0, payload.Length);
                        OnTextMessageReceived(text);
                    }
                    break;

                case (ushort)Commands.SPEED_POLAR_ODOMETRY_ROBOT_TO_GUI:
                    {
                        float xConsigne = payload.GetRange(0, 4).GetFloat();
                        float thetaConsigne = payload.GetRange(4, 4).GetFloat();
                        float xError = payload.GetRange(8, 4).GetFloat();
                        float thetaError = payload.GetRange(12, 4).GetFloat();
                        float xMeasured = payload.GetRange(16, 4).GetFloat();
                        float thetaMeasured = payload.GetRange(20, 4).GetFloat();
                        float xCommand = payload.GetRange(24, 4).GetFloat();
                        float thetaCommand = payload.GetRange(28, 4).GetFloat();
                        OnPolarSpeedOdometryDataReceived(xConsigne, thetaConsigne, xError, thetaError,
                            xMeasured, thetaMeasured, xCommand, thetaCommand);
                    }
                    break;

                case (ushort)Commands.SPEED_POLAR_GAINS_ROBOT_TO_GUI:
                    {
                        float gainPx = payload.GetRange(0, 4).GetFloat();
                        float gainIx = payload.GetRange(0, 4).GetFloat();
                        float gainDx = payload.GetRange(0, 4).GetFloat();
                        float gainPtheta = payload.GetRange(0, 4).GetFloat();
                        float gainItheta = payload.GetRange(0, 4).GetFloat();
                        float gainDtheta = payload.GetRange(0, 4).GetFloat();
                        OnPolarSpeedPidGainsDataReceived(gainPx, gainIx, gainDx, gainPtheta, gainItheta, gainDtheta);
                    }
                    break;

                case (ushort)Commands.SPEED_POLAR_CORRECTIONS_ROBOT_TO_GUI:
                    {
                        float corrPx = payload.GetRange(0, 4).GetFloat();
                        float corrIx = payload.GetRange(4, 4).GetFloat();
                        float corrDx = payload.GetRange(8, 4).GetFloat();
                        float corrPtheta = payload.GetRange(12, 4).GetFloat();
                        float corrItheta = payload.GetRange(16, 4).GetFloat();
                        float corrDtheta = payload.GetRange(20, 4).GetFloat();
                        OnPolarSpeedCorrectionsDataReceived(corrPx, corrIx, corrDx, corrPtheta, corrItheta, corrDtheta);
                    }
                    break;

                case (ushort)Commands.SPEED_POLAR_LIMIT_GAINS_ROBOT_TO_GUI:
                    {
                        float limPx = payload.GetRange(0, 4).GetFloat();
                        float limIx = payload.GetRange(4, 4).GetFloat();
                        float limDx = payload.GetRange(8, 4).GetFloat();
                        float limPtheta = payload.GetRange(12, 4).GetFloat();
                        float limItheta = payload.GetRange(16, 4).GetFloat();
                        float limDtheta = payload.GetRange(20, 4).GetFloat();
                        OnPolarSpeedLimitGainsReceived(limPx, limIx, limDx, limPtheta, limItheta, limDtheta);
                    }
                    break;
            }
        }
        #endregion

        #region Output Event
        public event EventHandler<EventArgs> OnWelcomeMessageFromRobotGeneratedEvent;
        public virtual void OnWelcomeMessageFromRobot()
            => OnWelcomeMessageFromRobotGeneratedEvent?.Invoke(this, new EventArgs());


        public event EventHandler<MessageDecodedArgs> OnCheckInstructionReceivedEvent;
        public virtual void OnCheckInstructionReceived(ushort command, ushort payloadLength, byte[] payload)
            => OnCheckInstructionReceivedEvent?.Invoke(this, new MessageDecodedArgs
            {
                MsgFunction = command,
                MsgPayloadLength = payloadLength,
                MsgPayload = payload
            });


        public event EventHandler<LEDMessageArgs> OnLEDMessageReceivedEvent;
        public virtual void OnLEDMessageReceived(ushort nbLed, bool stateLed)
            => OnLEDMessageReceivedEvent?.Invoke(this, new LEDMessageArgs
            {
                nbLed_a = nbLed,
                stateLed_a = stateLed
            });


        public event EventHandler<IRMessageArgs> OnIRMessageReceivedEvent;
        public virtual void OnIRMessageReceived(byte leftIR, byte leftEndIR, byte centerIR, byte rigthIR, byte rigthEndIR)
            => OnIRMessageReceivedEvent?.Invoke(this, new IRMessageArgs
            {
                leftIR_a = leftIR,
                leftEndIR_a = leftEndIR,
                centerIR_a = centerIR,
                rigthIR_a = rigthIR,
                rigthEndIR_a = rigthEndIR
            });

        public event EventHandler<MotorMessageArgs> OnMotorConsigneMessageReceivedEvent;
        public virtual void OnMotorConsigneMessageReceived(sbyte leftMotor, sbyte rightMotor)
            => OnMotorConsigneMessageReceivedEvent?.Invoke(this, new MotorMessageArgs
            {
                leftMotor_a = leftMotor,
                rightMotor_a = rightMotor
            });

        public event EventHandler<MotorMessageArgs> OnMotorMeasuredMessageReceivedEvent;
        public virtual void OnMotorMeasuredMeassgeReceived(sbyte leftMotor, sbyte rightMotor)
            => OnMotorMeasuredMessageReceivedEvent?.Invoke(this, new MotorMessageArgs
            {
                leftMotor_a = leftMotor,
                rightMotor_a = rightMotor
            });


        public event EventHandler<MotorMessageArgs> OnMotorErrorMessageReceivedEvent;
        public virtual void OnMotorErrorMessageReceived(sbyte leftMotor, sbyte rightMotor)
            => OnMotorErrorMessageReceivedEvent?.Invoke(this, new MotorMessageArgs
            {
                leftMotor_a = leftMotor,
                rightMotor_a = rightMotor
            });


        public event EventHandler<StateMessageArgs> OnStateMessageReceivedEvent;
        public virtual void OnStateMessageReceived(ushort state, uint time)
            => OnStateMessageReceivedEvent?.Invoke(this, new StateMessageArgs
            {
                state_a = state,
                time_a = time
            });

        public event EventHandler<BoolEventArgs> OnManualControlStateReceivedEvent;
        public virtual void OnManualControlStateReceived(bool isEnable)
            => OnManualControlStateReceivedEvent?.Invoke(this, new BoolEventArgs { Value = isEnable });


        public event EventHandler<PositionMessageArgs> OnPositionMessageReceivedEvent;
        public virtual void OnPositionMessageReceived(uint time, float xPos, float yPos, float theta, float linear, float angular)
           => OnPositionMessageReceivedEvent?.Invoke(this, new PositionMessageArgs
           {
               time_a = time,
               xPos_a = xPos,
               yPos_a = yPos,
               theta_a = theta,
               linearSpeed_a = linear,
               angularSpeed_a = angular
           });

        public event EventHandler<TextMessageArgs> OnTextMessageReceivedEvent;
        public virtual void OnTextMessageReceived(string text)
            => OnTextMessageReceivedEvent?.Invoke(this, new TextMessageArgs { text_a = text });

        public event EventHandler<PolarSpeedOdometryData> OnPolarSpeedOdometryDataReceivedEvent;
        public virtual void OnPolarSpeedOdometryDataReceived(float xConsigne, float thetaConsigne, float xError,
            float thetaError, float xMeasured, float thetaMeasured, float xCommand, float thetaCommand)
            => OnPolarSpeedOdometryDataReceivedEvent?.Invoke(this, new PolarSpeedOdometryData
            {
                xConsigne_a = xConsigne,
                thetaConsigne_a = thetaConsigne,
                xError_a = xError,
                thetaError_a = thetaError,
                xMeasured_a = xMeasured,
                thetaMeasured_a = thetaMeasured,
                xCommand_a = xCommand,
                thetaCommand_a = thetaCommand
            });

        public event EventHandler<PolarSpeedPidData> OnPolarSpeedPidGainsDataReceivedEvent;
        public virtual void OnPolarSpeedPidGainsDataReceived(float gainPx, float gainIx, float gainDx,
            float gainPtheta, float gainItheta, float gainDtheta)
            => OnPolarSpeedPidGainsDataReceivedEvent?.Invoke(this, new PolarSpeedPidData
            {
                Px_a = gainPx,
                Ix_a = gainIx,
                Dx_a = gainDx,
                Ptheta_a = gainPtheta,
                Itheta_a = gainItheta,
                Dtheta_a = gainDtheta
            });

        public event EventHandler<PolarSpeedPidData> OnPolarSpeedCorrectionsDataReceivedEvent;
        public virtual void OnPolarSpeedCorrectionsDataReceived(float corrPx, float corrIx,
            float corrDx, float corrPtheta, float corrItheta, float corrDtheta)
            => OnPolarSpeedCorrectionsDataReceivedEvent?.Invoke(this, new PolarSpeedPidData
            {
                Px_a = corrPx,
                Ix_a = corrIx,
                Dx_a = corrDx,
                Ptheta_a = corrPtheta,
                Itheta_a = corrItheta,
                Dtheta_a = corrDtheta
            });

        public event EventHandler<PolarSpeedPidData> OnPolarSpeedLimitGainsReceivedEvent;
        public virtual void OnPolarSpeedLimitGainsReceived(float limPx, float limIx,
            float limDx, float limPtheta, float limItheta, float limDtheta)
            => OnPolarSpeedLimitGainsReceivedEvent?.Invoke(this, new PolarSpeedPidData
            {
                Px_a = limPx,
                Ix_a = limIx,
                Dx_a = limDx,
                Ptheta_a = limPtheta,
                Itheta_a = limItheta,
                Dtheta_a = limDtheta
            });

        #endregion //Output Event

    }
}
