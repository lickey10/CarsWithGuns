using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MoneyPickup : MonoBehaviour
{
    public int amount;
    public float delay;
    private float nextTime;
    public bool limited;
    public int limit;
    private bool removed;
    public bool destroyAtLimit;
    //Called via message
    //Adds ammo to player
    public virtual void Interact()
    {
        if ((Time.time > this.nextTime) && ((this.limit != 0) || !this.limited)) //if it has been long enough, and we are either not past our limit or not limited
        {
            this.nextTime = Time.time + this.delay; //set next use time
            DBStoreController.singleton.balance = DBStoreController.singleton.balance + this.amount; //add up to max
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
            this.removed = true;
            if (this.removed)
            {
                this.limit--;
                this.removed = false;
            }
            if ((this.limit <= 0) && this.destroyAtLimit)
            {
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }
    }

}