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


//float xCoordonneeCentreRobot = 0;
//float yCoordonneeCentreRobot = 0;
//
//float xCoordonneeReferenceGauche = 25;
//float yCoordonneeReferenceGauche = 0;
//float xCoordonneeReferenceDroit = 25;
//float yCoordonneeReferenceDroit = 0;
//float xCoordonneeReferenceCritiqueGauche = 20;
//float yCoordonneeReferenceCritiqueGauche = 0;
//float xCoordonneeReferenceCritiqueDroit = 20;
//float yCoordonneeReferenceCritiqueDroit = 0;
//
//float DistanceReferenceCentreRobot = 25;
//float DistanceReferenceCritiqueCentreRobot = 10;
//
//float xCoordonneeTelemetreCentre = 0;
//float yCoordonneeTelemetreCentre = 0;
//float xCoordonneeTelemetreDroit = 6.3392; //cos(65)*15
//float yCoordonneeTelemetreDroit = 13.5946; //sin(65)*15
//float xCoordonneeTelemetreGauche = 6.3392;
//float yCoordonneeTelemetreGauche = 13.5946;
//float xCoordonneeTelemetreDroitExtremite = 11.4906; //sin(40)*15
//float yCoordonneeTelemetreDroitExtremite = 9.6418; //cos(40)*15
//float xCoordonneeTelemetreGaucheExtremite = 11.4906;
//float yCoordonneeTelemetreGaucheExtremite = 9.6418;
//
//float xCoordonneePointGauche;
//float yCoordonneePointGauche;
//float xCoordonneePointDroit;
//float yCoordonneePointDroit;
//float xCoordonneePointGaucheExtremite;
//float yCoordonneePointGaucheExtremite;
//float xCoordonneePointDroitExtremite;
//float yCoordonneePointDroitExtremite;
//
//float distanceCentreDroitExtremite;
//float distanceCentreDroit;
//float distanceCentreGaucheExtremite;
//float distanceCentreGauche;
//
//float distanceReferencePointDroitExtremite;
//float distanceReferencePointDroit;
//float distanceReferencePointGaucheExtremite;
//float distanceReferencePointGauche;
//
//float angleTelemetreDroitExtremite;
//float angleTelemetreDroit;
//float angleTelemetreGaucheExtremite;
//float angleTelemetreGauche;
//float angleReferenceDroitExtremite;
//float angleReferenceDroit;
//float angleReferenceGaucheExtremite;
//float angleReferenceGauche;

