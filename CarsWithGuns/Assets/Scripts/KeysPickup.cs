using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class KeysPickup : MonoBehaviour
{
    public int amount;
    public GunScript target;
    private GunScript wp;
    public float delay;
    private float nextTime;
    public bool limited;
    public int limit;
    private bool removed;
    public bool destroyAtLimit;
    //Called via message
    //Adds keys to player inventory
    public virtual void Interact()
    {
        if ((Time.time > this.nextTime) && ((this.limit != 0) || !this.limited)) //if it has been long enough, and we are either not past our limit or not limited
        {
            this.nextTime = Time.time + this.delay; //set next use time
            if (this.target == null) //if there isn't a target, use currently equipped weapon
            {
                this.wp = PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].GetComponent(GunScript); //currently equipped weapon	
            }
            else
            {
                 //otherwise use target
                this.wp = this.target;
            }
            this.wp.ApplyToSharedAmmo();
            if (this.GetComponent<AudioSource>())
            {
                GameObject audioObj = new GameObject("PickupSound");
                audioObj.transform.position = this.transform.position;
                audioObj.AddComponent(TimedObjectDestructorDB).timeOut = (error) (this.GetComponent<AudioSource>().clip.length + 0.1f);
                AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource)); //play sound
                aO.clip = this.GetComponent<AudioSource>().clip;
                aO.volume = this.GetComponent<AudioSource>().volume;
                aO.pitch = this.GetComponent<AudioSource>().pitch;
                aO.Play();
                aO.loop = false;
                aO.rolloffMode = AudioRolloffMode.Linear;
            }
            this.removed = true;
            this.wp.ApplyToSharedAmmo();
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