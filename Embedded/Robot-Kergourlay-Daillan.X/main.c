#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include "ChipConfig.h"
#include "IO.h"
#include "timer.h"
#include "PWM.h"
#include "Robot.h"
#include "ADC.h"

int main (void){
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
robotState.vitesseDroiteCommandeCourante = 0;
robotState.vitesseGaucheCommandeCourante = 0;
InitTimer23();
InitTimer1();


/***************************************************************************************************/
//Initialisation du PWM
/****************************************************************************************************/
InitPWM();

/***************************************************************************************************/
//Initialisation du ADC
/****************************************************************************************************/
InitADC1();
unsigned int ADCValue0, ADCValue1, ADCValue2; 
unsigned int* result;

/****************************************************************************************************/
// Boucle Principale
/****************************************************************************************************/
while(1){
    // LED_BLANCHE = !LED_BLANCHE;
    
    
    if (ADCIsConversionFinished() == 1){
        ADCClearConversionFinishedFlag();
        result = ADCGetResult();

       unsigned int * result = ADCGetResult ();
       float volts = ((float) result [2] )*3.3/4096*3.2;
      
       robotState.distanceTelemetreDroit = 34/volts-5;
         if(robotState.distanceTelemetreDroit > 420){
            LED_ORANGE = 1;
        }
        else{
            LED_ORANGE = 0;
        }
       
       volts = ((float)result[1])*3.3/4096*3.2;
       robotState.distanceTelemetreCentre = 34/volts-5;
        if(robotState.distanceTelemetreCentre >420){
            LED_BLEUE = 1;
        }
        else{
            LED_BLEUE = 0;
        }
       
       
       volts = ((float)result[0])*3.3/4096*3.2 ;
       robotState.distanceTelemetreGauche = 34/volts-5;
        if( robotState.distanceTelemetreGauche > 420){
            LED_BLANCHE = 1;
        }
        else{
            LED_BLANCHE = 0;
        }
    }
    
    //520 à 540 20cm
    //280 à 300 40cm
        
} // fin main


}