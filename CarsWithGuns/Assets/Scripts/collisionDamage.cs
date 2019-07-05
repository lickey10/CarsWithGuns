using UnityEngine;
using System.Collections;

public class collisionDamage : MonoBehaviour {

    float smashDamage = 0;
    float topSmashDamage = 0;//this tracks the highest amount of damage dealt
    Scripts.ScreenManager sceneManager = new Scripts.ScreenManager();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        smashDamage = 1 * other.relativeVelocity.magnitude;

        if(other.gameObject.GetComponent<Collider>() != null)
            other.gameObject.GetComponent<Collider>().SendMessageUpwards("ApplyDamagePlayer", smashDamage, SendMessageOptions.DontRequireReceiver);

        sceneManager.SmashDamage = sceneManager.SmashDamage + smashDamage;

        if (smashDamage > topSmashDamage)
            topSmashDamage = smashDamage;

        if (other.gameObject.tag == "Enemy")
        {
            Scripts.ScreenManager.Kills++;
            Scripts.ScreenManager.DeathByCar++;
            bloodSplatter.blood = true; 
        }
    }
}
