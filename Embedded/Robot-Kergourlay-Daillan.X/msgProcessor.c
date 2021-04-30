#include <xc.h>
#include "Protocol.h"
#include "msgProcessor.h"
#include "msgEncoder.h"
#include "RobotStateManagement.h"
#include "IO.h"
#include "QEI.h"
#include "PWM.h"
#include "Utilities.h"

void UartProcessDecodedMessage(unsigned char function, 
        unsigned char payloadLength, unsigned char* payload) {
    switch (function) {
        case LED_GUI_TO_ROBOT:
            if (payloadLength == 2) {
                switch (payload[0]) {
                    case 0x01:
                        LED_ORANGE = (payload[1] == 0x01) ? 1 : 0;
                        break;
                    case 0x02:
                        LED_BLEUE = (payload[1] == 0x01) ? 1 : 0;
                        break;
                    case 0x03:
                        LED_BLANCHE = (payload[1] == 0x01) ? 1 : 0;
                        break;
                }
            }
            break;

        case MOTOR_GUI_TO_ROBOT:
            if (payloadLength == 2) {
                PWMSetSpeedConsigne(payload[0], MOTEUR_DROIT);
                PWMSetSpeedConsigne(payload[1], MOTEUR_GAUCHE);
            }
            break;

        case ROBOT_STATE_GUI_TO_ROBOT:
            if (payloadLength == 1) {
                if (!GetRobotAutoControlState())
                    SetRobotState(payload[0]);
                else
                    UartEncodeAndSendMessage(MESSAGE_ROBOT_TO_GUI,
                        9, (unsigned char*) "Mode Auto");
            }
            break;

        case MANUAL_CONTROL_GUI_TO_ROBOT:
            if (payloadLength == 1) {
                SetRobotAutoControlState(payload[0]);
            }
            break;

        case RESET_POSITION_GUI_TO_ROBOT:
            QEIReset();
            break;

        case POSITION_DATA_GUI_TO_ROBOT:
            if (payloadLength == 6) {
                float xPos = 0;
                float yPos = 0;
                float angleDegre = 0;
                xPos = getFloat(payload, 0) * 10 + getFloat(payload, 1);
                yPos = getFloat(payload, 2) * 10 + getFloat(payload, 3);
                angleDegre = getFloat(payload, 4) * 10 + getFloat(payload, 5);
                QEISetPosition(xPos, yPos, angleDegre);
            }
            break;

        default:
            break;
    }
    UartEncodeAndSendMessage(CHECK_INSTRUCTION_ROBOT_TO_GUI, payloadLength, payload);
}