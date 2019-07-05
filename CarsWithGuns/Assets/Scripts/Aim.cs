using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {
    GameObject player;
    bool aiming = false;

	// Use this for initialization
	void Start () {
        player = PlayerWeapons.player;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
           
        }
    }

    public void AimGun()
    {
        aiming = !aiming;
        player.BroadcastMessage("KeepAiming", aiming, SendMessageOptions.DontRequireReceiver);
    }
}
