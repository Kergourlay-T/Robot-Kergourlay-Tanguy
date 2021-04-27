#include <xc.h>
#include "SendMessage.h"
#include "QEI.h"
#include "Utilities.h"
#include "timer.h"
#include "Robot.h"
#include "IO.h"
#include "UART_Protocol.h"
#include "RobotControlState.h"

void SendRobotInformations() {
    int pos = 0;

    unsigned char LED[1];
    int nbLed = 0;
    LED[0] = nbLed++;
    LED[1] = LED_ORANGE;
    UartEncodeAndSendMessage(LED_PROTOCOL, 2, LED);
    LED[0] = nbLed++;
    LED[1] = LED_BLEUE;
    UartEncodeAndSendMessage(LED_PROTOCOL, 2, LED);
    LED[0] = nbLed++;
    LED[1] = LED_BLANCHE;
    UartEncodeAndSendMessage(LED_PROTOCOL, 2, LED);

    pos = 0;
    unsigned char IR[2];
    IR[pos++] = robotState.distanceTelemetreGauche;
    IR[pos++] = robotState.distanceTelemetreCentre;
    IR[pos++] = robotState.distanceTelemetreDroit;
    UartEncodeAndSendMessage(TELEMETRE_PROTOCOL, 3, IR);

//    pos = 0;
//    unsigned char Motors[1];
//    //robotState.vitesseGaucheCommandeCourante
//    Motors[pos++] = MOTEUR_GAUCHE_DUTY_CYCLE;
//    Motors[pos++] = MOTEUR_DROIT_DUTY_CYCLE;
//    UartEncodeAndSendMessage(MOTORS_PROTOCOL, 2, Motors);
  
    unsigned char sendRobotAutoControl[1]={robotAutoControl};
    UartEncodeAndSendMessage(SET_ROBOT_MANUAL_CONTROL, 1, sendRobotAutoControl);
}

void SendPositionData() {
    unsigned char positionPayload[24];
    getBytesFromInt32(positionPayload, 0, timestamp);
    getBytesFromFloat(positionPayload, 4, (float) (robotState.xPosFromOdometry));
    getBytesFromFloat(positionPayload, 8, (float) (robotState.yPosFromOdometry));
    getBytesFromFloat(positionPayload, 12, (float) (robotState.angleRadianFromOdometry));
    getBytesFromFloat(positionPayload, 16, (float) (robotState.vitesseLineaireFromOdometry));
    getBytesFromFloat(positionPayload, 20, (float) (robotState.vitesseAngulaireFromOdometry));
    UartEncodeAndSendMessage(POSITION_DATA, 24, positionPayload);
}
