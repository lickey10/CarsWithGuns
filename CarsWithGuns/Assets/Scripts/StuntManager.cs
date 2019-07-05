using UnityEngine;
using System.Collections;

namespace Scripts
{
    public class StuntManager : MonoBehaviour
    {
        Vector3 rot; //<- Declarated at the top of the script
        bool rotX90 = false;
        bool rotX180 = false;
        bool rotX270 = false;
        bool rotX360 = false;
        bool rotY90 = false;
        bool rotY180 = false;
        bool rotY270 = false;
        bool rotY360 = false;
        bool rotZ90 = false;
        bool rotZ180 = false;
        bool rotZ270 = false;
        bool rotZ360 = false;
        bool showPopupMessage = false;
        string popupMessage = "";
        ArrayList currentStunt = new ArrayList();//the current stunt (spins, flips and rolls) that are being performed.  Ends when we land on the ground. Bonus for landing on wheels
        ArrayList performedStunts = new ArrayList();//alll of the stunts performed on this level
        clsStunt stunt = new clsStunt();//the current stunt object
        GameObject terrain;
        float topAltitude = 0;
        int rollCount = 0;
        int flipCount = 0;
        int spinCount = 0;

        public int Score = 0;

        public ArrayList Stunts
        {
            get { return performedStunts; }
        }

        // Use this for initialization
        void Start()
        {
            terrain = GameObject.FindGameObjectWithTag("Terrain");
        }

        void OnGUI()
        {
            if (showPopupMessage)
                showMessage();
        }

