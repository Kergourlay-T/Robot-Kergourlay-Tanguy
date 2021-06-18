#include <xc.h>
#include "Asservissement.h"
#include "QEI.h"
#include "timer.h"
#include "Robot.h"

double Te = 1 / FREQ_ECH_QEI;

/********************** CORRECTEUR VITESSE ANGULAIRE ****************************/
double eAng0, eAng1, eAng2; //valeurs de l'entrée du correcteur gauche à t, t-1, t-2
double sAng0, sAng1, sAng2; //valeurs de la sortie du correcteur gauche à t, t-1, t-2

double alphaAng;
double betaAng;
double deltaAng;

void SetUpPiAsservissementVitesseAngulaire(double Ku, double Tu) {
    //Reglage de Ziegler Nichols sans depassements : un tout petit peu mou
    //Implémente un correcteur PI
    double Kp = 0.45 * Ku; 
    double Ti = 0.83 * Tu; 
    double Td = 0; // pas une erreur
    double Ki = Kp / Ti;
    double Kd = Kp * Td;
    alphaAng = 0; // modifier
    betaAng = 0; // modifier
    deltaAng = 0; // modifier 
}

double CorrecteurVitesseAngulaire(double e) {
    eAng2 = eAng1;
    eAng1 = eAng0;
    eAng0 = e;
    sAng1 = sAng0;
    sAng0 = sAng1 + eAng0 * alphaAng + eAng1 * betaAng + eAng2 * deltaAng;
    return sAng0;
}


/********************** CORRECTEUR VITESSE LINEAIRE ****************************/
double eVit0, eVit1, eVit2; //valeurs de l'entrée du correcteur gauche à t, t-1, t-2
double sVit0, sVit1, sVit2; //valeurs de la sortie du correcteur gauche à t, t-1, t-2

double alphaVit;
double betaVit;
double deltaVit;

void SetUpPiAsservissementVitesseLineaire(double Ku, double Tu) {
    //Reglage de Ziegler Nichols sans depassements : un tout petit peu mou
    //Implémente un correcteur PI
    double Kp = 0.45 * Ku; 
    double Ti = 0.83 * Tu; 
    double Td = 0; // pas une erreur
    double Ki = Kp / Ti;
    double Kd = Kp * Td;
    alphaVit = 0; // modifier
    betaVit = 0; // modifier
    deltaVit = 0; // modifier 
}

double CorrecteurVitesseLineaire(double e) {
    eVit2 = eVit1;
    eVit1 = eVit0;
    eVit0 = e;
    sVit1 = sVit0;
    sVit0 = sVit1 + eVit0 * alphaVit + eAng1 * betaVit + eVit2 * deltaVit;
    return sAng0;
}


void SetUpPiLimitGains(){
    //Definition of proportional earnings limits
    robotState.KpLineaireMax = 0;
    robotState.KiLineaireMax = 0;
    robotState.KdLineaireMax = 0; //not use
    robotState.KpAngulaireMax = 0;
    robotState.KiAngulaireMax = 0;
    robotState.KdAngulaireMax = 0; //not use
}


