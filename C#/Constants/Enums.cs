using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public enum Equipe
    {
        Jaune,
        Bleue,
    }

    public enum GameMode
    {
        RoboCup,
        Eurobot,
        Cachan,
        Demo
    }

    public enum PlayingSide
    {
        Left,
        Right
    }

    public enum stateRobot : ushort
    {
        STATE_ATTENTE = 0,
        STATE_ATTENTE_EN_COURS = 1,

        STATE_AVANCE = 2,
        STATE_AVANCE_EN_COURS = 3,

        STATE_TOURNE_GAUCHE = 4,
        STATE_TOURNE_GAUCHE_EN_COURS = 5,
        STATE_TOURNE_DROITE = 6,
        STATE_TOURNE_DROITE_EN_COURS = 7,

        STATE_TOURNE_SUR_PLACE_GAUCHE = 8,
        STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS = 9,
        STATE_TOURNE_SUR_PLACE_DROITE = 10,
        STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS = 11,

        STATE_ARRET = 12,
        STATE_ARRET_EN_COURS = 13,
        STATE_RECULE = 14,
        STATE_RECULE_EN_COURS = 15,

        STATE_COULOIR_A_GAUCHE = 16,
        STATE_COULOIR_A_GAUCHE_EN_COURS = 17,
        STATE_COULOIR_A_DROITE = 18,
        STATE_COULOIR_A_DROITE_EN_COURS = 19,

        STATE_TOURNE_GAUCHE_LEGER = 20,
        STATE_TOURNE_GAUCHE_LEGER_EN_COURS = 21,
        STATE_TOURNE_DROITE_LEGER = 22,
        STATE_TOURNE_DROITE_LEGER_EN_COURS = 23
    }
}
