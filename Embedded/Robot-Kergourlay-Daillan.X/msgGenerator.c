#include "Protocol.h"
#include "msgGenerator.h"
#include "msgEncoder.h"
#include "timer.h"
#include "Robot.h"
#include "IO.h"
#include "Utilities.h"

void GenerateLEDMessage(int nbLED, int LEDState) {
    unsigned char payload[] = {nbLED, LEDState};
    UartEncodeAndSendMessage(LED_ROBOT_TO_GUI, 2, payload);
}

void GenerateTelemeterMessage() {
    unsigned char pos = 0;
    unsigned char payload[5];
    payload[pos++] = robotState.distanceTelemetreGaucheExtremite;
    payload[pos++] = robotState.distanceTelemetreGauche;
    payload[pos++] = robotState.distanceTelemetreCentre;
    payload[pos++] = robotState.distanceTelemetreDroit;
    payload[pos++] = robotState.distanceTelemetreDroitExtremite;
    UartEncodeAndSendMessage(TELEMETER_ROBOT_TO_GUI, 5, payload);
}

void GenerateMotorConsigneMessage() {
    unsigned char payload[8];
    getBytesFromFloat(payload, 0, (float) (robotState.vitesseDroiteConsigne));
    getBytesFromFloat(payload, 4, (float) (robotState.vitesseGaucheConsigne));
    UartEncodeAndSendMessage(MOTOR_CONSIGNE_ROBOT_TO_GUI, 8, payload);
}

void GenerateMotorMeasuredMessage() {
    unsigned char payload[8];
    getBytesFromFloat(payload, 0, (float) (robotState.vitesseDroiteCommandeCourante));
    getBytesFromFloat(payload, 4, (float) (robotState.vitesseGaucheCommandeCourante));
    UartEncodeAndSendMessage(MOTOR_MEASURED_ROBOT_TO_GUI, 8, payload);
}

void GenerateMotorErrordMessage() {
    unsigned char payload[8];
    getBytesFromFloat(payload, 0, (float) (robotState.vitesseDroiteErreure));
    getBytesFromFloat(payload, 4, (float) (robotState.vitesseGaucheErreure));
    UartEncodeAndSendMessage(MOTOR_ERROR_ROBOT_TO_GUI, 8, payload);
}

void GenerateRobotStateMessage(unsigned char stateRobot) {
    unsigned char payload[5] = {stateRobot, timestamp >> 24,
        timestamp >> 16, timestamp >> 8, timestamp >> 0};
    UartEncodeAndSendMessage(ROBOT_STATE_ROBOT_TO_GUI, 5, payload);
}

void GenerateManualControlMessage(int robotAutoControl) {
    unsigned char payload [] = {robotAutoControl};
    UartEncodeAndSendMessage(MANUAL_CONTROL_ROBOT_TO_GUI, 1, payload);
}

void GeneratePositionData() {
    unsigned char positionPayload[24];
    getBytesFromInt32(positionPayload, 0, timestamp);
    getBytesFromFloat(positionPayload, 4, (float) (robotState.xPosFromOdometry));
    getBytesFromFloat(positionPayload, 8, (float) (robotState.yPosFromOdometry));
    getBytesFromFloat(positionPayload, 12, (float) (robotState.angleRadianFromOdometry));
    getBytesFromFloat(positionPayload, 16, (float) (robotState.vitesseLineaireFromOdometry));
    getBytesFromFloat(positionPayload, 20, (float) (robotState.vitesseAngulaireFromOdometry));
    UartEncodeAndSendMessage(POSITION_DATA_ROBOT_TO_GUI, 24, positionPayload);
}

void GenerateTextMessage(unsigned char* message, unsigned int lenght) {
    UartEncodeAndSendMessage(MESSAGE_ROBOT_TO_GUI, lenght, message);
}

void GenerateSpeedPolarOdometryMessage() {
    unsigned char payload[32];
    getBytesFromFloat(payload, 0, robotState.vitesseLineaireConsigne);
    getBytesFromFloat(payload, 4, robotState.vitesseAngulaireConsigne);
    getBytesFromFloat(payload, 8, robotState.vitesseLineaireErreur);
    getBytesFromFloat(payload, 12, robotState.vitesseAngulaireErreur);
    getBytesFromFloat(payload, 16, robotState.vitesseLineaireFromOdometry);
    getBytesFromFloat(payload, 20, robotState.vitesseAngulaireFromOdometry);
    getBytesFromFloat(payload, 24, robotState.vitesseLineaireCommande);
    getBytesFromFloat(payload, 28, robotState.vitesseAngulaireCommande);
    UartEncodeAndSendMessage(SPEED_POLAR_ODOMETRY_ROBOT_TO_GUI, 32, payload);
}

void GenerateSpeedPolarGainsMessage() {
    unsigned char payload[24];
    getBytesFromFloat(payload, 0, robotState.KpLineaire);
    getBytesFromFloat(payload, 4, robotState.KiLineaire);
    getBytesFromFloat(payload, 8, robotState.KdLineaire);
    getBytesFromFloat(payload, 12, robotState.KpAngulaire);
    getBytesFromFloat(payload, 16, robotState.KiAngulaire);
    getBytesFromFloat(payload, 20, robotState.KdAngulaire);
    UartEncodeAndSendMessage(SPEED_POLAR_GAINS_ROBOT_TO_GUI, 24, payload);
}

void GenerateSpeedPolarCorrectionsMessage() {
    unsigned char payload[24];
    getBytesFromFloat(payload, 0, robotState.CorrectionLineaireKp);
    getBytesFromFloat(payload, 4, robotState.CorrectionLineaireKi);
    getBytesFromFloat(payload, 8, robotState.CorrectionLineaireKd);
    getBytesFromFloat(payload, 12, robotState.CorrectionAngulaireKp);
    getBytesFromFloat(payload, 16, robotState.CorrectionAngulaireKi);
    getBytesFromFloat(payload, 20, robotState.CorrectionAngulaireKd);
    UartEncodeAndSendMessage(SPEED_POLAR_CORRECTIONS_ROBOT_TO_GUI, 24, payload);
}

void GenerateSpeedPolarLimitGainsMessage() {
    unsigned char payload[24];
    getBytesFromFloat(payload, 0, robotState.KpLineaireMax);
    getBytesFromFloat(payload, 4, robotState.KiLineaireMax);
    getBytesFromFloat(payload, 8, robotState.KdLineaireMax);
    getBytesFromFloat(payload, 12, robotState.KpAngulaireMax);
    getBytesFromFloat(payload, 16, robotState.KiAngulaireMax);
    getBytesFromFloat(payload, 20, robotState.KdAngulaireMax);
    UartEncodeAndSendMessage(SPEED_POLAR_LIMIT_GAINS_ROBOT_TO_GUI, 24, payload);
}
