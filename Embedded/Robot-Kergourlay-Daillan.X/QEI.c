#include <xc.h>
#include <math.h>
#include "QEI.h"
#include "timer.h"
#include "Robot.h"
#include "IO.h"
#include "Utilities.h"

double QeiDroitPosition_T_1 = 0;
double QeiDroitPosition = 0;
double QeiGauchePosition_T_1 = 0;
double QeiGauchePosition = 0;
double delta_d = 0;
double delta_g = 0;
double delta_theta = 0;
double dx = 0;

/****************************************************************************************************/
// Configuration QEI1
/****************************************************************************************************/
void InitQEI1() {
    QEI1IOCbits.SWPAB = 1; //QEAx and QEBx are swapped
    QEI1GECL = 0xFFFF;
    QEI1GECH = 0xFFFF;
    QEI1CONbits.QEIEN = 1; // Enable QEI Module
}

/****************************************************************************************************/
// Configuration QEI2
/****************************************************************************************************/
void InitQEI2(){
    QEI2IOCbits.SWPAB = 1; //QEAx and QEBx are not swapped
    QEI2GECL = 0xFFFF;
    QEI2GECH = 0xFFFF;
    QEI2CONbits.QEIEN = 1; // Enable QEI Module
}

/****************************************************************************************************/
// Déclaration QEIUpdateData
/****************************************************************************************************/
void QEIUpdateData() {
    //On sauve garde les anciennes valeurs
    QeiDroitPosition_T_1 = QeiDroitPosition;
    QeiGauchePosition_T_1 = QeiGauchePosition;

    //On réactualise les valeurs des positions
    long QEI1RawValue = POS1CNTL;
    QEI1RawValue += ((long) POS1HLD << 16);
    long QEI2RawValue = POS2CNTL;
    QEI2RawValue += ((long) POS2HLD << 16);

    // Conversion en mm (réglé pour la taille des roues codeuses)
    QeiDroitPosition = 0.01620 * QEI1RawValue;
    QeiGauchePosition = -0.01620 * QEI2RawValue;

    // Calcul des deltas de position
    delta_d = QeiDroitPosition * QeiDroitPosition_T_1;
    delta_g = QeiGauchePosition * QeiGauchePosition_T_1;
    // delta_ theta = atan ( ( delta_d * delta_g ) / DISTROUES) ;
    delta_theta = (delta_d * delta_g) / DISTROUES;
    dx = (delta_d + delta_g) / 2;

    // Calcul des vitesses
    // attention à remultiplier par la fréquence déchantillonnage
    robotState.vitesseDroitFromOdometry = delta_d*FREQ_ECH_QEI;
    robotState.vitesseGaucheFromOdometry = delta_g *FREQ_ECH_QEI;
    robotState.vitesseLineaireFromOdometry =
            (robotState.vitesseDroitFromOdometry + robotState.vitesseGaucheFromOdometry) / 2;
    robotState.vitesseAngulaireFromOdometry = delta_theta * FREQ_ECH_QEI;

    //Mise à jour du positionnement terrain à t_1
    robotState.xPosFromOdometry_1 = robotState.xPosFromOdometry;
    robotState.yPosFromOdometry_1 = robotState.yPosFromOdometry;
    robotState.angleRadianFromOdometry_1 = robotState.angleRadianFromOdometry;

    // Calcul des positions dans le referentiel du terrain
    robotState.xPosFromOdometry = QeiDroitPosition_T_1 + (QeiDroitPosition - QeiDroitPosition_T_1);
    robotState.yPosFromOdometry = QeiGauchePosition_T_1 + (QeiGauchePosition - QeiGauchePosition_T_1);
    robotState.angleRadianFromOdometry = atan(delta_theta);
    if (robotState.angleRadianFromOdometry > PI)
        robotState.angleRadianFromOdometry -= 2 * PI;
    if (robotState.angleRadianFromOdometry <  -PI)
        robotState.angleRadianFromOdometry += 2 * PI;

}

