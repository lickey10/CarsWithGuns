using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ScareTrigger : MonoBehaviour
{
    public GameObject scare;
    public bool played;
    public bool trig;
    public AudioClip scareSound;
    ////////Make sure its not visable and reset////////
    public virtual void Start()
    {
        this.trig = false;
        this.scare.GetComponent<Renderer>().enabled = false;
    }

    /////When player enters trigger/////// set to true///////
    public virtual void OnTriggerEnter(Collider other)
    {
        this.trig = true;
    }

    //////Enable renderer and trigger sound and timer/////
    public virtual void Update()
    {
        if (this.trig == true)
        {
            this.scare.GetComponent<Renderer>().enabled = true;
            this.StartCoroutine(this.removeovertime());
            this.makehimscream();
        }
    }

    //// timer ////
    public virtual IEnumerator removeovertime()
    {
        yield return new WaitForSeconds(0.8f);
        this.scare.GetComponent<Renderer>().enabled = false;
        UnityEngine.Object.Destroy(this.gameObject);
    }

    //// sound /////
    public virtual void makehimscream()
    {
        if (!this.played)
        {
            this.played = true;
            this.GetComponent<AudioSource>().PlayOneShot(this.scareSound);
        }
    }

}