#include <xc.h>
#include "timer.h"
#include "IO.h"
#include "PWM.h"
#include "ADC.h"

unsigned char toggle = 0;

// Initialisation d'un timer 32 bits

void InitTimer23(void) {
    T3CONbits.TON = 0; // Stop any 16bit Timer3 operation
    T2CONbits.TON = 0; // Stop any 16/32bit Timer3 operation
    T2CONbits.T32 = 1; // Enable 32bit Timer mode
    T2CONbits.TCS = 0; // Select internal instruction cycle clock
    T2CONbits.TCKPS = 0b01; // Select 1:1 Prescaler
    TMR3 = 0x00; // Clear 32?bit Timer (msw)
    TMR2 = 0x00; // Clear 32?bit Timer (lsw)
    PR3 = 0x0098; // Load 32?bit period value (msw) //0x0262
    PR2 = 0x9680; // Load 32?bit period value (lsw) 0x5A00 
    IPC2bits.T3IP = 0x01; // Set Timer3 Interrupt Priority Level
    IFS0bits.T3IF = 0; // Clear Timer3 Interrupt Flag
    IEC0bits.T3IE = 1; // Enable Timer3 interrupt
    T2CONbits.TON = 1; // Start 32?bit Timer
    /* Example code for Timer3 ISR */
}

// Interruption du timer 32bits sur  2?3

void __attribute__((interrupt, no_auto_psv))_T3Interrupt(void) {
    IFS0bits.T3IF = 0; // Clear Timer3 Interrupt Flag
   
    if (toggle == 0) {
        PWMSetSpeedConsigne(0, MOTEUR_DROIT);
        PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
        toggle = 1;
    } else {
        PWMSetSpeedConsigne(0, MOTEUR_DROIT);
        PWMSetSpeedConsigne(0, MOTEUR_GAUCHE);
        toggle = 0;
    }
}

// Initialisation d'un timer 16 bits

void InitTimer1(void) {
    //Timer1 pour horodater les mesures (1ms)
    T1CONbits.TON = 0; // Disable Timer
    T1CONbits.TCKPS = 0b11; // Prescaler
    // 11 = 1:256 prescale value
    // 10 = 1:64 prescale value
    // 01 = 1:8 prescale value
    // 00 = 1:1 prescale value
    T1CONbits.TCS = 0; // clocksource = internalclock

    int f = 100;
    PR1 = 40000000 / (256 * f);

    IFS0bits.T1IF = 0; // Clear Timer Interrupt Flag
    IEC0bits.T1IE = 1; // Enable Timer interrupt
    T1CONbits.TON = 1; // Enable Timer
}



// Interruption du timer 1

void __attribute__((interrupt, no_auto_psv)) _T1Interrupt(void) {
    IFS0bits.T1IF = 0;
  
    PWMUpdateSpeed();
    ADC1StartConversionSequence();
}
