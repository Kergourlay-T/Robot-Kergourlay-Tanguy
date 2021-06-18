#ifndef ASSERVISSEMENT_H
#define	ASSERVISSEMENT_H

void SetUpPiLimitGains();

void SetUpPiAsservissementVitesseAngulaire(double, double);
double CorrecteurVitesseAngulaire (double);

void SetUpPiAsservissementVitesseLineaire(double Ku, double Tu);
double CorrecteurVitesseLineaire(double e);

#endif	/* ASSERVISSEMENT_H */


