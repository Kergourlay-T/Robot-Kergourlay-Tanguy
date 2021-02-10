#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include <math.h>
#include "ChipConfig.h"
#include "IO.h"
#include "timer.h"
#include "PWM.h"
#include "Robot.h"
#include "ADC.h"
#include "main.h"

int main(void) {
    /***************************************************************************************************/
    //Initialisation de l'oscillateur
    /****************************************************************************************************/
    InitOscillator();

    /****************************************************************************************************/
    // Configuration des entr�es sorties
    /****************************************************************************************************/
    InitIO();

    LED_BLANCHE = 0;
    LED_BLEUE = 0;
    LED_ORANGE = 0;

    /***************************************************************************************************/
    //Initialisation des timers
    /****************************************************************************************************/
 
    InitTimer23();
    InitTimer1(50);
    InitTimer4(1000);


    /***************************************************************************************************/
    //Initialisation du PWM
    /****************************************************************************************************/
    InitPWM();

    /***************************************************************************************************/
    //Initialisation du ADC
    /****************************************************************************************************/
    InitADC1();

    unsigned int* result;


    /****************************************************************************************************/
    // Boucle Principale
    /****************************************************************************************************/
    while (1) {

        if (ADCIsConversionFinished() == 1) {
            ADCClearConversionFinishedFlag();
            result = ADCGetResult();

            unsigned int * result = ADCGetResult();

            float volts = ((float) result [0])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreDroitExtremite = 34 / volts - 5;
            if (robotState.distanceTelemetreDroitExtremite < 10) {
                LED_ORANGE = 1;
            } else {
                LED_ORANGE = 0;
            }

            volts = ((float) result [1])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreDroit = 34 / volts - 5;
            if (robotState.distanceTelemetreDroit < 10) {
                LED_ORANGE = 1;
            } else {
                LED_ORANGE = 0;
            }

            volts = ((float) result[2])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreCentre = 34 / volts - 5;
            if (robotState.distanceTelemetreCentre < 10) {
                LED_BLEUE = 1;
            } else {
                LED_BLEUE = 0;
            }

            volts = ((float) result[4])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreGauche = 34 / volts - 5;
            if (robotState.distanceTelemetreGauche < 10) {
                LED_BLANCHE = 1;
            } else {
                LED_BLANCHE = 0;
            }

            volts = ((float) result[3])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreGaucheExtremite = 34 / volts - 5;
            if (robotState.distanceTelemetreGaucheExtremite < 10) {
                LED_BLANCHE = 1;
            } else {
                LED_BLANCHE = 0;
            }

//            //Telemetre droit extremite
//            angleTelemetreDroitExtremite = acosf( (robotState.distanceTelemetreDroit^2 + 
//                    distanceTelemetre^2 -robotState.distanceTelemetreDroitExtremite^2) 
//                    / (2 * robotState.distanceTelemetreDroit * distanceTelemetre) );
//
//            xCoordonneePointDroitExtremite = cos(angleTelemetreDroitExtremite)*(xCoordonneeTelemetreDroit - xCoordonneeTelemetreDroitExtremite)
//                    + sin(angleTelemetreDroitExtremite)*(yCoordonneeTelemetreDroit - yCoordonneeTelemetreDroitExtremite);
//            yCoordonneePointDroitExtremite = -sin(angleTelemetreDroitExtremite)*(xCoordonneeTelemetreDroit - xCoordonneeTelemetreDroitExtremite)
//                    + cos(angleTelemetreDroitExtremite)*(yCoordonneeTelemetreDroit - yCoordonneeTelemetreDroitExtremite);
//            distanceCentreDroitExtremite = sqrt((xCoordonneePointDroitExtremite - xCoordonneeCentreRobot)^2 +
//                    (yCoordonneePointDroitExtremite - yCoordonneeCentreRobot)^2);
//
//            distanceReferencePointDroitExtremite = sqrt((xCoordonneePointDroitExtremite - xCoordonneeReferenceDroit)^2 +
//                    (yCoordonneePointDroitExtremite - yCoordonneeReferenceDroit)^2);
//            angleReferenceDroitExtremite = acosf((distanceCentreDroitExtremite^2 + distanceReferencePointDroitExtremite^2 -
//                    DistanceReferenceCentreRobot^2) / (2 * distanceCentreDroitExtremite * distanceReferencePointDroitExtremite));
//
//
//            //Telemetre droit
//            angleTelemetreDroit = acosf((robotState.distanceTelemetreCentre^2 + distanceTelemetre^2 -
//                    robotState.distanceTelemetreDroit^2) / (2 * robotState.distanceTelemetreCentre * distanceTelemetre));
//
//            xCoordonneePointDroit = cos(angleTelemetreDroit)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreDroit)
//                    + sin(angleTelemetreDroit)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreDroit);
//            yCoordonneePointDroit = -sin(angleTelemetreDroit)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreDroit)
//                    + cos(angleTelemetreDroit)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreDroit);
//
//            distanceCentreDroit = sqrt((xCoordonneePointDroit - xCoordonneeCentreRobot)^2 +
//                    (yCoordonneePointDroit - yCoordonneeCentreRobot)^2);
//
//            distanceReferencePointDroit = sqrt((xCoordonneePointDroit - xCoordonneeReferenceDroit)^2 +
//                    (yCoordonneePointDroit - yCoordonneeReferenceDroit)^2);
//            angleReferenceDroit = acosf((distanceCentreDroit^2 + distanceReferencePointDroit^2 -
//                    DistanceReferenceCritiqueCentreRobot^2) / (2 * distanceCentreDroit * distanceReferencePointDroit));
//
//
//
//            //Telemetre gauche extremite
//            angleTelemetreGaucheExtremite = acosf((robotState.distanceTelemetreGauche^2 + distanceTelemetre^2 -
//                    robotState.distanceTelemetreGaucheExtremite^2) / (2 * robotState.distanceTelemetreGauche * distanceTelemetre));
//
//            xCoordonneePointGaucheExtremite = cos(angleTelemetreGaucheExtremite)*(xCoordonneeTelemetreGauche - xCoordonneeTelemetreGaucheExtremite)
//                    + sin(angleTelemetreGaucheExtremite)*(yCoordonneeTelemetreGauche - yCoordonneeTelemetreGaucheExtremite);
//            yCoordonneePointGaucheExtremite = -sin(angleTelemetreGaucheExtremite)*(xCoordonneeTelemetreGauche - xCoordonneeTelemetreGaucheExtremite)
//                    + cos(angleTelemetreGaucheExtremite)*(yCoordonneeTelemetreGauche - yCoordonneeTelemetreGaucheExtremite);
//            distanceCentreGaucheExtremite = sqrt((xCoordonneePointGaucheExtremite - xCoordonneeCentreRobot)^2 +
//                    (yCoordonneePointGaucheExtremite - yCoordonneeCentreRobot)^2);
//
//            distanceReferencePointGaucheExtremite = sqrt((xCoordonneePointGaucheExtremite - xCoordonneeReferenceGauche)^2 +
//                    (yCoordonneePointGaucheExtremite - yCoordonneeReferenceGauche)^2);
//            angleReferenceGaucheExtremite = acosf((distanceCentreGaucheExtremite^2 + distanceReferencePointGaucheExtremite^2 -
//                    DistanceReferenceCentreRobot^2) / (2 * distanceCentreGaucheExtremite * distanceReferencePointGaucheExtremite));
//
//
//            //Telemetre gauche
//            angleTelemetreGauche = acosf((robotState.distanceTelemetreCentre^2 + distanceTelemetre^2 -
//                    robotState.distanceTelemetreGauche^2) / (2 * robotState.distanceTelemetreCentre * distanceTelemetre));
//
//            xCoordonneePointGauche = cos(angleTelemetreGauche)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreGauche)
//                    + sin(angleTelemetreGauche)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreGauche);
//            yCoordonneePointGauche = -sin(angleTelemetreGauche)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreGauche)
//                    + cos(angleTelemetreGauche)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreGauche);
//
//            distanceCentreGauche = sqrt((xCoordonneePointGauche - xCoordonneeCentreRobot)^2 +
//                    (yCoordonneePointGauche - yCoordonneeCentreRobot)^2);
//
//            distanceReferencePointGauche = sqrt((xCoordonneePointGauche - xCoordonneeReferenceGauche)^2 +
//                    (yCoordonneePointGauche - yCoordonneeReferenceGauche)^2);
//            angleReferenceGauche = acosf((distanceCentreGauche^2 + distanceReferencePointGauche^2 -
//                    DistanceReferenceCritiqueCentreRobot^2) / (2 * distanceCentreGauche * distanceReferencePointGauche));
//
//
//            if (((angleReferenceDroitExtremite < 90 && distanceCentreDroitExtremite < 30) || (distanceCentreDroit < 30 && angleReferenceDroit < 90))
//                    && (angleReferenceGaucheExtremite > 90 && distanceCentreGaucheExtremite > 30) && (distanceCentreGauche > 30 && angleReferenceGauche > 90));
//            if ((angleReferenceDroitExtremite > 90 && distanceCentreDroitExtremite > 30) && (distanceCentreDroit > 30 && angleReferenceDroit > 90)
//                    && ((angleReferenceGaucheExtremite < 90 && distanceCentreGaucheExtremite < 30) || (distanceCentreGauche < 30 && angleReferenceGauche < 90)));
//            if ((distanceCentreDroit < 30 && angleReferenceDroit < 90) || (distanceCentreGauche < 30 && angleReferenceGauche < 90)
//                    || (robotState.distanceTelemetreCentre < 20));


        }

        //520 � 540 20cm
        //280 � 300 40cm
    }
} // fin main




