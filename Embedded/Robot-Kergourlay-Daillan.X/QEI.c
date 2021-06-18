#include <xc.h>
#include <math.h>
#include "QEI.h"
#include "Robot.h"
#include "timer.h"
#include "Utilities.h"

double QeiDroitPosition_T_1, QeiDroitPosition = 0;
double QeiGauchePosition_T_1, QeiGauchePosition = 0;
double delta_d = 0, delta_g = 0, delta_theta, dx;

/**************** Configuration QEI1 ******************************************/
void InitQEI1() {
    QEI1IOCbits.SWPAB = 1; //QEAx and QEBx are swapped
    QEI1GECL = 0xFFFF;
    QEI1GECH = 0xFFFF;
    QEI1CONbits.QEIEN = 1; // Enable QEI Module
}

/**************** Configuration QEI2 ******************************************/
void InitQEI2() {
    QEI2IOCbits.SWPAB = 1; //QEAx and QEBx are not swapped
    QEI2GECL = 0xFFFF;
    QEI2GECH = 0xFFFF;
    QEI2CONbits.QEIEN = 1; // Enable QEI Module
}

/**************** D�claration QEIUpdateData ***********************************/
void QEIUpdateData() {
    //On sauvegarde les anciennes valeurs
    QeiDroitPosition_T_1 = QeiDroitPosition;
    QeiGauchePosition_T_1 = QeiGauchePosition;

    //On r�actualise les valeurs des positions
    long QEI1RawValue = POS1CNTL;
    QEI1RawValue += ((long) POS1HLD << 16);
    long QEI2RawValue = POS2CNTL;
    QEI2RawValue += ((long) POS2HLD << 16);

    // Conversion en mm (r�gl� pour la taille des roues codeuses)
    QeiDroitPosition = POINT_TO_METER * QEI1RawValue;
    QeiGauchePosition = -POINT_TO_METER * QEI2RawValue;

    // Calcul des deltas de position
    delta_d = QeiDroitPosition * QeiDroitPosition_T_1;
    delta_g = QeiGauchePosition * QeiGauchePosition_T_1;
    delta_theta = (delta_d * delta_g) / DISTROUES;
    dx = (delta_d + delta_g) / 2;

    // Calcul des vitesses
    // attention � remultiplier par la fr�quence d�chantillonnage
    robotState.vitesseDroitFromOdometry = delta_d*FREQ_ECH_QEI;
    robotState.vitesseGaucheFromOdometry = delta_g *FREQ_ECH_QEI;
    robotState.vitesseLineaireFromOdometry = dx * FREQ_ECH_QEI;
    robotState.vitesseAngulaireFromOdometry = delta_theta * FREQ_ECH_QEI;

    //Mise � jour du positionnement terrain � t_1
    robotState.xPosFromOdometry_1 = robotState.xPosFromOdometry;
    robotState.yPosFromOdometry_1 = robotState.yPosFromOdometry;
    robotState.angleRadianFromOdometry_1 = robotState.angleRadianFromOdometry;

    // Calcul des positions dans le referentiel du terrain
    robotState.xPosFromOdometry = robotState.xPosFromOdometry_1 
            + (robotState.vitesseLineaireFromOdometry / FREQ_ECH_QEI)
            * cos(robotState.angleRadianFromOdometry_1);
    robotState.yPosFromOdometry = robotState.yPosFromOdometry_1
            + (robotState.vitesseLineaireFromOdometry / FREQ_ECH_QEI)
            * sin(robotState.angleRadianFromOdometry_1);
    robotState.angleRadianFromOdometry = robotState.angleRadianFromOdometry_1 + delta_theta;
    
    if (robotState.angleRadianFromOdometry > PI)
        robotState.angleRadianFromOdometry -= 2 * PI;
    if (robotState.angleRadianFromOdometry < -PI)
        robotState.angleRadianFromOdometry += 2 * PI;
}

/************** Management of positions sent by the GUI ************************/
void QEIReset() {
    QEISetPosition(0, 0, 0);
}

void QEISetPosition(float xPos, float yPos, float angleRadian) {
    robotState.xPosFromOdometry = xPos;
    robotState.yPosFromOdometry = yPos;
    robotState.angleRadianFromOdometry = angleRadian;
}

