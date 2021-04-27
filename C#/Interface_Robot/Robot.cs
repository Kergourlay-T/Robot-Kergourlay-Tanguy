using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Robot
{
    public class Robot
    {
        public string receivedText = "";
        public float distanceTelemetreDroit;
        public float distanceTelemetreCentre;
        public float distanceTelemetreGauche;
        public Queue<byte> byteListReceived = new Queue<byte>();

        public float timestamp;
        public float positionXOdo;
        public float postionYOdo;
        public float angleRadianOdo;
        public float linearSpeedOdo;
        public float angularSpeedOdo;

        public Robot()
        {


        }
    }
}