int main(void) {
    /***************************************************************************************************/
    //Initialisation de l'oscillateur
    /****************************************************************************************************/
    InitOscillator();

    /****************************************************************************************************/
    // Configuration des entrées sorties
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
            if (robotState.distanceTelemetreGaucheExtremite < 60) {
                LED_BLANCHE = 1;
            } else {
                LED_BLANCHE = 0;
            }

            //            //Telemetre droit extremite
            //
            //            //calcul angle beta par la loi cosinus
            //            angleTelemetreDroitExtremite = acos((pow(robotState.distanceTelemetreDroitExtremite, 2) + pow(distanceTelemetre, 2)
            //                    - pow(robotState.distanceTelemetreDroit, 2)) / (2 * robotState.distanceTelemetreDroitExtremite * distanceTelemetre));
            //
            //
            //            //calcul des coordonnées du croisement entre télémètres droit et droit extrémité
            //            xCoordonneePointDroitExtremite = (cos(angleTelemetreDroitExtremite)*(xCoordonneeTelemetreDroit - xCoordonneeTelemetreDroitExtremite)
            //                    + sin(angleTelemetreDroitExtremite)*(yCoordonneeTelemetreDroit - yCoordonneeTelemetreDroitExtremite))
            //                    *(robotState.distanceTelemetreDroitExtremite / distanceTelemetre) + xCoordonneeTelemetreDroitExtremite;
            //
            //            yCoordonneePointDroitExtremite = (-sin(angleTelemetreDroitExtremite)*(xCoordonneeTelemetreDroit - xCoordonneeTelemetreDroitExtremite)
            //                    + cos(angleTelemetreDroitExtremite)*(yCoordonneeTelemetreDroit - yCoordonneeTelemetreDroitExtremite))
            //                    *(robotState.distanceTelemetreDroitExtremite / distanceTelemetre) + yCoordonneeTelemetreDroitExtremite;
            //
            //
            //            //calcul distance entre le centre du robot et point de croisement des télémètres
            //            distanceCentreDroitExtremite = sqrtf(abs(pow(xCoordonneePointDroitExtremite - xCoordonneeCentreRobot, 2)
            //                    + pow(yCoordonneePointDroitExtremite - yCoordonneeCentreRobot, 2)));
            //            //calcul distance entre repère critique droit et croisement des télémètres
            //            distanceReferencePointDroitExtremite = sqrtf(abs(pow(xCoordonneePointDroitExtremite - xCoordonneeReferenceDroit, 2)
            //                    + pow(yCoordonneePointDroitExtremite - yCoordonneeReferenceDroit, 2)));
            //            //détermination de l'angle entre repère critique droit et croisement des télémètres
            //            angleReferenceDroitExtremite = acos((pow(distanceReferencePointDroitExtremite, 2) +
            //                    pow(DistanceReferenceCentreRobot, 2) - pow(distanceCentreDroitExtremite, 2))
            //                    / (2 * distanceReferencePointDroitExtremite * DistanceReferenceCentreRobot));
            //
            //
            //
            //            //Telemetre droit
            //
            //            //calcul angle beta par la loi cosinus
            //            angleTelemetreDroit = acosf((pow(distanceTelemetre, 2) + pow(robotState.distanceTelemetreDroit, 2)
            //                    - pow(robotState.distanceTelemetreCentre, 2)) / (2 * robotState.distanceTelemetreDroit * distanceTelemetre));
            //
            //            //calcul des coordonnées du croisement entre télémètres droit et centre
            //            xCoordonneePointDroit = (cos(angleTelemetreDroit)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreDroit)
            //                    + sin(angleTelemetreDroit)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreDroit))
            //                    *(distanceTelemetreDroit / distanceTelemetre) + xCoordonneeTelemetreDroit;
            //
            //            yCoordonneePointDroit = (-sin(angleTelemetreDroit)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreDroit)
            //                    + cos(angleTelemetreDroit)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreDroit))
            //                    *(distanceTelemetreDroit / distanceTelemetre) + yCoordonneeTelemetreDroit;
            //
            //            //calcul distance entre le centre du robot et point de croisement des télémètres
            //            distanceCentreDroit = sqrtf(abs(pow(xCoordonneePointDroit - xCoordonneeCentreRobot, 2)
            //                    + pow(yCoordonneePointDroit - yCoordonneeCentreRobot, 2)));
            //            //calcul distance entre repère critique droit et croisement des télémètres
            //            distanceReferencePointDroit = sqrtf(abs(pow(xCoordonneePointDroit - xCoordonneeReferenceCritiqueDroit, 2)
            //                    + pow(yCoordonneePointDroit - yCoordonneeReferenceCritiqueDroit, 2)));
            //            //détermination de l'angle entre repère critique droit et croisement des télémètres
            //            angleReferenceDroit = acosf(pow(distanceReferencePointDroit, 2) + pow(DistanceReferenceCritiqueCentreRobot, 2)
            //                    -(pow(distanceCentreDroit, 2)) / (2 * DistanceReferenceCritiqueCentreRobot * distanceReferencePointDroit));
            //
            //
            //
            //            //Telemetre gauche extremite
            //
            //            //calcul angle beta par la loi cosinus
            //            angleTelemetreGaucheExtremite = acos((pow(distanceTelemetre, 2) + pow(robotState.distanceTelemetreGaucheExtremite, 2)
            //                    - pow(robotState.distanceTelemetreGauche, 2)) / (2 * robotState.distanceTelemetreGaucheExtremite * distanceTelemetre));
            //
            //            //calcul des coordonnées du croisement entre télémètres gauche et gauche extrémit
            //            xCoordonneePointGaucheExtremite = (cos(angleTelemetreGaucheExtremite)*(xCoordonneeTelemetreGauche - xCoordonneeTelemetreGaucheExtremite)
            //                    + sin(angleTelemetreGaucheExtremite)*(yCoordonneeTelemetreGauche - yCoordonneeTelemetreGaucheExtremite))
            //                    *(robotState.distanceTelemetreGaucheExtremite / distanceTelemetre) + xCoordonneeTelemetreGaucheExtremite;
            //
            //
            //            yCoordonneePointGaucheExtremite = (-sin(angleTelemetreGaucheExtremite)*(xCoordonneeTelemetreGauche - xCoordonneeTelemetreGaucheExtremite)
            //                    + cos(angleTelemetreGaucheExtremite)*(yCoordonneeTelemetreGauche - yCoordonneeTelemetreGaucheExtremite))
            //                    *(robotState.distanceTelemetreGaucheExtremite / distanceTelemetre) + yCoordonneeTelemetreGaucheExtremite;
            //
            //            //calcul distance entre le centre du robot et point de croisement des télémètres
            //            distanceCentreGaucheExtremite = sqrtf(abs(pow(xCoordonneePointGaucheExtremite - xCoordonneeCentreRobot, 2)
            //                    + pow(yCoordonneePointGaucheExtremite - yCoordonneeCentreRobot, 2)));
            //            //calcul distance entre repère gauche et croisement des télémètres
            //            distanceReferencePointGaucheExtremite = sqrtf(abs(pow(xCoordonneePointGaucheExtremite - xCoordonneeReferenceGauche, 2)
            //                    + pow(yCoordonneePointGaucheExtremite - yCoordonneeReferenceGauche, 2)));
            //
            //            //détermination de l'angle entre repère gauche et croisement des télémètres
            //
            //            angleReferenceGaucheExtremite = acosf(pow(distanceReferencePointGaucheExtremite, 2)
            //                    + pow(DistanceReferenceCentreRobot, 2)-(pow(distanceCentreGaucheExtremite, 2))
            //                    / (2 * DistanceReferenceCentreRobot * distanceReferencePointGaucheExtremite));
            //
            //
            //
            //            //Telemetre gauche
            //
            //            //calcul angle beta par la loi
            //            angleTelemetreGauche = acosf((pow(distanceTelemetre, 2) + pow(robotState.distanceTelemetreGauche, 2)
            //                    pow(robotState.distanceTelemetreCentre, 2)) / (2 * robotState.distanceTelemetreGauche * distanceTelemetre));
            //
            //            //calcul des coordonnées du croisement entre télémètres gauche et centre
            //
            //            xCoordonneePointDroitExtremite = (cos(angleTelemetreDroitExtremite)*(xCoordonneeTelemetreDroit - xCoordonneeTelemetreDroitExtremite)
            //                    + sin(angleTelemetreDroitExtremite)*(yCoordonneeTelemetreDroit - yCoordonneeTelemetreDroitExtremite))
            //                    *(robotState.distanceTelemetreDroitExtremite / distanceTelemetre) + xCoordonneeTelemetreDroitExtremite;
            //
            //
            //            xCoordonneePointGauche = (cos(angleTelemetreGauche)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreGauche)
            //                    + sin(angleTelemetreGauche)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreGauche))
            //                    *(robotState.distanceTelemetreGaucheExtremite / distanceTelemetre) + xCoordonneeTelemetreGaucheExtremite;
            //
            //            yCoordonneePointGauche = (-sin(angleTelemetreGauche)*(xCoordonneeTelemetreCentre - xCoordonneeTelemetreGauche)
            //                    + cos(angleTelemetreGauche)*(yCoordonneeTelemetreCentre - yCoordonneeTelemetreGauche))
            //                    *(distanceTelemetreGaucheExtremite / distanceTelemtre) + yCoordonneeTelemetreGaucheExtremite;
            //
            //            //calcul distance entre le centre du robot et point de croisement des télémètres
            //            distanceCentreGauche = sqrt(abs(pow(xCoordonneePointGauche - xCoordonneeCentreRobot, 2)
            //                    + pow(yCoordonneePointGauche - yCoordonneeCentreRobot, 2)));
            //
            //            //calcul distance entre repère critique gauche et croisement des télémètres
            //            distanceReferencePointGauche = sqrt(abs(pow(xCoordonneePointGauche - xCoordonneeReferenceCritiqueGauche, 2)
            //                    + pow(yCoordonneePointGauche - yCoordonneeReferenceCritiqueGauche, 2)));
            //
            //            //détermination de l'angle entre repère critique gauche et croisement des télémètres
            //
            //            angleReferenceGauche = acos(pow(distanceReferencePointGauche, 2)
            //                    + pow(DistanceReferenceCritiqueCentreRobot, 2) -(pow(distanceCentreGauche, 2))
            //                    / (2 * DistanceReferenceCritiqueCentreRobot * distanceReferencePointGauche));
        }
    }
} // fin main


