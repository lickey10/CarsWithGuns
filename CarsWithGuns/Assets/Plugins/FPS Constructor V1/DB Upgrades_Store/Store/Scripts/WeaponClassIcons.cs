using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class WeaponClassIcons : MonoBehaviour
{
    //Array to hold icons for the Weapon Classes
    //A Custom Editor Script only displays the number of actual Weapon Classes Defined in WeaponInfo
    public Texture[] weaponClassTextures;
    public WeaponClassIcons()
    {
        this.weaponClassTextures = new Texture[20];
    }

}