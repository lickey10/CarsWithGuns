using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ReloadSounds : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    public AudioClip sound5;
    public AudioClip sound6;
    public virtual void play1()
    {
        this.GetComponent<AudioSource>().clip = this.sound1;
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void play2()
    {
        this.GetComponent<AudioSource>().clip = this.sound2;
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void play3()
    {
        this.GetComponent<AudioSource>().clip = this.sound3;
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void play4()
    {
        this.GetComponent<AudioSource>().clip = this.sound4;
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void play5()
    {
        this.GetComponent<AudioSource>().clip = this.sound5;
        this.GetComponent<AudioSource>().Play();
    }

    public virtual void play6()
    {
        this.GetComponent<AudioSource>().clip = this.sound6;
        this.GetComponent<AudioSource>().Play();
    }

}