/***************************************************************************************************/
//Déclaration OperatingSystemLoop
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
            PWMSetSpeedConsigne(16, MOTEUR_DROIT);
            PWMSetSpeedConsigne(15, MOTEUR_GAUCHE);
            stateRobot = STATE_AVANCE_EN_COURS;
            break;
        case STATE_AVANCE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE:
            PWMSetSpeedConsigne(15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_EN_COURS;
            break;
        case STATE_TOURNE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(15, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_EN_COURS;
            break;
        case STATE_TOURNE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_GAUCHE:
            PWMSetSpeedConsigne(15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(-15, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS;
            break;
        case STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_DROITE:
            PWMSetSpeedConsigne(-15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(15, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS;
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        default:
            stateRobot = STATE_ATTENTE;
            break;
    }
}//OperatingSystemLoop

/***************************************************************************************************/
//Déclaration SetNextRobotStateInAutomaticMode
/****************************************************************************************************/
unsigned char nextStateRobot = 0;

void SetNextRobotStateInAutomaticMode() {
    unsigned char positionObstacle = PAS_D_OBSTACLE;

    //Détermination de la position des obstacles en fonction des télémètres
    if (robotState.distanceTelemetreDroit < 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche > 30) //Obstacle à droite
        positionObstacle = OBSTACLE_A_DROITE;
    else if (robotState.distanceTelemetreDroit > 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche < 30) //Obstacle à gauche
        positionObstacle = OBSTACLE_A_GAUCHE;
    else if (robotState.distanceTelemetreCentre < 20) //Obstacle en face
        positionObstacle = OBSTACLE_EN_FACE;
    else if (robotState.distanceTelemetreDroit > 30 &&

            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche > 30) //pas d'obstacle
        positionObstacle = PAS_D_OBSTACLE;

//    //fait demi-tour si les deux angles critiques sont inférieurs à 90°
//    if (((distanceCentreDroit < 25 && angleReferenceDroit < 90) && (distanceCentreGauche < 25 && angleReferenceGauche < 90))
//            || (robotState.distanceTelemetreCentre < 20)) {
//        positionObstacle = OBSTACLE_EN_FACE;
//    }//tourne à gauche si l'angle entre l'obstacle et l'un des repères est inférieur à 90°
//    else if (((angleReferenceDroitExtremite < 90 && distanceCentreDroitExtremite < 25)
//            || (distanceCentreDroit < 25 && angleReferenceDroit < 90))
//            || (distanceCentreDroitExtremite < 10)) {
//        positionObstacle = OBSTACLE_A_GAUCHE;
//    }//tourne à gauche si l'angle entre l'obstacle et l'un des repères est inférieur à 90°
//    else if ((angleReferenceDroitExtremite > 90 && distanceCentreDroitExtremite > 25)
//            || (distanceCentreDroit > 25 && angleReferenceDroit > 90)
//            || ((distanceCentreGaucheExtremite < 10))) {
//        positionObstacle = OBSTACLE_A_DROITE;
//    } else {
//        positionObstacle = PAS_D_OBSTACLE;
//    }

    //Détermination de l'état à venir du robot
    if (positionObstacle == PAS_D_OBSTACLE) {
        nextStateRobot = STATE_AVANCE;
    } else if (positionObstacle == OBSTACLE_A_DROITE) {
        nextStateRobot = STATE_TOURNE_GAUCHE;
    } else if (positionObstacle == OBSTACLE_A_GAUCHE) {
        nextStateRobot = STATE_TOURNE_DROITE;
    } else if (positionObstacle == OBSTACLE_EN_FACE) {
        nextStateRobot = STATE_TOURNE_SUR_PLACE_DROITE;
    }

    //Si l'on n'est pas dans la transition de l'étape en cours
    if (nextStateRobot != stateRobot - 1) {
        stateRobot = nextStateRobot;
    }
}//SetNextRobotStateInAutomaticMode






