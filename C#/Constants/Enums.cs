using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Enums
    {
        public enum Functions : ushort
        {
            CHECK_INSTRUCTION_ROBOT_TO_GUI = 0x0010,
            LED_ROBOT_TO_GUI = 0x0020,
            LED_GUI_TO_ROBOT = 0x0021,
            TELEMETER_ROBOT_TO_GUI = 0x0030,
            MOTOR_ROBOT_TO_GUI = 0x0040,
            MOTOR_GUI_TO_ROBOT = 0x0041,
            ROBOT_STATE_ROBOT_TO_GUI = 0x0050,
            ROBOT_STATE_GUI_TO_ROBOT = 0x0051,
            MANUAL_CONTROL_ROBOT_TO_GUI = 0x0052,
            MANUAL_CONTROL_GUI_TO_ROBOT = 0x0053,
            POSITION_DATA_ROBOT_TO_GUI = 0x0060,
            POSITION_DATA_GUI_TO_ROBOT = 0x0061,
            RESET_POSITION_GUI_TO_ROBOT = 0x0062,
            MESSAGE_ROBOT_TO_GUI = 0x0080,
            MESSAGE_GUI_TO_ROBOT = 0x0081
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

        public enum AsservissementMode
        {
            Disabled = 0,
            Polar = 1,
            Independant = 2,
        }
    }

}
