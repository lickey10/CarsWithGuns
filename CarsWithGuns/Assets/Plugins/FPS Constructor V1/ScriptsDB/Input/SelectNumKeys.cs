using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SelectNumKeys : MonoBehaviour
{
    public InputItem input;
    public virtual void UpdateInput()
    {
        if (Input.GetKeyDown("1"))
        {
            WeaponSelector.selectedWeapon = 0;
            this.input.down = true;
        }
        else
        {
            if (Input.GetKeyDown("2"))
            {
                WeaponSelector.selectedWeapon = 1;
                this.input.down = true;
            }
            else
            {
                if (Input.GetKeyDown("3"))
                {
                    WeaponSelector.selectedWeapon = 2;
                    this.input.down = true;
                }
                else
                {
                    if (Input.GetKeyDown("4"))
                    {
                        WeaponSelector.selectedWeapon = 3;
                        this.input.down = true;
                    }
                    else
                    {
                        if (Input.GetKeyDown("5"))
                        {
                            WeaponSelector.selectedWeapon = 4;
                            this.input.down = true;
                        }
                        else
                        {
                            if (Input.GetKeyDown("6"))
                            {
                                WeaponSelector.selectedWeapon = 5;
                                this.input.down = true;
                            }
                            else
                            {
                                if (Input.GetKeyDown("7"))
                                {
                                    WeaponSelector.selectedWeapon = 6;
                                    this.input.down = true;
                                }
                                else
                                {
                                    if (Input.GetKeyDown("8"))
                                    {
                                        WeaponSelector.selectedWeapon = 7;
                                        this.input.down = true;
                                    }
                                    else
                                    {
                                        if (Input.GetKeyDown("9"))
                                        {
                                            WeaponSelector.selectedWeapon = 8;
                                            this.input.down = true;
                                        }
                                        else
                                        {
                                            if (Input.GetKeyDown("0"))
                                            {
                                                WeaponSelector.selectedWeapon = 9;
                                                this.input.down = true;
                                            }
                                            else
                                            {
                                                this.input.down = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}