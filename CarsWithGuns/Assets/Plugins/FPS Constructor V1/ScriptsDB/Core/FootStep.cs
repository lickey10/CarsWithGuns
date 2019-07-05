using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FootStep : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float footstepInterval;
    public float footstepVolume;
    private float distanceMoved;
    private Vector3 lastStep;
    private bool landing;
    [UnityEngine.HideInInspector]
    public EffectsManager effectsManager;
    [UnityEngine.HideInInspector]
    public CharacterMotorDB characterMotor;
    [UnityEngine.HideInInspector]
    public AudioSource source;
    [UnityEngine.HideInInspector]
    public AudioClip soundClip;
    [UnityEngine.HideInInspector]
    public int playDex;
    [UnityEngine.HideInInspector]
    public GameObject surface;
    public virtual void Awake()
    {
        this.effectsManager = (EffectsManager) GameObject.FindObjectOfType(typeof(EffectsManager));
        this.characterMotor = (CharacterMotorDB) this.gameObject.GetComponent(typeof(CharacterMotorDB));
        this.source = (AudioSource) this.gameObject.GetComponent(typeof(AudioSource));
    }

    public virtual void Update()
    {
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        if (!this.characterMotor.grounded)
        {
            this.distanceMoved = this.footstepInterval;
            this.landing = true;
        }
        this.distanceMoved = this.distanceMoved + Vector3.Distance(this.transform.position, this.lastStep);
        this.lastStep = this.transform.position;
        if (CharacterMotorDB.walking)//|| (landing && characterMotor.grounded))){
        {
            if (CharacterMotorDB.prone)
            {
                this.Crawl();
                this.landing = false;
            }
            else
            {
                this.Footstep();
                this.landing = false;
            }
        }
    }

    public virtual void Airborne()
    {
        if (CharacterMotorDB.prone)
        {
            this.Crawl();
            this.landing = false;
        }
        else
        {
            this.Footstep();
            this.landing = false;
        }
    }

    public virtual void Landed()
    {
        if (CharacterMotorDB.prone)
        {
            this.Crawl();
            this.landing = false;
        }
        else
        {
            this.Footstep();
            this.landing = false;
        }
    }

    public virtual void Footstep()
    {
        if (this.distanceMoved >= this.footstepInterval)
        {
            this.GetClip();
            /*source.clip = soundClip;
		source.volume = footstepVolume;
		source.Play();*/
            if (this.soundClip != null)
            {
                GameObject audioObj = new GameObject("Footstep");
                audioObj.transform.position = this.transform.position;
                audioObj.transform.parent = this.transform;
                ((TimedObjectDestructorDB) audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = this.soundClip.length + 0.1f;
                AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource));
                aO.clip = this.soundClip;
                aO.volume = this.footstepVolume;
                aO.Play();
                aO.loop = false;
                aO.rolloffMode = AudioRolloffMode.Linear;
            }
            this.distanceMoved = 0;
        }
    }

    public virtual void Crawl()
    {
        if (this.distanceMoved >= this.footstepInterval)
        {
            this.GetCrawlClip();
            /*source.clip = soundClip;
		source.volume = footstepVolume;
		source.Play();*/
            if (this.soundClip != null)
            {
                GameObject audioObj = new GameObject("Footstep");
                audioObj.transform.position = this.transform.position;
                audioObj.transform.parent = this.transform;
                ((TimedObjectDestructorDB) audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = this.soundClip.length + 0.1f;
                AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource));
                aO.clip = this.soundClip;
                aO.volume = this.footstepVolume;
                aO.Play();
                aO.loop = false;
                aO.rolloffMode = AudioRolloffMode.Linear;
            }
            this.distanceMoved = 0;
        }
    }

    //This function, called by Crawl, gets a random clip and sets soundClip to equal that clip
    public virtual void GetCrawlClip()
    {
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(this.transform.position, -Vector3.up, out hit, 100, ~(1 << PlayerWeapons.playerLayer)))
        {
            if ((UseEffects) hit.transform.GetComponent(typeof(UseEffects)))
            {
                UseEffects effectScript = (UseEffects) hit.transform.GetComponent(typeof(UseEffects));
                int dex = effectScript.setIndex;
                if (!(this.effectsManager.setArray[0] == null))
                {
                    if (!(this.effectsManager.setArray[dex].crawlSounds == null))
                    {
                        this.soundClip = this.effectsManager.setArray[dex].crawlSounds[this.playDex];
                        if (this.playDex >= (this.effectsManager.setArray[dex].lastCrawlSound - 1))
                        {
                            this.playDex = 0;
                        }
                        else
                        {
                            this.playDex++;
                        }
                    }
                }
            }
        }
    }

    //This function, called by Footstep, gets a random clip and sets soundClip to equal that clip
    public virtual void GetClip()
    {
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(this.transform.position, -Vector3.up, out hit, 100, ~(1 << PlayerWeapons.playerLayer)))
        {
            if ((UseEffects) hit.transform.GetComponent(typeof(UseEffects)))
            {
                UseEffects effectScript = (UseEffects) hit.transform.GetComponent(typeof(UseEffects));
                int dex = effectScript.setIndex;
                if (!(this.effectsManager.setArray[0] == null))
                {
                    if (!(this.effectsManager.setArray[dex].footstepSounds == null))
                    {
                        this.soundClip = this.effectsManager.setArray[dex].footstepSounds[this.playDex];
                        if (this.playDex >= (this.effectsManager.setArray[dex].lastFootstepSound - 1))
                        {
                            this.playDex = 0;
                        }
                        else
                        {
                            this.playDex++;
                        }
                    }
                }
            }
        }
    }

    public FootStep()
    {
        this.footstepInterval = 0.5f;
        this.footstepVolume = 0.5f;
    }

}