/***************************************************************************************************/
//D�claration OperatingSystemLoop
/****************************************************************************************************/


unsigned char stateRobot;

void OperatingSystemLoop(void) {
    switch (stateRobot) {
        case STATE_ATTENTE:
            timestamp = 0;
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_ATTENTE_EN_COURS;

        case STATE_ATTENTE_EN_COURS:
            if (timestamp > 1000)
                stateRobot = STATE_AVANCE;
            break;

        case STATE_AVANCE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_AVANCE_EN_COURS;
            break;
        case STATE_AVANCE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_EN_COURS;
            break;
        case STATE_TOURNE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_EN_COURS;
            break;
        case STATE_TOURNE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_GAUCHE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS;
            break;
        case STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_DROITE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS;
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        default:
            stateRobot = STATE_ATTENTE;
            break;
    }
}

unsigned char nextStateRobot = 0;

void SetNextRobotStateInAutomaticMode() {
    unsigned char positionObstacle = PAS_D_OBSTACLE;

    //D�termination de la position des obstacles en fonction des t�l�m�tres
    if (robotState.distanceTelemetreDroit < 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche > 30) //Obstacle � droite
        positionObstacle = OBSTACLE_A_DROITE;
    else if (robotState.distanceTelemetreDroit > 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche < 30) //Obstacle � gauche
        positionObstacle = OBSTACLE_A_GAUCHE;
    else if (robotState.distanceTelemetreCentre < 20) //Obstacle en face
        positionObstacle = OBSTACLE_EN_FACE;
    else if (robotState.distanceTelemetreDroit > 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche > 30) //pas d?obstacle
        positionObstacle = PAS_D_OBSTACLE;

    //D�termination de l?�tat � venir du robot
    if (positionObstacle == PAS_D_OBSTACLE) {
        nextStateRobot = STATE_AVANCE;
    } else if (positionObstacle == OBSTACLE_A_DROITE) {
        nextStateRobot = STATE_TOURNE_GAUCHE;
    } else if (positionObstacle == OBSTACLE_A_GAUCHE) {
        nextStateRobot = STATE_TOURNE_DROITE;
    } else if (positionObstacle == OBSTACLE_EN_FACE) {
        nextStateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE;
    }

    //Si l?on n?est pas dans la transition de l?�tape en cours
    if (nextStateRobot != stateRobot - 1) {
        stateRobot = nextStateRobot;
    }
}//SetNextRobotStateInAutomaticMode






