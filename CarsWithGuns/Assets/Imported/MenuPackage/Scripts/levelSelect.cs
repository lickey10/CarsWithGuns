using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/LevelSelect")]
public partial class levelSelect : MonoBehaviour
{
    public float speed;
    public RaycastHit hit;
    public Ray ray;
    public Transform loadingPlane;
    public GUITexture faderTxtr;
    public static float fadeTime;
    public static float fadeAlpha;
    public bool flag;
    public virtual void Start()
    {
         //Fading texture used for fading in and out
        this.faderTxtr = ((GUITexture) GameObject.Find("faderTexture").GetComponent(typeof(GUITexture))) as GUITexture;
        //Loading texture you see whenever any scene is loading
        //Initially it must be deactive
        if (this.loadingPlane.gameObject.active)
        {
            this.loadingPlane.gameObject.active = false;
        }
    }

    public virtual void FixedUpdate() //renderer.material.color=Color.white;
    {
         /*
    The aplha of the fading texture is altered depending on the condition whether you want to fade in
    or fade out. So in the next condition it is being checked that if the alpha of the texture is reached to
    the half of its value then load the scene, in order to give it an effect of fading in and flag determines
    that a load scene button has been clicked
    */
        if ((this.faderTxtr.color.a == (levelSelect.fadeAlpha / 2)) && this.flag)
        {
            Application.LoadLevel(this.hit.transform.name);
        }
        /*The next condition is to detect a mouse event in order to load a desired scene*/
        if (Input.GetMouseButtonDown(0))
        {
            this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            /*
		Raycast determines whether a collider has been hit and then load only that particular scene
		for which the raycast is registered
		*/
            if (Physics.Raycast(this.ray, out this.hit) && (this.hit.transform.name == this.transform.name))
            {
                this.hit.transform.GetComponent<Renderer>().material.color = Color.red;
                this.loadingPlane.gameObject.active = true;
                this.StartCoroutine(Fade.use.Alpha(this.faderTxtr, 0f, levelSelect.fadeAlpha, levelSelect.fadeTime, EaseType.In));
                this.flag = true;
            }
        }
        /*Same above but for the touch events*/
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                this.ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(this.ray, out this.hit) && (this.hit.transform.name == this.transform.name))
                {
                    this.hit.transform.GetComponent<Renderer>().material.color = Color.red;
                    this.loadingPlane.gameObject.active = true;
                    this.flag = true;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
            }
        }
    }

    public levelSelect()
    {
        this.speed = 0.5f;
    }

    static levelSelect()
    {
        levelSelect.fadeTime = 0.3f;
        levelSelect.fadeAlpha = 0.7f;
    }

}