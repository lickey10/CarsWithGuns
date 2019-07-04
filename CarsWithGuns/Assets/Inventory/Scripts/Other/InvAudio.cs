using UnityEngine;
using System.Collections;

[System.Serializable]
//The sound clips
[UnityEngine.RequireComponent(typeof(AudioSource))]
[UnityEngine.AddComponentMenu("Inventory/Other/Inv Audio")]
public partial class InvAudio : MonoBehaviour
{
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip equipSound;
    public AudioClip pickUpSound;
    public AudioClip dropItemSound;
    public virtual void Awake()
    {
         //This is where we check if the script is attached to the Inventory.
        if (this.transform.name != "Inventory")
        {
            Debug.LogError(("An InvAudio script is placed on " + this.transform.name) + ". It should only be attached to an 'Inventory' object");
        }
        //This is where we assign the default sounds if nothing else has been put in.
        if (this.openSound == null)
        {
            this.openSound = (AudioClip) Resources.Load("Sounds/InvOpenSound", typeof(AudioClip));
        }
        if (this.closeSound == null)
        {
            this.closeSound = (AudioClip) Resources.Load("Sounds/InvCloseSound", typeof(AudioClip));
        }
        if (this.equipSound == null)
        {
            this.equipSound = (AudioClip) Resources.Load("Sounds/InvEquipSound", typeof(AudioClip));
        }
        if (this.pickUpSound == null)
        {
            this.pickUpSound = (AudioClip) Resources.Load("Sounds/InvPickUpSound", typeof(AudioClip));
        }
        if (this.dropItemSound == null)
        {
            this.dropItemSound = (AudioClip) Resources.Load("Sounds/InvDropItemSound", typeof(AudioClip));
        }
    }

    //This is where we play the open and close sounds.
    public virtual void ChangedState(bool open)
    {
        if (open)
        {
            this.GetComponent<AudioSource>().clip = this.openSound;
            this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.1f);
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().clip = this.closeSound;
            this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.1f);
            this.GetComponent<AudioSource>().Play();
        }
    }

    //The rest of the functions can easily be called to play different sounds using SendMessage("Play<NameOfSound>", SendMessageOptions.DontRequireReceiver);
    public virtual void PlayEquipSound()
    {
        this.GetComponent<AudioSource>().clip = this.equipSound;
        this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.1f);
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void PlayPickUpSound()
    {
        this.GetComponent<AudioSource>().clip = this.pickUpSound;
        this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.1f);
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void PlayDropItemSound()
    {
        this.GetComponent<AudioSource>().clip = this.dropItemSound;
        this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.1f);
        this.GetComponent<AudioSource>().Play();
    }

}