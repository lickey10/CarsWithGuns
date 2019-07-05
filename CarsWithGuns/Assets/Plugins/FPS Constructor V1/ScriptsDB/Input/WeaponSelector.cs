using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class WeaponSelector : MonoBehaviour
{
    public static int selectedWeapon; //this value must be changed to switch weapons.
    public virtual void Awake()
    {
        WeaponSelector.selectedWeapon = 0;
    }

    public virtual void LateUpdate()
    {
        WeaponSelector.selectedWeapon = PlayerWeapons.PW.selectedWeapon;
    }

}