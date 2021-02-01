/* 
 * File:   PWM.h
 * Author: TP-EO-1
 *
 * Created on 1 février 2021, 14:01
 */

#ifndef PWM_H
#define	PWM_H

//Définitions des moteurs
#define MOTEUR_DROIT 0
#define MOTEUR_GAUCHE 1

void InitPWM(void);
//void PWMSetSpeed(float vitesseEnPourcents, int moteur);
void PWMSetSpeedConsigne(float vitesseEnPourcents, int moteur);
void PWMUpdateSpeed(void);        
        
#endif	/* PWM_H */

