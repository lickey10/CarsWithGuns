using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ScreenManager : MonoBehaviour
    {

        public GameObject CurrentWaypoint;
        public GameObject LastWaypoint;
        public GameObject[] Waypoints = new GameObject[4];
        public int NumberOfLaps = 5;
        public float TimeLimitForLevel = 180;//seconds
        public static int Kills = 0;
        public static int DeathByCar = 0;
        public static float TopSmashDamage = 0;
        public int CurrentLapNumber = 1;
        public int TopSpeed = 0;
        public int Score = 0;
        public GameObject ScrollingMenu;
        public GameObject TestCarChange;
        public Object LoadingScreen = null;
        public string SceneNameToLoad = "";

        private int numberOfLapsComplete = 0;
        private GameObject[] targetObjects;
        private int speed = 0;
        GameObject scripts;
        bool swipeDown = false;
        bool swipeUp = false;
        bool swipeLeft = false;
        bool swipeRight = false;
        bool doubleTap = false;
        ResetCar resetCar;
        public ResetCar ResetCarScript;
        float smashDamage = 0;
        private int coins = 0;
        private int totalNumberOfTargets = 0;
        private GUIStyle customGuiStyle;
        [SerializeField]
        private FadeSprite m_blackScreenCover;
        [SerializeField]
        private float m_minDuration = 1.5f;
        private bool sceneIsLoading = false;
        public UnityEngine.UI.Slider slider;
        AsyncOperation asyncLoad = null;

        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;

                if (speed > TopSpeed)
                    TopSpeed = speed;
            }
        }

        public float SmashDamage
        {
            get { return smashDamage; }
            set
            {
                smashDamage = value;

                if (smashDamage > TopSmashDamage)
                    TopSmashDamage = smashDamage;
            }
        }

        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;

                DBStoreController.singleton.balance += value;
            }
        }

        // Use this for initialization
        void Start()
        {
            CurrentWaypoint = Waypoints[0];
            LastWaypoint = Waypoints[0];

            scripts = GameObject.FindWithTag("Scripts");
            resetCar = GameObject.FindWithTag("Player").GetComponent<ResetCar>();

            if (resetCar == null)
                resetCar = ResetCarScript;

            ResetCar();

            totalNumberOfTargets = GameObject.FindGameObjectsWithTag("TargetObject").Length;
            if (totalNumberOfTargets == null)
                totalNumberOfTargets = 0;

            gamestate.Instance.StartTimer(TimeLimitForLevel);

            Time.timeScale = 1;
        }

        // Update is called once per frame
        void Update()
        {
            targetObjects = GameObject.FindGameObjectsWithTag("TargetObject");
            //if (targetObjects.Length <= 0 || gamestate.Instance.Timer < 0)
            //{
            //    print("Level is complete");
            //    Application.LoadLevel("levelComplete");
            //}

            if (gamestate.Instance.Timer <= 0)//the game is over
                Application.LoadLevel("gameover");

            if (Input.GetKeyDown("space"))
            {
                //Instantiate(ScrollingMenu, new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z), GameObject.FindGameObjectWithTag("Player").transform.rotation);
                ////GameObject.FindGameObjectWithTag("ScrollingMenu").SetActive(true);
                //GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = true;
                //GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = true;

                ChangeCurrentVehicle(TestCarChange);
                //TestCarChange;

            }

            if (Input.GetKeyDown("c"))
            {
                GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = true;
                GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = true;
                //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;

                Time.timeScale = 0;
            }

            if (asyncLoad != null && slider != null)
            {
                slider.gameObject.SetActive(true);
                slider.value = asyncLoad.progress;
            }
            else if (slider != null)
            {
                slider.gameObject.SetActive(false);
            }
        }

        protected virtual void OnEnable()
        {
#if MOBILE_INPUT
            // Hook the OnFingerSet event
            Lean.LeanTouch.OnFingerSwipe += OnFingerSwipe;
            Lean.LeanTouch.OnMultiTap += OnMultiTap;
#endif
        }

        protected virtual void OnDisable()
        {
#if MOBILE_INPUT
            // Unhook the OnFingerSet event
            Lean.LeanTouch.OnFingerSwipe -= OnFingerSwipe;
            Lean.LeanTouch.OnMultiTap -= OnMultiTap;
#endif
        }

        //this is set up for testing and can be removed
        public void ChangeCurrentVehicle(GameObject NewVehiclePrefab)
        {
            GameObject currentVehiclePrefab = GameObject.FindGameObjectWithTag("Car GO");
            Vector3 carPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            Quaternion carRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
            Destroy(currentVehiclePrefab);
            PlayerWeapons pw = GameObject.FindWithTag("WeaponCamera").GetComponent<PlayerWeapons>();
            Destroy(pw);

            Instantiate(NewVehiclePrefab, carPosition, carRotation);

            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            //GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = false;
            //GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = false;

            Time.timeScale = 1;

            //Destroy(GameObject.FindGameObjectWithTag("ScrollingMenu"));
        }

        void OnGUI()
        {
            //GUI.skin.button.normal.background = null;
            //GUI.skin.button.hover.background = null;
            //GUI.skin.button.active.background = null;
            //GUIStyle style = new GUIStyle(GUI.skin.button);
            //GUIStyle style = new GUIStyle("box");


            int topBarHeight = 0;

#if MOBILE_INPUT
            topBarHeight = 30;

            customGuiStyle = new GUIStyle();
            customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
            //customGuiStyle.active.textColor = Color.red; // not working
            //customGuiStyle.hover.textColor = Color.blue; // not working
            customGuiStyle.normal.textColor = Color.white;
            customGuiStyle.fontSize = 25;
            customGuiStyle.stretchWidth = true; // ---
            customGuiStyle.stretchHeight = true; // not working, since backgrounds aren't showing
            //customGuiStyle.alignment = TextAnchor.MiddleCenter;
#else
            topBarHeight = 22;

            customGuiStyle = new GUIStyle();
            customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
            //customGuiStyle.active.textColor = Color.red; // not working
            //customGuiStyle.hover.textColor = Color.blue; // not working
            //customGuiStyle.normal.textColor = Color.green;
            customGuiStyle.normal.textColor = Color.white;
            customGuiStyle.fontSize = 20;
            customGuiStyle.stretchWidth = true; // ---
            customGuiStyle.stretchHeight = true; // not working, since backgrounds aren't showing
                                                 //customGuiStyle.alignment = TextAnchor.MiddleCenter;
#endif

            GUI.Box(new Rect(0, 0, Screen.width, topBarHeight), "");

            //display targets
            if(totalNumberOfTargets == 0)
                GUI.Label(new Rect(0, 0, 100, topBarHeight), "Targets:0/0" + totalNumberOfTargets.ToString(), customGuiStyle);
            else
                GUI.Label(new Rect(0, 0, 100, topBarHeight), "Targets:" + (totalNumberOfTargets - targetObjects.Length).ToString() + "/" + totalNumberOfTargets.ToString(), customGuiStyle);

            //display laps
            GUI.Label(new Rect(150, 0, 100, topBarHeight), "Laps:" + CurrentLapNumber.ToString() + "/" + NumberOfLaps.ToString(), customGuiStyle);

            //display kills
            //GUI.Label(new Rect(175, 0, 100, 20), "Kills:" + Kills.ToString());

            //display smash damage
            //GUI.Label(new Rect(250, 25, 100, 20), "Smash Damage:" + SmashDamage.ToString());

            //display speed
            GUI.Label(new Rect(Screen.width / 2 + 25, 0, 100, topBarHeight), "Speed:" + Speed, customGuiStyle);

            //display score
            GUI.Label(new Rect(Screen.width - 300, 0, 150, topBarHeight), "Score:" + gamestate.Instance.getScore(), customGuiStyle);

            //display coins
            GUI.Label(new Rect(Screen.width - 150, 0, 150, topBarHeight), "Coins:" + Coins, customGuiStyle);

            //display timer
            string minutes = Mathf.Floor(gamestate.Instance.Timer / 60).ToString("00");
            string seconds = Mathf.Floor(gamestate.Instance.Timer % 60).ToString("00");
            GUI.Label(new Rect(Screen.width / 2 - 125, 0, 100, topBarHeight), "Timer:" + minutes + ":" + seconds, customGuiStyle);

#if MOBILE_INPUT
            //if(GUI.Button(new Rect(0, 40, 100, 40), "Reset", customGuiStyle))
            //{
                
            //}
#endif

            //if (swipeLeft)
            //{
            //    GUI.Label(new Rect(300, 200, 100, 100), "calling SelectPreviousWeapon");
            //}

            //if(swipeRight)
            //    GUI.Label(new Rect(300, 200, 100, 100), "calling SelectNextWeapon");

            //if(swipeUp)
            //    GUI.Label(new Rect(300, 200, 100, 100), "swipe up");

            //if(swipeDown)
            //    GUI.Label(new Rect(300, 200, 100, 100), "swipe down");

            //if(doubleTap)
            //    GUI.Label(new Rect(500, 100, 100, 100), "DOUBLETAP");
        }

        public void ResetCar()
        {
            if (resetCar != null)
                resetCar.ResetTheCar();
        }

        public void OnMultiTap(int fingerCount)
        {
            doubleTap = true;


            //if(resetCar != null)
            //    resetCar.ResetTheCar();

            //Debug.Log("The screen was just tapped by " + fingerCount + " finger(s)");
        }

        public void OnFingerSwipe(Lean.LeanFinger finger)
        {
            // Store the swipe delta in a temp variable
            var swipe = finger.SwipeDelta;
            doubleTap = false;

            if (swipe.x < -Mathf.Abs(swipe.y))
            {
                swipeLeft = true;
                swipeRight = false;
                swipeDown = false;
                swipeUp = false;
                //scripts.BroadcastMessage("SelectNextWeapon");

                if (finger.Age > .2 && finger.StartScreenPosition.x > 300)
                {
                    PlayerWeapons playerWeapons = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<PlayerWeapons>();
                    int newWeaponIndex = playerWeapons.selectedWeapon + 1;
                    if (playerWeapons.weapons[newWeaponIndex] == null)
                        newWeaponIndex = 0;

                    playerWeapons.SelectWeapon(newWeaponIndex);
                    playerWeapons.selectedWeapon = newWeaponIndex;
                    playerWeapons.weapons[newWeaponIndex].SetActive(true);
                }

                //print("You swiped left!");
            }

            if (swipe.x > Mathf.Abs(swipe.y))
            {
                swipeLeft = false;
                swipeRight = true;
                swipeDown = false;
                swipeUp = false;

                if (finger.Age > .2 && finger.StartScreenPosition.x > 300)
                {
                    //scripts.BroadcastMessage("SelectPreviousWeapon");
                    PlayerWeapons playerWeapons = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<PlayerWeapons>();

                    int newWeaponIndex = playerWeapons.selectedWeapon - 1;

                    if (newWeaponIndex < 0)
                    {
                        for (int x = playerWeapons.weapons.Length - 1; x >= 0; x--)
                        {
                            if (playerWeapons.weapons[x] != null)
                            {
                                newWeaponIndex = x;
                                break;
                            }
                        }
                    }

                    playerWeapons.SelectWeapon(newWeaponIndex);
                    playerWeapons.selectedWeapon = newWeaponIndex;
                    playerWeapons.weapons[newWeaponIndex].SetActive(true);
                }

                //print("You swiped right!");
            }

            if (swipe.y < -Mathf.Abs(swipe.x))
            {

                swipeLeft = false;
                swipeRight = false;
                swipeDown = true;
                swipeUp = false;

                if (finger.Age > .2 && finger.StartScreenPosition.y > 300)
                {
                    Instantiate(ScrollingMenu, new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z), GameObject.FindGameObjectWithTag("Player").transform.rotation);
                    //GameObject.FindGameObjectWithTag("ScrollingMenu").SetActive(true);
                    GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = true;
                    GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = true;
                    //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;


                    Time.timeScale = 0;
                }

                //print("You swiped down!");
            }

            if (swipe.y > Mathf.Abs(swipe.x))
            {

                swipeLeft = false;
                swipeRight = false;
                swipeDown = false;
                swipeUp = true;

                if (finger.Age > .2 && finger.StartScreenPosition.y > 300)
                {
                    GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = true;
                    GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = true;
                    //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;

                    Time.timeScale = 0;
                }

                //print("You swiped up!");
            }
        }

        public void ActivateWaypoint(GameObject currentWaypoint)
        {
            if (Waypoints.Length > 0 && CurrentWaypoint == currentWaypoint)
            {
                bool foundCurrentWaypoint = false;

                //if (Waypoints[Waypoints.Length - 1] == CurrentWaypoint)//this is the last waypoint which makes a complete lap
                //{
                //    numberOfLapsComplete++;
                //    foundCurrentWaypoint = true;
                //    CurrentLapNumber++;

                //    if (CurrentLapNumber > NumberOfLaps)//level is complete
                //    {
                //        print("Level is complete");
                //        Application.LoadLevel("levelComplete");
                //    }
                //}

                if (Waypoints[Waypoints.Length - 1] == CurrentWaypoint)//this is the last waypoint which makes a complete lap
                {
                    numberOfLapsComplete++;
                    foundCurrentWaypoint = true;
                    CurrentLapNumber++;
                    CurrentWaypoint = Waypoints[0];

                    if (CurrentLapNumber > NumberOfLaps)//level is complete
                    {
                        print("Level is complete");
                        Application.LoadLevel("levelComplete");
                    }
                }

                foreach (GameObject waypoint in Waypoints)
                {
                    MapMarker mapMarker = waypoint.GetComponent<MapMarker>();

                    if (foundCurrentWaypoint)
                    {
                        mapMarker.isActive = true;
                        LastWaypoint = CurrentWaypoint;
                        CurrentWaypoint = waypoint;
                        foundCurrentWaypoint = false;
                    }
                    else
                    {
                        if (waypoint == CurrentWaypoint)
                            foundCurrentWaypoint = true;

                        mapMarker.isActive = false;
                    }
                }
            }
        }

        public IEnumerator LoadSceneAsync2(string sceneName)
        {
            SceneNameToLoad = sceneName;

            // Fade to black
            yield return StartCoroutine(m_blackScreenCover.FadeIn());

            

            // Load loading screen
            //yield return StartCoroutine(LoadAsyncScene(LoadingScreen.name, LoadSceneMode.Single));
            //yield return SceneManager.LoadSceneAsync(LoadingScreen.name);
            //yield return Application.LoadLevelAsync(LoadingScreen.name);

            // !!! unload old screen (automatic)

            // Fade to loading screen
            yield return StartCoroutine(m_blackScreenCover.FadeOut());

            float endTime = Time.time + m_minDuration;

            // Load level async
            //yield return StartCoroutine(LoadAsyncScene(sceneName, LoadSceneMode.Additive));
            //yield return Application.LoadLevelAdditiveAsync(sceneName);

            while (Time.time < endTime)
                yield return null;

            // Play music or perform other misc tasks

            // Fade to black
            yield return StartCoroutine(m_blackScreenCover.FadeIn());

            // !!! unload loading screen
            LoadingSceneManager.UnloadLoadingScene();

            // Fade to new screen
            yield return StartCoroutine(m_blackScreenCover.FadeOut());
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            throw new System.NotImplementedException();
        }

        //IEnumerator LoadAsyncScene(string sceneName, LoadSceneMode loadSceneMode)
        //{
        //    // The Application loads the Scene in the background as the current Scene runs.
        //    // This is particularly good for creating loading screens.
        //    // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        //    // a sceneBuildIndex of 1 as shown in Build Settings.

        //    //asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        //    //asyncLoad.allowSceneActivation = false;

        //    //// Wait until the asynchronous scene fully loads
        //    //while ((!asyncLoad.isDone) && (asyncLoad.progress <= 0.9f))
        //    //{
        //    //    yield return null;
        //    //}

        //    //asyncLoad.allowSceneActivation = true;

        //    //// now activate the scene
        //    //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        //}
    }
}
