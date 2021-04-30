#include "main.h"
#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include <math.h>
#include <libpic30.h>
#include "ChipConfig.h"
#include "IO.h"
#include "timer.h"
#include "PWM.h"
#include "ADC.h"
#include "UART.h"
#include "CB_RX1.h"
#include "msgDecoder.h"
#include "QEI.h"

int main(void) {
    /**************** Initialisation de l'oscillateur *************************/
    InitOscillator();

    /**************** Configuration des entrées/sorties ***********************/
    InitIO();
    LED_BLANCHE = 0;
    LED_BLEUE = 0;
    LED_ORANGE = 0;

    /**************** Initialisation des timers *******************************/
    InitTimer1(300);
    InitTimer4(100);
    
    /**************** Initialisation de la PWM ********************************/
    InitPWM();

    /**************** Initialisation de l'ADC *********************************/
    InitADC1();

    /**************** Initialisation de UART **********************************/
    InitUART();

    /**************** Initialisation des modules QEI **************************/
    InitQEI1();
    InitQEI2();

    /**************** Boucle Principale ***************************************/
    while (1) {
        unsigned char i;
        for (i = 0; i < CB_RX1_GetDataSize(); i++) {
            unsigned char c = CB_RX1_Get();
            UartDecodeMessage(c);
            LED_BLEUE = !LED_BLEUE;
        }
    }//fin boucle infinie
} // fin main
