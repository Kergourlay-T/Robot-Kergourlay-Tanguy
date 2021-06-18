#ifndef ROBOT_H
#define ROBOT_H

typedef struct robotStateBITS {

    union {

        struct {
            unsigned char taskEnCours;

            float vitesseGaucheConsigne;
            float vitesseGaucheCommandeCourante;
            float vitesseGaucheErreure;

            float vitesseDroiteConsigne;
            float vitesseDroiteCommandeCourante;
            float vitesseDroiteErreure;

            float distanceTelemetreDroit;
            float distanceTelemetreGauche;
            float distanceTelemetreCentre;
            float distanceTelemetreDroitExtremite;
            float distanceTelemetreGaucheExtremite;

            float vitesseDroitFromOdometry;
            float vitesseGaucheFromOdometry;

            double distanceGaucheFromOdometry;
            double distanceDroitFromOdometry;

            float xPosFromOdometry_1;
            float xPosFromOdometry;
            float yPosFromOdometry_1;
            float yPosFromOdometry;
            float angleRadianFromOdometry_1;
            float angleRadianFromOdometry;

            double KpLineaire;
            double KiLineaire;
            double KdLineaire;
            double KpLineaireMax;
            double KiLineaireMax;
            double KdLineaireMax;


            double KpAngulaire;
            double KiAngulaire;
            double KdAngulaire;
            double KpAngulaireMax;
            double KiAngulaireMax;
            double KdAngulaireMax;

            double vitesseLineaireFromOdometry;
            double vitesseLineaireConsigne;
            double vitesseLineaireCommande;
            double vitesseLineaireErreur;
            double vitesseLineaireCorrection;
            double CorrectionLineaireKp;
            double CorrectionLineaireKi;
            double CorrectionLineaireKd;

            double vitesseAngulaireFromOdometry;
            double vitesseAngulaireConsigne;
            double vitesseAngulaireCommande;
            double vitesseAngulaireErreur;
            double vitesseAngulaireCorrection;
            double CorrectionAngulaireKp;
            double CorrectionAngulaireKi;
            double CorrectionAngulaireKd;
        };
    };
} ROBOT_STATE_BITS;
extern volatile ROBOT_STATE_BITS robotState;

#endif /* ROBOT_H */