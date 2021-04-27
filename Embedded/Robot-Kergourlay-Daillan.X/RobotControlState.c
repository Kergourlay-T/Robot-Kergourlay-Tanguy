#include <xc.h>
#include "RobotControlState.h"
#include "IO.h"
#include "Robot.h"
#include "PWM.h"
#include "timer.h"
#include "SendMessage.h"
#include "UART_Protocol.h"

unsigned char robotAutoControl;

/***************************************************************************************************/
//Déclaration OperatingSystemLoop
/****************************************************************************************************/
unsigned char stateRobot = 0;

void OperatingSystemLoop(void) {
    switch (stateRobot) {
        case STATE_ATTENTE:
            timestamp = 0;
            robotState.demiTourAlea = 0;
            robotAutoControl = 1;
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_ATTENTE_EN_COURS;
        case STATE_ATTENTE_EN_COURS:
            if (timestamp > 1000)
                stateRobot = STATE_AVANCE;
            break;

            /****************************************************/
            // Gestion State Avance
            /****************************************************/
        case STATE_AVANCE:
            if (robotState.speedAdaptedToDistances >= 30) {
                LED_BLANCHE = 0;
                LED_BLEUE = 1;
                LED_ORANGE = 0;
            } else if (robotState.speedAdaptedToDistances >= 20) {
                LED_BLANCHE = 1;
                LED_BLEUE = 0;
                LED_ORANGE = 0;
            } else if (robotState.speedAdaptedToDistances <= 20) {
                LED_BLANCHE = 0;
                LED_BLEUE = 0;
                LED_ORANGE = 0;
            }
            PWMSetSpeedConsigne(robotState.speedAdaptedToDistances, MOTEUR_DROIT);
            PWMSetSpeedConsigne(robotState.speedAdaptedToDistances, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_AVANCE_EN_COURS;
        case STATE_AVANCE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

            /****************************************************/
            // Gestion State Tourne
            /****************************************************/
        case STATE_TOURNE_GAUCHE:
            PWMSetSpeedConsigne(30, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            SendState(stateRobot);
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
            PWMSetSpeedConsigne(30, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_TOURNE_DROITE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

            /****************************************************/
            // Gestion State Tourne sur Place
            /****************************************************/
        case STATE_TOURNE_SUR_PLACE_GAUCHE:
            PWMSetSpeedConsigne(15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(-15, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_SUR_PLACE_DROITE:
            PWMSetSpeedConsigne(-15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(15, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 1;
            break;
        case STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

            /****************************************************/
            // Gestion State Arrêt et State Recule
            /****************************************************/
        case STATE_ARRET:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_ARRET_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 0;
            break;
        case STATE_ARRET_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_RECULE:
            PWMSetSpeedConsigne(-15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(-15, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_RECULE_EN_COURS;
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 0;
            break;
        case STATE_RECULE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

            /****************************************************/
            // Gestion State Couloir
            /****************************************************/
        case STATE_COULOIR_A_GAUCHE:
            PWMSetSpeedConsigne(15, MOTEUR_DROIT);
            PWMSetSpeedConsigne(10, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_COULOIR_A_GAUCHE_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 1;
            LED_ORANGE = 0;
            break;
        case STATE_COULOIR_A_GAUCHE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_COULOIR_A_DROITE:
            PWMSetSpeedConsigne(10, MOTEUR_DROIT);
            PWMSetSpeedConsigne(15, MOTEUR_GAUCHE);
            SendState(stateRobot);
            LED_BLANCHE = 0;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            stateRobot = STATE_COULOIR_A_DROITE_EN_COURS;
            break;
        case STATE_COULOIR_A_DROITE_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

            /****************************************************/
            // Gestion State Tourne Léger
            /****************************************************/
        case STATE_TOURNE_DROITE_LEGER:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            stateRobot = STATE_TOURNE_DROITE_LEGER_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 1;
            SendState(stateRobot);
            break;
        case STATE_TOURNE_DROITE_LEGER_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        case STATE_TOURNE_GAUCHE_LEGER:
            PWMSetSpeedConsigne(0, MOTEUR_DROIT);
            PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
            SendState(stateRobot);
            stateRobot = STATE_TOURNE_GAUCHE_LEGER_EN_COURS;
            LED_BLANCHE = 1;
            LED_BLEUE = 0;
            LED_ORANGE = 1;

            break;
        case STATE_TOURNE_GAUCHE_LEGER_EN_COURS:
            SetNextRobotStateInAutomaticMode();
            break;

        default:
            stateRobot = STATE_ATTENTE;
            break;
    }
}//End OperatingSystemLoop


/***************************************************************************************************/
//Déclaration SetNextRobotStateInAutomaticMode
/****************************************************************************************************/
unsigned char nextStateRobot = 0;

void SetNextRobotStateInAutomaticMode() {
    if (robotAutoControl == 1) {
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
            robotState.demiTourAlea = (robotState.demiTourAlea + 1) % 2;
        }

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
    }
}//SetNextRobotStateInAutomaticMode

void SetRobotState(unsigned char ReceivedSetRobotState) {
    //Fonction fixant l'état du robot
    stateRobot = ReceivedSetRobotState;
}

void SetRobotAutoControlState(unsigned char SetRobotControlState) {
    //Fonction qui fixe le mode de contrôle du robot
    if (SetRobotControlState == 0) {
        robotAutoControl = 0;
        stateRobot = STATE_ARRET;
    } else if (SetRobotControlState == 1) {
        robotAutoControl = 1;
        stateRobot = STATE_ATTENTE;
    }
}

void SendState(unsigned char stateRobot) {
    int pos = 0;
    unsigned char instant[4];
    instant[pos++] = ((unsigned char) stateRobot);
    instant[pos++] = (unsigned char) (timestamp << 24);
    instant[pos++] = (unsigned char) (timestamp << 16);
    instant[pos++] = (unsigned char) (timestamp << 8);
    instant[pos++] = (unsigned char) (timestamp << 0);
    UartEncodeAndSendMessage(STATE_PROTOCOL, 5, instant);
}

