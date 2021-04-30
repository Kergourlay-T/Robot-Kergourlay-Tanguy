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
            float vitesseLineaireFromOdometry;
            float vitesseAngulaireFromOdometry;

            float xPosFromOdometry_1;
            float xPosFromOdometry;
            float yPosFromOdometry_1;
            float yPosFromOdometry;
            float angleRadianFromOdometry_1;
            float angleRadianFromOdometry;
        };
    };
} ROBOT_STATE_BITS;
extern volatile ROBOT_STATE_BITS robotState;

#endif /* ROBOT_H */