using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HealthPickup : MonoBehaviour
{
    public int amount;
    private PlayerHealth p;
    public float delay;
    public bool destroys;
    private float nextTime;
    public bool limited;
    public int limit;
    //Called via message
    //Adds health to player
    public virtual void Interact()
    {
        if ((Time.time > this.nextTime) && ((this.limit != 0) || !this.limited)) //if it has been long enough, and we are either not past our limit or not limited
        {
            this.nextTime = Time.time + this.delay; //set next time
            this.p = (PlayerHealth) PlayerWeapons.player.GetComponent(typeof(PlayerHealth));
            if (this.p.health < this.p.maxHealth) //if we aren't at full health already
            {
                this.p.health = Mathf.Clamp(this.p.health + this.amount, this.p.health, this.p.maxHealth); //add health up to max
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

}