#include <xc.h>
#include "RobotStateManagement.h"
#include "Robot.h"
#include "IO.h"
#include "PWM.h"
#include "timer.h"
#include "msgGenerator.h"
#include "Utilities.h"

boolean robotAutoControl = TRUE;
boolean demiTourAlea = TRUE;
unsigned char stateRobot = 0;
float speedAdaptedToDistances = 0;

/************* Management of instructions sent by the GUI *********************/
void SetRobotState(unsigned char receivedRobotState) {
    stateRobot = receivedRobotState;
}

void SetRobotAutoControlState(unsigned char receivedRobotAutoControlState) {
    robotAutoControl = (receivedRobotAutoControlState == 1) ? TRUE : FALSE;
    if (!robotAutoControl) {
        stateRobot = STATE_ARRET;
    } else {
        stateRobot = STATE_ATTENTE;
    }
}

boolean GetRobotAutoControlState() {
    return robotAutoControl;
}

/***************** Déclaration OperatingSystemLoop ****************************/
void OperatingSystemLoop(void) {
    GenerateManualControlMessage(robotAutoControl);

    switch (stateRobot) {
        case STATE_ATTENTE:
            timestamp = 0;
            robotAutoControl = 1;
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_ATTENTE_EN_COURS;
        case STATE_ATTENTE_EN_COURS:
            if (timestamp > 1000)
                stateRobot = STATE_AVANCE;
            break;

        case STATE_AVANCE:
            speedAdaptedToDistances = DetermineSpeedAdaptedToDistances();
            if (speedAdaptedToDistances >= 30) {
                LED_BLANCHE = 0;
                LED_BLEUE = 1;
                LED_ORANGE = 0;
            } else if (speedAdaptedToDistances >= 20) {
                LED_BLANCHE = 1;
                LED_BLEUE = 0;
                LED_ORANGE = 0;
            } else if (speedAdaptedToDistances < 20) {
                LED_BLANCHE = 0;
                LED_BLEUE = 0;
                LED_ORANGE = 0;
            }
            PWMSetSpeedConsigne(speedAdaptedToDistances, MOTEUR_DROIT);
            PWMSetSpeedConsigne(speedAdaptedToDistances, MOTEUR_GAUCHE);
            stateRobot = STATE_AVANCE_EN_COURS;
        case STATE_AVANCE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE:
            PWMSetSpeedConsigne(VITESSE_TOURNE, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 0;
            break;
        case STATE_TOURNE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_TOURNE, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_GAUCHE:
            PWMSetSpeedConsigne(VITESSE_DEMI_TOUR, MOTEUR_DROIT);
            PWMSetSpeedConsigne(-VITESSE_DEMI_TOUR, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_DROITE:
            PWMSetSpeedConsigne(-VITESSE_DEMI_TOUR, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_DEMI_TOUR, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_ARRET:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_ARRET_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 0;
            break;
        case STATE_ARRET_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_RECULE:
            PWMSetSpeedConsigne(VITESSE_RECULE, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_RECULE, MOTEUR_GAUCHE);
            stateRobot = STATE_RECULE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 0;
            break;
        case STATE_RECULE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_COULOIR_A_GAUCHE:
            PWMSetSpeedConsigne(VITESSE_TOURNE_LEGER, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_COULOIR, MOTEUR_GAUCHE);
            stateRobot = STATE_COULOIR_A_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 0;
            break;
        case STATE_COULOIR_A_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_COULOIR_A_DROITE:
            PWMSetSpeedConsigne(VITESSE_COULOIR, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_TOURNE_LEGER, MOTEUR_GAUCHE);
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            stateRobot = STATE_COULOIR_A_DROITE_EN_COURS;
            break;
        case STATE_COULOIR_A_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE_LEGER:
            PWMSetSpeedConsigne(VITESSE_TOURNE_LEGER, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_GAUCHE_LEGER_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 1;

            break;
        case STATE_TOURNE_GAUCHE_LEGER_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_DROITE_LEGER:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(VITESSE_TOURNE_LEGER, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_LEGER_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_DROITE_LEGER_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        default:
            stateRobot = STATE_ARRET;
            break;
    }
}//End OperatingSystemLoop

/***************** Déclaration SetNextRobotStateInAutomaticMode ***************/
unsigned char nextStateRobot = 0;

void SetNextRobotStateInAutomaticMode() {
    if (robotAutoControl == TRUE) {
        unsigned char positionObstacle = PAS_D_OBSTACLE;

        //Obstacle en face
        if ((robotState.distanceTelemetreCentre < 20))
            positionObstacle = OBSTACLE_EN_FACE;
            //Obstacle à l'extrémité droite
        else if (((robotState.distanceTelemetreDroitExtremite < 10)
                && (robotState.distanceTelemetreGaucheExtremite > 10))
                || (robotState.distanceTelemetreDroitExtremite < 5)) {
            positionObstacle = OBSTACLE_A_DROITE_LEGER;
        }//Obstacle à l'extrémité gauche
        else if (((robotState.distanceTelemetreDroitExtremite > 10)
                && (robotState.distanceTelemetreGaucheExtremite < 10))
                || (robotState.distanceTelemetreGaucheExtremite < 5)) {
            positionObstacle = OBSTACLE_A_GAUCHE_LEGER;
        }//Dans un couloir
        else if ((robotState.distanceTelemetreDroitExtremite < 10)
                && (robotState.distanceTelemetreGaucheExtremite < 10)) {
            if (robotState.distanceTelemetreDroitExtremite <
                    robotState.distanceTelemetreGaucheExtremite) {
                positionObstacle = COULOIR_A_DROITE;
            } else {
                positionObstacle = COULOIR_A_GAUCHE;
            }
        }//Obstacle en face très près
        else if ((robotState.distanceTelemetreDroit < 20) &&
                (robotState.distanceTelemetreGauche < 20))
            positionObstacle = OBSTACLE_EN_FACE;
            //Obstalce à droite
        else if (robotState.distanceTelemetreDroit < 30 &&
                robotState.distanceTelemetreCentre > 20 &&
                robotState.distanceTelemetreGauche > 30)
            positionObstacle = OBSTACLE_A_DROITE;
            //Obstacle à gauche
        else if (robotState.distanceTelemetreDroit > 30 &&
                robotState.distanceTelemetreCentre > 20 &&
                robotState.distanceTelemetreGauche < 30)
            positionObstacle = OBSTACLE_A_GAUCHE;
            //Pas d'obstacle
        else {
            positionObstacle = PAS_D_OBSTACLE;
            demiTourAlea = ((demiTourAlea + 1) % 2 == 1) ? TRUE : FALSE;
        }

        //Détermination de l'état à venir du robot
        if (positionObstacle == PAS_D_OBSTACLE) {
            nextStateRobot = STATE_AVANCE;
        } else if (positionObstacle == OBSTACLE_EN_FACE) {
            if (demiTourAlea) {
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
            GenerateRobotStateMessage(stateRobot);
        }
    }
}//End SetNextRobotStateInAutomaticMode

float DetermineSpeedAdaptedToDistances() {
    // Détermine l'obstacle le plus proche    
    float Distances[5] = {robotState.distanceTelemetreDroitExtremite,
        robotState.distanceTelemetreDroit, robotState.distanceTelemetreCentre,
        robotState.distanceTelemetreGauche, robotState.distanceTelemetreGaucheExtremite};
    // Détermine la vitesse à adopter en fonction de la distance à l'objet le plus proche
    speedAdaptedToDistances = MinDistance(Distances) - 10;
    if (speedAdaptedToDistances > MAXIMUM_SPEED)
        speedAdaptedToDistances = MAXIMUM_SPEED;
    else if (speedAdaptedToDistances < MINIMUM_SPEED)
        speedAdaptedToDistances = MINIMUM_SPEED;
    return speedAdaptedToDistances;
}


