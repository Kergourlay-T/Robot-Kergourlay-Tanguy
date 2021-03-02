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
#include "ToolBox.h"

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
    InitTimer1(300);
    InitTimer4(1000);

    /***************************************************************************************************/
    //Initialisation du PWM
    /****************************************************************************************************/
    InitPWM();

    /***************************************************************************************************/
    //Initialisation du ADC
    /****************************************************************************************************/
    InitADC1();


    /****************************************************************************************************/
    // Boucle Principale
    /****************************************************************************************************/
    while (1) {

        if (ADCIsConversionFinished() == 1) {
            ADCClearConversionFinishedFlag();
            unsigned int * result = ADCGetResult();

            float volts = ((float) result [0])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreDroitExtremite = 34 / volts - 5;

            volts = ((float) result [1])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreDroit = (34 / volts - 5);

            volts = ((float) result[2])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreCentre = 34 / volts - 5;

            volts = ((float) result[4])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreGauche = (34 / volts - 5);

            volts = ((float) result[3])*3.3 / 4096 * 3.2;
            robotState.distanceTelemetreGaucheExtremite = 34 / volts - 5;

            // Détermine l'obstacle le plus proche    
            float Distance[5] = {robotState.distanceTelemetreDroitExtremite, robotState.distanceTelemetreDroit,
                robotState.distanceTelemetreCentre, robotState.distanceTelemetreGauche, robotState.distanceTelemetreGaucheExtremite};
            robotState.distancePlusCourte = MinDistance(Distance);
            // Détermine la vitesse a adopter en fonction de la distance à l'objet le plus proche
            robotState.vitesseAdapteeMetre = robotState.distancePlusCourte / 100;
            if (2 * robotState.vitesseAdapteeMetre > vitesseMaximale)
                robotState.vitesseAdapteeMetre = vitesseMaximale / 2 - 10;
        }
    }
} // fin main


/***************************************************************************************************/
//Déclaration OperatingSystemLoop
/****************************************************************************************************/
unsigned char stateRobot;

