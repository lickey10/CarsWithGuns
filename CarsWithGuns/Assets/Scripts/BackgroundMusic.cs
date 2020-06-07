using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource fxSound; 
    public AudioClip[] BackgroundMusicClips;

    // Start is called before the first frame update
    void Start()
    {
        fxSound = GetComponent<AudioSource>();
        fxSound.clip = BackgroundMusicClips[Random.Range(0, BackgroundMusicClips.Length - 1)];

        fxSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
