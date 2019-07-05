using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FireSelector : MonoBehaviour
{
    public GunScript gscript;
    public int state;
    public AudioClip sound;
    public float soundVolume;
    public string anim;
    public virtual void Start()
    {
        this.gscript.autoFire = this.state == 0;
        this.gscript.burstFire = this.state == 1;
    }

    public virtual void Cycle()
    {
        if (((!this.gscript.gunActive || AimMode.sprintingPublic) || LockCursor.unPaused) || GunScript.reloading)
        {
            return;
        }
        this.GetComponent<AudioSource>().PlayOneShot(this.sound, this.soundVolume);
        if (this.anim != "")
        {
            this.BroadcastMessage("PlayAnim", this.anim);
        }
        this.state++;
        if (this.state == 3)
        {
            this.state = 0;
        }
        this.gscript.autoFire = this.state == 0;
        this.gscript.burstFire = this.state == 1;
    }

    public virtual void Update()
    {
        if (InputDB.GetButtonDown("Fire2"))
        {
            this.Cycle();
        }
    }

    public FireSelector()
    {
        this.soundVolume = 1;
    }

}