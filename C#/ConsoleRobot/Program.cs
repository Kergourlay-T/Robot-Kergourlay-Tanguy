using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageDecoder;
using MessageEncoder;
using MessageGenerator;
using MessageProcessor;
using Serial;

namespace ConsoleRobot
{
    public class Program
    {
        static Serial serial;

        static void Main()
        {
            ConsoleFormat.ConsoleInformationFormat(Constants.ConsoleTitleFormatConst.MAIN, "Begin Booting Sequence", true);

            serial = new Serial();
            MessageDecoder.MsgDecoder.OnMessageDecoderCreatedEvent += ConsoleFormat.PrintMessageDecoderCreated;
            OnMessageEncoderCreatedEvent += ConsoleFormat.PrintMessageEncoderCreated;


            #region Event
            #region Communication 
            #region Serial
            //OnSerialConnectedEvent;
            //OnAutoConnectionLaunchedEvent;
            //OnNewSerialAttemptEvent;
            //OnSerialAvailableListEvent;
            //OnWrongCOMAvailableEvent;
            //OnCorrectCOMAvailableEvent;
            //OnSerialAvailableEvent;
            //OnNoConnectionAvailablEvent;
            //OnErrorWhileWhileAttemptingCOMEvent;
            PrintMessageDecoderCreated
            
          PrintMessageEncoderCreated
            
          PrintMessageProcessorCreated
           
            PrintMessageGeneratorCreated

            #endregion

            #region Hex Viewer
            OnMessageDecoderCreatedEvent;
            OnSOFByteReceivedEvent;
            OnFunctionMSBByteReceivedEvent;
            OnFunctionLSBByteReceivedEvent;
            OnPayloadLenghtMSBByteReceivedEvent;
            OnPayloadLenghtLSBByteReceivedEvent;
            OnPayloadByteReceivedEvent;
            OnPayloadReceivedEvent;
            OnChecksumByteReceivedEvent;
            OnCorrectChecksumReceivedEvent;
            OnWrongChecksumReceivedEvent;
            #endregion

            #region Hex Viewer Error
            OnUnknowByteReceivedEvent;
            OnUnknowFunctionEvent;
            OnOverLenghtMessageEvent;
            OnWrongLenghtEvent;
            #endregion

            #region Hex Sender
            OnMessageEncoderCreatedEvent;
            OnSendMessageEvent;
            OnSetLEDStateEvent;
            OnMotorSetSpeedEvent;
            OnSetStateEvent;
            #endregion

            #region Hex Sender Error 
            OnSerialDeconnectedEvent;
            OnWrongPayloadSendEvent;
            OnUnknowFunctionSentEvent;
            #endregion




            #region Processor
            OnMessageProcessorCreatedEvent;
            OnIRMessageReceivedEvent;
            OnMotorMessageReceivedEvent;
            OnStateMessageReceivedEvent;
            OnPositionMessageReceivedEvent;
            OnTextMessageReceivedEvent;
            OnUnknowFunctionReceivedEvent;
            #endregion

            #endregion //End region Communication
            #endregion //End region Event


        }


    }//End Program
}//End ConsolerOBOT
    