void OperatingSystemLoop(void) {
    float vitesseDroiteCalcul = 0, vitesseGaucheCalcul = 0;
    switch (stateRobot) {
        case STATE_ATTENTE:
            timestamp = 0;
            robotState.demiTourAlea = 0;
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_ATTENTE_EN_COURS;

        case STATE_ATTENTE_EN_COURS:
            if (timestamp > 1000)
                stateRobot = STATE_AVANCE;
            break;

        case STATE_AVANCE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre;
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre;
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            if (vitesseDroiteCalcul < 8)
                vitesseDroiteCalcul = 8;
            if (vitesseGaucheCalcul < 8)
                vitesseGaucheCalcul = 8;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 0;
            stateRobot = STATE_AVANCE_EN_COURS;
            break;
        case STATE_AVANCE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 30 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 30 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 1;
            LED_ORANGE = 0;
            break;
        case STATE_TOURNE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 30 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 30 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 0;
            break;
        case STATE_TOURNE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_COULOIR_A_DROITE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 5 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 5 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            stateRobot = STATE_COULOIR_A_DROITE_EN_COURS;
            break;
        case STATE_COULOIR_A_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_COULOIR_A_GAUCHE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 5 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 5 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_COULOIR_A_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            break;
        case STATE_COULOIR_A_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_GAUCHE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 40 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 40 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 40 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 40 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE_LEGER:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 18 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 18 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_LEGER_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_DROITE_LEGER_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;


        case STATE_TOURNE_GAUCHE_LEGER:
            vitesseDroiteCalcul = robotState.vitesseAdapteeMetre + (PI * distanceEntreRoues * 18 / 360);
            vitesseGaucheCalcul = robotState.vitesseAdapteeMetre - (PI * distanceEntreRoues * 18 / 360);
            vitesseDroiteCalcul = (100 * vitesseDroiteCalcul) / vitesseMaximale;
            vitesseGaucheCalcul = (100 * vitesseGaucheCalcul) / vitesseMaximale;
            PWMSetSpeedConsigne(vitesseDroiteCalcul, MOTEUR_DROIT);
            PWMSetSpeedConsigne(vitesseGaucheCalcul, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_LEGER_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_GAUCHE_LEGER_EN_COURS:
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

    if ((robotState.distanceTelemetreCentre < 20)
            || ((robotState.distanceTelemetreDroit < 20) && (robotState.distanceTelemetreGauche < 20))) //Obstacle en face
        positionObstacle = OBSTACLE_EN_FACE;
    else if ((robotState.distanceTelemetreDroitExtremite < 10)
            && (robotState.distanceTelemetreGaucheExtremite < 10)
            && (robotState.distanceTelemetreDroit < 15)
            && (robotState.distanceTelemetreGauche < 15)) {
        if (robotState.distanceTelemetreDroitExtremite < robotState.distanceTelemetreGaucheExtremite) {
            positionObstacle = COULOIR_A_DROITE;
        } else {
            positionObstacle = COULOIR_A_GAUCHE;
        }
    } else if ((robotState.distanceTelemetreDroitExtremite < 10)
            && (robotState.distanceTelemetreGaucheExtremite > 10)
            && (robotState.distanceTelemetreDroit < 15)
            && (robotState.distanceTelemetreGauche > 15)) {
        positionObstacle = OBSTACLE_A_DROITE_LEGER;
    } else if ((robotState.distanceTelemetreDroitExtremite > 10)
            && (robotState.distanceTelemetreGaucheExtremite < 10)
            && (robotState.distanceTelemetreDroit > 15)
            && (robotState.distanceTelemetreGauche < 15)) {
        positionObstacle = OBSTACLE_A_GAUCHE_LEGER;
    } else if (robotState.distanceTelemetreDroit < 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche > 30) //Obstacle à droite
        positionObstacle = OBSTACLE_A_DROITE;
    else if (robotState.distanceTelemetreDroit > 30 &&
            robotState.distanceTelemetreCentre > 20 &&
            robotState.distanceTelemetreGauche < 30) //Obstacle à gauche
        positionObstacle = OBSTACLE_A_GAUCHE;
    else {
        positionObstacle = PAS_D_OBSTACLE;
        robotState.demiTourAlea = (robotState.demiTourAlea + 1) % 2;
    }

    //    if (robotState.distancePlusCourte > 30) {
    //        positionObstacle = PAS_D_OBSTACLE;
    //        robotState.demiTourAlea = (robotState.demiTourAlea + 1) % 2;
    //    } else if (
    //            (robotState.distanceTelemetreCentre == robotState.distancePlusCourte)
    //            || //telemetre droit
    //            ((robotState.distanceTelemetreDroit == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreGauche - robotState.distanceTelemetreDroit) <= 2.5)))
    //            ||
    //            ((robotState.distanceTelemetreDroit == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreGaucheExtremite - robotState.distanceTelemetreDroit) <= 2.5)))
    //            || //telemetre gauche  
    //            ((robotState.distanceTelemetreGauche == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreDroit - robotState.distanceTelemetreGauche) <= 2.5)))
    //            ||
    //            ((robotState.distanceTelemetreGauche == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreDroitExtremite - robotState.distanceTelemetreGauche) <= 2.5)))
    //            || //telemetre droit extrémité    
    //            ((robotState.distanceTelemetreDroitExtremite == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreGauche - robotState.distanceTelemetreDroitExtremite) <= 2.5)))
    //            ||
    //            ((robotState.distanceTelemetreDroitExtremite == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreGaucheExtremite - robotState.distanceTelemetreDroitExtremite) <= 2.5)))
    //            || //telemetre gauche extrémité        
    //            ((robotState.distanceTelemetreGaucheExtremite == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreDroit - robotState.distanceTelemetreGaucheExtremite) <= 2.5)))
    //            ||
    //            ((robotState.distanceTelemetreGaucheExtremite == robotState.distancePlusCourte)
    //            &&((abs(robotState.distanceTelemetreDroitExtremite - robotState.distanceTelemetreGaucheExtremite) <= 2.5)))
    //            )
    //        positionObstacle = OBSTACLE_EN_FACE;
    //    else if (
    //            ((robotState.distanceTelemetreDroitExtremite == robotState.distancePlusCourte)
    //            || (robotState.distanceTelemetreGauche == robotState.distancePlusCourte))
    //            && ((robotState.distanceTelemetreDroitExtremite - robotState.distanceTelemetreGaucheExtremite) <= 2.5)
    //            ) {
    //        if (robotState.distanceTelemetreDroitExtremite < robotState.distanceTelemetreGaucheExtremite)
    //            positionObstacle = COULOIR_A_DROITE;
    //        else
    //            positionObstacle = COULOIR_A_GAUCHE;
    //    } else if (
    //            (robotState.distanceTelemetreDroitExtremite == robotState.distancePlusCourte)
    //            )
    //        positionObstacle = COULOIR_A_DROITE;
    //    else if (
    //            (robotState.distanceTelemetreGaucheExtremite == robotState.distancePlusCourte)
    //            )
    //        positionObstacle = COULOIR_A_GAUCHE;
    //    else if (
    //            (robotState.distanceTelemetreDroit == robotState.distancePlusCourte)
    //            )
    //        positionObstacle = OBSTACLE_A_DROITE;
    //    else if (
    //            (robotState.distanceTelemetreGauche == robotState.distancePlusCourte)
    //            )
    //        positionObstacle = OBSTACLE_A_GAUCHE;


    //Détermination de l'état à venir du robot
    if (positionObstacle == PAS_D_OBSTACLE) {
        nextStateRobot = STATE_AVANCE;
    } else if (positionObstacle == OBSTACLE_EN_FACE) {
        if (!robotState.demiTourAlea) {
            nextStateRobot = STATE_TOURNE_SUR_PLACE_DROITE;
        } else {
            nextStateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE;
        }
    } else if (positionObstacle == OBSTACLE_A_DROITE) {
        nextStateRobot = STATE_TOURNE_GAUCHE;
    } else if (positionObstacle == OBSTACLE_A_GAUCHE) {
        nextStateRobot = STATE_TOURNE_DROITE;
    } else if (positionObstacle == COULOIR_A_DROITE) {
        nextStateRobot = STATE_COULOIR_A_DROITE;
    } else if (positionObstacle == COULOIR_A_GAUCHE) {
        nextStateRobot = STATE_COULOIR_A_GAUCHE;
    } else if (positionObstacle == OBSTACLE_A_DROITE_LEGER) {
        nextStateRobot = STATE_TOURNE_DROITE_LEGER;
    } else if (positionObstacle == OBSTACLE_A_GAUCHE_LEGER) {
        nextStateRobot = STATE_TOURNE_GAUCHE_LEGER;
    } 

    //Si l'on n'est pas dans la transition de l'étape en cours
    if (nextStateRobot != stateRobot - 1) {
        stateRobot = nextStateRobot;
    }
}//SetNextRobotStateInAutomaticMode






