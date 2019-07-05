using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class UpgradePickup : MonoBehaviour
{
    public bool apply;
    public bool own;
    public bool mustBeEquipped;
    public Upgrade upgrade;
    public bool destroys;
    private float nextTime;
    public float delay;
    public bool limited;
    public int limit;
    //Called via message
    //Gives Upgrade
    public virtual void Interact()
    {
        if (((Time.time > this.nextTime) && ((this.limit != 0) || !this.limited)) && (((GunScript) this.upgrade.transform.parent.GetComponent(typeof(GunScript))).gunActive || !this.mustBeEquipped)) //if it has been long enough, and we are either not past our limit or not limited
        {
            this.nextTime = Time.time + this.delay; //set next time
            if ((this.own && !this.upgrade.owned) || (this.apply && !this.upgrade.applied)) //if the upgrade isn't already applied
            {
                if (this.own)
                {
                    this.upgrade.owned = true;
                }
                if (this.apply)
                {
                    this.upgrade.ApplyUpgrade();
                }
                if (this.GetComponent<AudioSource>())
                {
                    GameObject audioObj = new GameObject("PickupSound");
                    audioObj.transform.position = this.transform.position;
                    ((TimedObjectDestructorDB) audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = this.GetComponent<AudioSource>().clip.length + 0.1f;
                    AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource)); //play sound
                    aO.clip = this.GetComponent<AudioSource>().clip;
                    aO.volume = this.GetComponent<AudioSource>().volume;
                    aO.pitch = this.GetComponent<AudioSource>().pitch;
                    aO.Play();
                    aO.loop = false;
                    aO.rolloffMode = AudioRolloffMode.Linear;
                }
                this.limit--; //decrement limit
            }
        }
        if ((this.limit <= 0) && this.destroys)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public UpgradePickup()
    {
        this.apply = true;
        this.own = true;
        this.mustBeEquipped = true;
        this.delay = 1;
    }

}