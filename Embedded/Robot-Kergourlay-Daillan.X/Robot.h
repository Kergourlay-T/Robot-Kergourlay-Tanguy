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
//float xCoordonneeTelemetreCentre=0;
//float yCoordonneeTelemetreCentre=0;
//float xCoordonneeTelemetreDroit= 12,287; // cos(75)*15
//float yCoordonneeTelemetreDroit= 14,489; //sin(75)*15
//float xCoordonneeTelemetreGauche = -12,287 // -cos(75)*15;
//float yCoordonneeTelemetreGauche = -14,489 //-sin(75)*15;
//float xCoordonneeTelemetreDroitExtremite = 12,287; //cos(35)*15;
//float yCoordonneeTelemetreDroitExtremite = 8,603; //sin(35)*15;
//float xCoordonneeTelemetreGaucheExtremite = -12,287; //-cos(35)*15;
//float yCoordonneeTelemetreGaucheExtremite = -8,603; //-sin(35)*15;
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