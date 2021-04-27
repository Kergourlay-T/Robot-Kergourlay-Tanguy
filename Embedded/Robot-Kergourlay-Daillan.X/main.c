#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include <math.h>
#include <libpic30.h>
#include "ChipConfig.h"
#include "IO.h"
#include "timer.h"
#include "PWM.h"
#include "Robot.h"
#include "ADC.h"
#include "main.h"
#include "Utilities.h"
#include "UART.h"
#include "CB_TX1.h"
#include "CB_RX1.h"
#include "UART_Protocol.h"
#include "QEI.h"
#include "RobotControlState.h"

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
    InitTimer4(100);

    /***************************************************************************************************/
    //Initialisation du PWM
    /****************************************************************************************************/
    InitPWM();

    /***************************************************************************************************/
    //Initialisation du ADC
    /****************************************************************************************************/
    InitADC1();

    /***************************************************************************************************/
    //Initialisation du UART
    /****************************************************************************************************/
    InitUART();

    /***************************************************************************************************/
    //Initialisation des modules QEI
    /****************************************************************************************************/
    InitQEI1();
    InitQEI2();

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
            float Distances[5] = {robotState.distanceTelemetreDroitExtremite, robotState.distanceTelemetreDroit,
                robotState.distanceTelemetreCentre, robotState.distanceTelemetreGauche, robotState.distanceTelemetreGaucheExtremite};
            // Détermine la vitesse à adopter en fonction de la distance à l'objet le plus proche
            robotState.speedAdaptedToDistances = MinDistance(Distances) - 10;
            if (robotState.speedAdaptedToDistances > MAXIMUM_SPEED)
                robotState.speedAdaptedToDistances = MAXIMUM_SPEED;
            else if (robotState.speedAdaptedToDistances < MINIMUM_SPEED)
                robotState.speedAdaptedToDistances = MINIMUM_SPEED;
        }

        int i;
        for (i = 0; i < CB_RX1_GetDataSize(); i++) {
            CB_RX1_Get();
        }
        SendRobotInformations();
        SendPositionData();
         SendSpeed();
        __delay32(4000000);
    }//fin boucle infinie
} // fin main
