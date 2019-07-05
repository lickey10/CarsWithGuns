using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EffectsManager : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    /*
This script is the manager script for the FPS package's effects system. It has two major r. The first is creating,
storing, and managing all the effects sets which will be used in the scene. The user interacts with the script through a custom
editor script called "EffectsManagerEditor", which can be found in the Editor folder.

The script also plays a role in the actual execution of effects at runtime. When a player shoots a wall which uses a hit effect, this
script instantiates the appropriate decal effect; it is also responsible for playing hit sounds and hit effects. The only effect which
is not applied by this script are footstep sounds. Footsteps are handled in a script called "Footstep", which is attached to the player.
*/
    public int maxSets; //The maximum number of decal sets. This is an arbitrary number, and can be increased if required for some reason.
    public int maxDecals; //The maximum number of decals that can exist in the world. This is editable in the inspector
    public EffectSet[] setArray; //This is an array which stores all the effects sets we have created.
    public string[] setNameArray; //The names of each set are stored in a separate array from the actual effects sets
    public int selectedSet; //The set we are currently viewing in the inspector
    public int highestSet; //Because we use a builtin array for our setArray, the size of the array can never change. This variable allows 
     //us to know how many items in our setArray are actually in use, so we don't display any null entries
    public GameObject audioEvent; //When we instantiate an audio event in the script we will store it here.
    public GameObject[] decalArray; //Every time we instantiate a decal effect in the world we will track it here
    public int currentDecal; //Just like highestSet, this tracks the highest non-null index in the decalArray
    public GameObject thePlayer; //We find the player object in update and store its info here
    //These variables are used to make sure that multiple bullet sounds are not all played at once.
    public float lastSoundTime;
    public float soundCooldown;
    public bool canPlay; //If this is false, we can't play a sound yet
    public static string DELETED; //When the user deletes an effects set, we have to move every effects set after it to a new location
    //in the array. A deleted set is renamed "deleted" so the system knows to compact the array.
    public static EffectsManager manager; //static access variable so other scripts can access this one.
    public virtual void Awake()
    {
        this.thePlayer = GameObject.FindWithTag("Player");
        EffectsManager.manager = this;
    }

    public virtual void Update()
    {
        //We want to limit the number of sounds that can be played in a short timeframe, so weapons such as shotguns do not
        //play a hit sound for every single bullet
        if (this.canPlay == false)
        {
            if (Time.time > (this.lastSoundTime + this.soundCooldown))
            {
                this.canPlay = true;
            }
        }
    }

    //This function is called whenever the user adds a new effects set. The new name has to be added to the name array
    public virtual void RebuildNameArray(string str)
    {
        object[] tempArr = new object[0];
        bool abort = false;
        if (this.setNameArray.Length == 0)
        {
            this.setNameArray = new string[1];
        }
        if (this.setNameArray[0] == null)
        {
            this.setNameArray[0] = str;
        }
        else
        {
            int i = 0;
            while (i < this.setNameArray.Length)
            {
                tempArr.Add(this.setNameArray[i]);
                i++;
            }
            if (abort == false)
            {
                tempArr.Add(str);
                this.setNameArray = tempArr.ToBuiltin(typeof(string));
            }
        }
    }

    //Called when the user creates a set
    public virtual void CreateSet()
    {
        this.setArray[this.highestSet] = new EffectSet();
        this.RebuildNameArray("Set " + this.highestSet);
        this.selectedSet = this.highestSet;
        this.highestSet++;
        this.TrimSetArray();
    }

    //Whenever the user deletes a set, this is called
    public virtual void DeleteSet(int index)
    {
        this.setArray[index] = null;
        this.setNameArray[index] = "deleted";
        this.CompactNameArray();
        this.CompactSetArray();
        this.highestSet = this.setNameArray.Length;
    }

    //This is called inside of DeleteSet. It rebuilds the setNameArray to not include the deleted set.
    public virtual void CompactNameArray()
    {
        object[] tempArr = new object[0];
        int i = 0;
        while (i < this.setNameArray.Length)
        {
            if (this.setNameArray[i] != EffectsManager.DELETED)
            {
                tempArr.Add(this.setNameArray[i]);
            }
            i++;
        }
        this.setNameArray = tempArr.ToBuiltin(typeof(string));
    }

    //We shouldn't have any sets at indices greater than 'highestSet.' This function just makes sure of that
    public virtual void TrimSetArray()
    {
        int i = this.highestSet;
        while (i < this.maxSets)
        {
            this.setArray[i] = null;
            i++;
        }
    }

    //This function is called inside of DeleteSet. It reuilds our array of decal sets to not include the deleted set
    public virtual void CompactSetArray()
    {
        EffectSet[] tmpSetArray = new EffectSet[this.maxSets];
        int n = 0;
        int i = 0;
        while (i < this.maxSets)
        {
            if (!(this.setArray[i] == null))
            {
                tmpSetArray[n] = this.setArray[i];
                n++;
            }
            i++;
        }
        this.setArray = tmpSetArray;
    }

    //Renames a Decal Set
    public virtual void Rename(string str)
    {
        bool illegal = false;
        int i = 0;
        while (i < this.setNameArray.Length)
        {
            if (this.setNameArray[i] == str)
            {
                illegal = true;
                Debug.LogWarning(("There is already an effects set named " + str) + "! Please choose a different name");
            }
            i++;
        }
        if (!illegal)
        {
            this.setArray[this.selectedSet].setName = str;
            this.setNameArray[this.selectedSet] = str;
        }
    }

    //Applies hit effects like sparks. Called from either ApplyDecal or ApplyDent
    public virtual void ApplyHitEffect(object[] info)
    {
        //The info array has hit.point, hitRotation, hit.transform, hit.normal, hitSet
        if (this.setArray[info[4]].hitParticles[0] != null)
        {
            Vector3 tempVector = info[0];
            Quaternion tempQuat1 = info[1];
            Transform tempTrans2 = info[2];
            Vector3 tempVector3 = info[3];
            int tempInt4 = info[4];
            int toApply = Random.Range(0, this.setArray[tempInt4].lastHitParticle);
            GameObject clone = UnityEngine.Object.Instantiate(this.setArray[tempInt4].hitParticles[toApply], tempVector, tempQuat1);
            clone.transform.localPosition = clone.transform.localPosition + (0.01f * tempVector3);
            clone.transform.LookAt(this.thePlayer.transform.position);
        }
    }

    //This function is called whenever a weapon is fired (so in 'GunScript' for players and 'Fire' for non-players).
    //It is responsible for applying any required decals, hit effects, and hit sounds.
    public virtual void ApplyDecal(object[] info)
    {
        //The info array has hit.point, hitRotation, hit.transform, hit.normal, hitSet
        if (!(this.setArray[0] == null))
        {
            if (this.setArray[info[4]].bulletDecals[0] != null)
            {
                Vector3 tempVector = info[0];
                Quaternion tempQuat1 = info[1];
                Transform tempTrans2 = info[2];
                Vector3 tempVector3 = info[3];
                int tempInt4 = info[4];
                int toApply = Random.Range(0, this.setArray[tempInt4].lastBulletDecal);
                GameObject clone = UnityEngine.Object.Instantiate(this.setArray[tempInt4].bulletDecals[toApply], tempVector, tempQuat1);
                clone.transform.localPosition = clone.transform.localPosition + (0.05f * tempVector3);
                clone.transform.parent = tempTrans2;
                if (this.currentDecal >= this.maxDecals)
                {
                    this.currentDecal = 0;
                }
                if (this.decalArray[this.currentDecal] != null)
                {
                    UnityEngine.Object.Destroy(this.decalArray[this.currentDecal]);
                }
                this.decalArray[this.currentDecal] = clone;
                this.currentDecal++;
            }
            this.ApplyHitEffect(info);
            this.BulletSound(info);
        }
        else
        {
            Debug.LogWarning("EffectsManager: You have at least one object with the UseDecals script attached, but no decal sets. Please create a decal set");
        }
    }

    //Some weapons may apply 'dents' instead of 'decals' - for example, hitting a sheet of metal with a pipe would not make a round bullet hole appear.
    //This functions identically to ApplyDecal, only it is used in cases where dents are applied instead of the default decal.
    public virtual void ApplyDent(object[] info)
    {
        //The info array has hit.point, hitRotation, hit.transform, hit.normal, hitSet
        if (!(this.setArray[0] == null))
        {
            if (this.setArray[info[4]].dentDecals[0] != null)
            {
                Vector3 tempVector = info[0];
                Quaternion tempQuat1 = info[1];
                Transform tempTrans2 = info[2];
                Vector3 tempVector3 = info[3];
                int tempInt4 = info[4];
                int toApply = Random.Range(0, this.setArray[tempInt4].lastBulletDecal);
                GameObject clone = UnityEngine.Object.Instantiate(this.setArray[tempInt4].bulletDecals[toApply], tempVector, tempQuat1);
                clone.transform.localPosition = clone.transform.localPosition + (0.05f * tempVector3);
                clone.transform.parent = tempTrans2;
                if (this.currentDecal >= this.maxDecals)
                {
                    this.currentDecal = 0;
                }
                if (this.decalArray[this.currentDecal] != null)
                {
                    UnityEngine.Object.Destroy(this.decalArray[this.currentDecal]);
                }
                this.decalArray[this.currentDecal] = clone;
                this.currentDecal++;
            }
            this.ApplyHitEffect(info);
            this.CollisionSound(info);
        }
        else
        {
            Debug.LogWarning("EffectsManager: You have at least one object with the UseDecals script attached, but no decal sets. Please create a decal set");
        }
    }

    //Called by BulletSound and CollisionSound; actually does the legwork of creating an audio event and playing the correct sound
    public virtual void CreateAudioEvent(AudioClip clipToPlay, object[] info)
    {
        this.audioEvent = new GameObject("Audio Event");
        Transform t = info[2] as Transform;
        this.audioEvent.transform.position = t.position;
        this.audioEvent.AddComponent(typeof(AudioSource));
        AudioSource source = (AudioSource) this.audioEvent.GetComponent(typeof(AudioSource));
        //source.rolloffMode = AudioRolloffMode.Linear;
        source.clip = clipToPlay;
        source.Play();
        this.audioEvent.AddComponent(typeof(TimedObjectDestructorDB));
        ((TimedObjectDestructorDB) this.audioEvent.GetComponent(typeof(TimedObjectDestructorDB))).timeOut = clipToPlay.length + 0.5f;
        this.canPlay = false;
        this.lastSoundTime = Time.time;
    }

    //Called from either ApplyDecal or ApplyDent.
    public virtual void BulletSound(object[] info)
    {
        //The info array has hit.point, hitRotation, hit.transform, hit.normal, hitSet
        if ((this.setArray[info[4]].bulletSounds[0] != null) && this.canPlay)
        {
            int toPlay = Random.Range(0, this.setArray[info[4]].lastBulletSound);
            this.CreateAudioEvent(this.setArray[info[4]].bulletSounds[toPlay], info);
        }
    }

    //Called from either ApplyDecal or ApplyDent; same as BulletSound except it plays the set's collision sound instead of its bullet sound
    public virtual void CollisionSound(object[] info)
    {
        //The info array has hit.point, hitRotation, hit.transform, hit.normal, hitSet
        if ((this.setArray[info[4]].collisionSounds[0] != null) && this.canPlay)
        {
            int toPlay = Random.Range(0, this.setArray[info[4]].lastCollisionSound);
            this.CreateAudioEvent(this.setArray[info[4]].collisionSounds[toPlay], info);
        }
    }

    public EffectsManager()
    {
        this.maxSets = 75;
        this.maxDecals = 75;
        this.setArray = new EffectSet[this.maxSets];
        this.setNameArray = new string[1];
        this.decalArray = new GameObject[this.maxDecals];
        this.soundCooldown = 0.005f;
        this.canPlay = true;
    }

    static EffectsManager()
    {
        EffectsManager.DELETED = "deleted";
    }

}