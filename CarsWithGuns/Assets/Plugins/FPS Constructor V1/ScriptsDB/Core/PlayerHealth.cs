using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerHealth : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public float health;
    public float maxHealth;
    public float hitKickBack;
    public float hitKickBackX;
    public float kickMaxY;
    public float kickMaxX;
    public float kickInt;
    private float nextKickTime;
    public Texture redWindow;
    public float redFlashTime;
    public float directionalFlashTime;
    private float hitEffectTime;
    private float dirEffectTime;
    private GameObject cam;
    public AudioClip[] hitSounds;
    public AudioClip[] landSounds;
    public float hitSoundInterval;
    public float hitSoundVolume;
    private float nextSoundTime;
    private PlayerWeapons pWeapons;
    private GameObject mainCam;
    public static bool dead;
    public AudioClip deathSound;
    public GameObject directionalTexture;
    public static PlayerHealth singleton;
    public virtual void Start()
    {
        PlayerHealth.singleton = this;
        PlayerHealth.dead = false;
        this.cam = PlayerWeapons.weaponCam;
        this.hitEffectTime = 0;
        this.mainCam = PlayerWeapons.mainCam;
        this.pWeapons = (PlayerWeapons) this.GetComponentInChildren(typeof(PlayerWeapons));
        this.health = this.maxHealth;
    }

    public virtual void ApplyFallDamage(float d)
    {
        if (PlayerHealth.dead)
        {
            return;
        }
        this.health = Mathf.Clamp(this.health - d, 0, this.health);
        this.HitEffects(d);
        if (this.health <= 0)
        {
            this.GetComponent<AudioSource>().clip = this.deathSound;
            this.GetComponent<AudioSource>().volume = this.hitSoundVolume;
            this.GetComponent<AudioSource>().Play();
            this.Die();
        }
    }

    public virtual void ApplyDamage(float d)
    {
        if (PlayerHealth.dead)
        {
            return;
        }
        this.hitEffectTime = this.redFlashTime;
        //	float.TryParse(Arr[0], tempFloat);
        this.health = Mathf.Clamp(this.health - d, 0, this.health);
        this.HitEffects(d);
        if (this.health <= 0)
        {
            this.GetComponent<AudioSource>().clip = this.deathSound;
            this.GetComponent<AudioSource>().volume = this.hitSoundVolume;
            this.GetComponent<AudioSource>().Play();
            this.Die();
        }
    }

    public virtual void ApplyDamage(object[] Arr)
    {
        float tempFloat = 0.0f;
        if (PlayerHealth.dead)
        {
            return;
        }
        tempFloat = Arr[0];
        this.health = Mathf.Clamp(this.health - tempFloat, 0, this.health);
        this.HitEffects(tempFloat);
        if (this.health <= 0)
        {
            this.GetComponent<AudioSource>().clip = this.deathSound;
            this.GetComponent<AudioSource>().volume = this.hitSoundVolume;
            this.GetComponent<AudioSource>().Play();
            this.Die();
        }
    }

    public virtual void OnGUI()
    {
        this.hitEffectTime = this.hitEffectTime - Time.deltaTime;
        if (this.hitEffectTime > 0)
        {
            GUI.color = new Color(1, 1, 1, this.hitEffectTime);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.redWindow);
        }
    }

    public virtual void Direction(Transform h)
    {
        GameObject temp = UnityEngine.Object.Instantiate(this.directionalTexture, this.transform.position, this.transform.rotation);
        temp.transform.parent = this.cam.transform;
        this.StartCoroutine(((HitDirectional) temp.GetComponent(typeof(HitDirectional))).Init(h, this.directionalFlashTime));
    }

    public static void ScreenDamage(float t)
    {
        PlayerHealth.singleton.hitEffectTime = t;
    }

    public static void ScreenDamage(float damage, Transform where)
    {
        PlayerHealth.singleton.Direction(where);
        PlayerHealth.singleton.HitEffects(damage);
    }

    public virtual void HitEffects(float damage)
    {
        this.hitEffectTime = this.redFlashTime;
        if (Time.time > this.nextKickTime)
        {
            //cam.GetComponent(MouseLookDBJS).offsetY = Mathf.Clamp(hitKickBack*damage*Random.value, 0, kickMaxY);
            //GameObject.FindWithTag("Player").GetComponent(MouseLookDBJS).offsetX = Mathf.Clamp(hitKickBackX*damage*Random.value, 0, kickMaxX);
            this.nextKickTime = Time.time + this.kickInt;
        }
        if (Time.time > this.nextSoundTime)
        {
            this.nextSoundTime = Time.time + this.hitSoundInterval;
            int temp = Mathf.Round(Random.value * this.hitSounds.Length);
            if (temp == 0)
            {
                temp = 1;
            }
            if ((this.GetComponent<AudioSource>() != null) && (this.hitSounds.Length > 0))
            {
                this.GetComponent<AudioSource>().clip = this.hitSounds[temp - 1];
                this.GetComponent<AudioSource>().volume = this.hitSoundVolume;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    public virtual void Die()
    {
        PlayerWeapons.HideWeapon();
        PlayerWeapons.playerActive = false;
        //this.GetComponent(CharacterMotorDB).canControl = false;
        this.BroadcastMessage("Death");
        this.BroadcastMessage("Freeze");
        LockCursor.HardUnlock();
        PlayerHealth.dead = true;
    }

    public PlayerHealth()
    {
        this.health = 100;
        this.maxHealth = 100;
        this.hitSoundInterval = 0.6f;
        this.hitSoundVolume = 1;
    }

}