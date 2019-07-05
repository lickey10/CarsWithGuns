using UnityEngine;
using System.Collections;

//FPS Constructor - Weapons
//Copyright� Dastardly Banana Productions 2010
//This script, and all others contained within the Dastardly Banana Weapons Package, may not be shared or redistributed. They can be used in games, either commerical or non-commercial, as long as Dastardly Banana Productions is attributed in the credits.
//Permissions beyond the scope of this license may be available at mailto://info@dastardlybanana.com.
//Custom editor 
public enum crosshairTypes
{
    Friend = 0,
    Foe = 1,
    Other = 2
}

[System.Serializable]
public partial class CrosshairColor : MonoBehaviour
{
    public crosshairTypes crosshairType;
    private GameObject weaponCam;
    public virtual void Start()
    {
        this.weaponCam = GameObject.FindWithTag("WeaponCamera");
    }

}