using UnityEngine;
using System.Collections;

namespace Scripts
{
    public class clsStunt : MonoBehaviour
    {
        private ArrayList stunts;
        private float airTime;
        private string stuntName = "";
        private float altitude = 0;

        public string Name
        {
            get { return stuntName; }
            set { stuntName = value; }
        }

        public ArrayList Stunts
        {
            get { return stunts; }

            set
            {
                stunts = value;
            }
        }

        public float AirTime
        {
            get { return airTime; }

            set
            {
                airTime = value;
            }
        }

        public float Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }
    }
}
