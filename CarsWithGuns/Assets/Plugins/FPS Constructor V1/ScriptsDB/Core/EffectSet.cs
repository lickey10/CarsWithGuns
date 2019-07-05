using UnityEngine;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
[System.Serializable]
public class EffectSet : object
{
    public int maxOfEach; //You can increase this if desired
    public int setID;
    public string setName;
    public int localMax;
    public GameObject blankGameObject;
    public GameObject[] bulletDecals;
    public bool bulletDecalsFolded;
    public int lastBulletDecal;
    public GameObject[] dentDecals;
    public bool dentDecalsFolded;
    public int lastDentDecal;
    public GameObject[] hitParticles;
    public bool hitParticlesFolded;
    public int lastHitParticle;
    public AudioClip[] footstepSounds;
    public bool footstepSoundsFolded;
    public int lastFootstepSound;
    public AudioClip[] crawlSounds;
    public bool crawlSoundsFolded;
    public int lastCrawlSound;
    public AudioClip[] bulletSounds;
    public bool bulletSoundsFolded;
    public int lastBulletSound;
    public AudioClip[] collisionSounds;
    public bool collisionSoundsFolded;
    public int lastCollisionSound;
    public EffectSet()
    {
        this.maxOfEach = 15;
        this.setName = "New Set";
        this.localMax = 20;
        this.bulletDecals = new GameObject[this.maxOfEach];
        this.dentDecals = new GameObject[this.maxOfEach];
        this.hitParticles = new GameObject[this.maxOfEach];
        this.footstepSounds = new AudioClip[this.maxOfEach];
        this.crawlSounds = new AudioClip[this.maxOfEach];
        this.bulletSounds = new AudioClip[this.maxOfEach];
        this.collisionSounds = new AudioClip[this.maxOfEach];
    }

}