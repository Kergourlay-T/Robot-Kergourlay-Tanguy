#ifndef ROBOT_H
#define ROBOT_H

typedef struct robotStateBITS {
    union {
        struct {
            unsigned char taskEnCours;
            float vitesseGaucheConsigne;
            float vitesseGaucheCommandeCourante;
            float vitesseDroiteConsigne;
            float vitesseDroiteCommandeCourante;
            float distanceTelemetreDroit;
            float distanceTelemetreGauche;
            float distanceTelemetreCentre;
            float distanceTelemetreDroitExtremite;
            float distanceTelemetreGaucheExtremite;
        };
    };
} ROBOT_STATE_BITS;
extern volatile ROBOT_STATE_BITS robotState;

//float xCoordonneeCentreRobot=0;
//float yCoordonneeCentreRobot=0;
//
//float xCoordonneeReferenceGauche = -30;
//float yCoordonneeReferenceGauche = 0;
//float xCoordonneeReferenceDroit = 30;
//float yCoordonneeReferenceDroit = 0;
//
//float DistanceReferenceCentreRobot = 30;
//float DistanceReferenceCritiqueCentreRobot = 10;
//
//float xCoordonneeTelemetreCentre;
//float yCoordonneeTelemetreCentre;
//float xCoordonneeTelemetreDroit;
//float yCoordonneeTelemetreDroit;
//float xCoordonneeTelemetreGauche;
//float yCoordonneeTelemetreGauche;
//float xCoordonneeTelemetreDroitExtremite;
//float yCoordonneeTelemetreDroitExtremite;
//float xCoordonneeTelemetreGaucheExtremite;
//float yCoordonneeTelemetreGaucheExtremite;
//
//float xCoordonneePointGauche;
//float yCoordonneePointGauche;
//float xCoordonneePointDroit;
//float yCoordonneePointDroit;
//float xCoordonneePointGaucheExtremite;
//float yCoordonneePointGaucheExtremite;
//float xCoordonneePointDroitExtremite;
//float yCoordonneePointDroitExtremite;
//
//float distanceCentreDroitExtremite;
//float distanceCentreDroit;
//float distanceCentreGaucheExtremite;
//float distanceCentreGauche;
//
//float distanceReferencePointDroitExtremite;
//float distanceReferencePointDroit;
//float distanceReferencePointGaucheExtremite;
//float distanceReferencePointGauche;
//
//float angleTelemetreDroitExtremite;
//float angleTelemetreDroit;
//float angleTelemetreGaucheExtremite;
//float angleTelemetreGauche;
//float angleReferenceDroitExtremite; 
//float angleReferenceDroit; 
//float  angleReferenceGaucheExtremite;
//float angleReferenceGauche;

#endif /* ROBOT_H */