        public void StuntEnded(float AirTime)
        {
            if (AirTime > 1)
            {
                if (stunt == null)
                {
                    stunt = new clsStunt();

                    if(Vector3.Distance(terrain.transform.position, transform.position) > 10)
                        stunt.Name = "Jump";
                }

                if (stunt != null)
                {
                    if (stunt.Name.Length > 0)
                    {
                        stunt.AirTime = AirTime;
                        stunt.Altitude = Vector3.Distance(terrain.transform.position, transform.position);

                        if (stunt.Altitude > topAltitude)
                            topAltitude = stunt.Altitude;

                        currentStunt.Add(stunt);

                        performedStunts.Add(currentStunt);
                    }
                }
            }

            rotX90 = false;
            rotX180 = false;
            rotX270 = false;
            rotX360 = false;
            rot.x = 0;

            rotY90 = false;
            rotY180 = false;
            rotY270 = false;
            rotY360 = false;
            rot.y = 0;

            rotZ90 = false;
            rotZ180 = false;
            rotZ270 = false;
            rotZ360 = false;
            rot.z = 0;

            stunt = null;
            currentStunt.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            //print("localEulerAngles x " + transform.localEulerAngles.x);
            //print("localEulerAngles y " + transform.localEulerAngles.y);
            //print("localEulerAngles z " + transform.localEulerAngles.z);

            //print("eulerAngles x " + transform.eulerAngles.x);
            //print("eulerAngles y " + transform.eulerAngles.y);
            //print("eulerAngles z " + transform.eulerAngles.z);

            float angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0, 0, 0)), transform.rotation);
            //print("angle " + angle);

            //flip
            //if (transform.eulerAngles.x != rot.x)
            //{
            rot.x = transform.eulerAngles.x;

            if (rot.x >= 85 && rot.x < 105 && !rotX90)
            {
                addStunt("Flip_90");
                showPopupMessage = true;
                print("score 90 x");
                rotX90 = true;
                Score += 900;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 900);
            }
            if (rot.x >= 175 && rot.x < 195 && rotX90 && !rotX180)
            {
                addStunt("Flip_180");
                print("score 180 x");
                rotX180 = true;
                Score += 1800;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 1800);
            }
            if (rot.x >= 270 && rot.x < 285 && rotX180 && !rotX270)
            {
                addStunt("Flip_270");
                print("score 270 x");
                rotX270 = true;
                Score += 2700;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 2700);
            }
            if (rot.x > 286 && rot.x < 1 && rotX270 && !rotX360)
            {
                addStunt("Flip_360");
                print("score 360 x");
                rotX360 = true;
                Score += 3600;
                rot.x = 0;
                flipCount++;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 3600);
            }
            //}
            else
            {
                rotX90 = false;
                rotX180 = false;
                rotX270 = false;
                rotX360 = false;
                rot.x = 0;
            }

            //spin
            //if (transform.eulerAngles.y != rot.y)
            //{

            rot.y = transform.eulerAngles.y;

            if (rot.y >= 85 && rot.y < 105 && !rotY90)
            {
                addStunt("Spin_90");
                showPopupMessage = true;
                print("score 90 y");
                rotY90 = true;
                Score += 900;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 900);
            }
            if (rot.y >= 175 && rot.y < 195 && rotY90 && !rotY180)
            {
                addStunt("Spin_180");
                print("score 180 y");
                rotY180 = true;
                Score += 1800;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 1800);
            }
            if (rot.y >= 270 && rot.y < 285 && rotY180 && !rotY270)
            {
                //this is a close as I get to detecting a spin
                addStunt("Spin_270");
                print("score 270 y");
                rotY270 = true;
                Score += 2700;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 2700);
            }
            if (rot.y > 286 && rot.y < 10 && rotY270 && !rotY360)
            {
                addStunt("Spin_360");
                print("score 360 y");
                rotY360 = true;
                Score += 3600;
                rot.y = 0;
                spinCount++;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 3600);
            }
            //}
            else
            {
                rotY90 = false;
                rotY180 = false;
                rotY270 = false;
                rotY360 = false;
                rot.y = 0;
            }

            //roll
            //if (transform.eulerAngles.z != rot.z)
            //{

            rot.z = transform.eulerAngles.z;

            if (rot.z >= 85 && rot.z < 105 && !rotZ90)
            {
                addStunt("Roll_90");
                showPopupMessage = true;
                print("score 90 z");
                rotZ90 = true;
                Score += 900;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 900);
            }
            if (rot.z >= 175 && rot.z < 195 && rotZ90 && !rotZ180)
            {
                addStunt("Roll_180");
                print("score 180 z");
                rotZ180 = true;
                Score += 1800;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 1800);
            }
            if (rot.z >= 270 && rot.z < 285 && rotZ180 && !rotZ270)
            {
                addStunt("Roll_270");
                print("score 270 z");
                rotZ270 = true;
                Score += 2700;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 2700);
            }
            if (rot.z > 286 && rot.z < 10 && rotZ270 && !rotZ360)
            {
                addStunt("Roll_360");
                print("score 360 z");
                rotZ360 = true;
                Score += 3600;
                rot.z = 0;
                rollCount++;
                gamestate.Instance.SetScore(gamestate.Instance.getScore() + 3600);
            }
            //}
            else
            {
                rotZ90 = false;
                rotZ180 = false;
                rotZ270 = false;
                rotZ360 = false;
                rot.z = 0;
            }
        }

        void addStunt(string stuntName)
        {
            if (stunt == null)
                stunt = new clsStunt();

            if (stunt != null)
            {
                stunt.Name = stuntName;
                stunt.Altitude = Vector3.Distance(terrain.transform.position, transform.position);
                currentStunt.Add(stunt);
            }
        }

        void showMessage()
        {
            GUI.Label(new Rect(0, 0, 100, 100), popupMessage);

            //guiText.text = message;
            //guiText.enabled = true;
            //yield return new WaitForSeconds(delay);
            //guiText.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Waypoint")
            {
                GameObject scripts = GameObject.FindWithTag("Scripts");

                if (scripts != null)
                {
                    ScreenManager gameManager = scripts.GetComponent<ScreenManager>();
                    gameManager.ActivateWaypoint(other.gameObject);
                }
            }
        }
    }
}
