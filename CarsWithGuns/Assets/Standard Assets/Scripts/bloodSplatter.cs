using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bloodSplatter : MonoBehaviour {
    public static bool blood = false;

    public Sprite noBlood; //this is your blank texture
    public Sprite[] hit; // and this is the blood splatter texture

    private bool fadeBlood = false;
    float fadeValue = 1;
    float currentTime = 0;
    float timeItTakesToFade = 1;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!blood)
            noSplatter();


        if (blood)
            StartCoroutine(splatter());

        if (fadeBlood)
        {

            currentTime += Time.deltaTime;

            if (currentTime <= timeItTakesToFade)
            {
                fadeValue = 1 - (currentTime / timeItTakesToFade);
                //Fade.use.Alpha(hit.material, 1.0, 0.0, 1.0);
                //hit.color = new Color(1, 1, 1, fadeValue);
            }
            else
            {
                fadeBlood = false;
            }
        }
    }

    void startFade(float howLong)
    {
        currentTime = 0;
        timeItTakesToFade = howLong;
        fadeBlood = true;
    }

    IEnumerator splatter()
    {
        if (blood)
        {
            gameObject.transform.localScale = new Vector3(1, 1);
            GetComponent<Image>().sprite = hit[Random.Range(0,hit.Length-1)];
            yield return new WaitForSeconds(1f);
            
            blood = false;
        }
    }

    public void noSplatter()
    {
        GetComponent<Image>().sprite = noBlood;
        gameObject.transform.localScale = new Vector3(0, 0);
    }
}







