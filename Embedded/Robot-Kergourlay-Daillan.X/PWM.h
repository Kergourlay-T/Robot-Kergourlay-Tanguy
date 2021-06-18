#ifndef PWM_H
#define	PWM_H

#define PWMPER 40.0 //échantillonage moteur

//Définitions des moteurs
#define MOTEUR_DROIT 0
#define MOTEUR_GAUCHE 1

//Definition of coefficients for odometry
#define COEFF_VITESSE_LINEAIRE_PERCENT 1/25.
#define COEFF_VITESSE_ANGULAIRE_PERCENT 1/50.

//Prototype fonctions
void InitPWM(void);
void PWMSetSpeed(float vitesseEnPourcents, int moteur);
void PWMSetSpeedConsigne(float vitesseEnPourcents, int moteur);
void PWMUpdateSpeed(void);
void PWMSetSpeedConsignePolaire(void);

void SendSpeed(void);

#endif	/* PWM_H */

