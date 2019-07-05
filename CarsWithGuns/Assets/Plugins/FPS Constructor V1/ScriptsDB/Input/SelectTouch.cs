using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SelectTouch : MonoBehaviour
{
    public InputItem input;
    public Vector2 pos1; //position of button 1
    public Vector2 pos2; //position of button 2
    public Vector2 dimensions; //size of buttons
    private int selected;
    //was either button already being touched?
    private bool touched1;
    private bool touched2;
    public int numWeapons; //number of weapons in PlayerWeapons array
    public string label1;
    public string label2;
    public virtual void UpdateInput()
    {
        //are we touching one of the buttons this frame? Used in loop
        bool touching1 = false;
        bool touching2 = false;
        //flag variables for if touch 
        bool t1 = false;
        bool t2 = false;
        if (Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                touching1 = false;
                touching2 = false;
                //Is the touch within the bounds of wither button?
                touching1 = this.Within(touch.position, new Rect(this.pos1.x, this.pos1.y, this.dimensions.x, this.dimensions.y));
                touching2 = this.Within(touch.position, new Rect(this.pos2.x, this.pos2.y, this.dimensions.x, this.dimensions.y));
                if (touching1)
                {
                    t1 = true;
                }
                if (touching2)
                {
                    t2 = true;
                }
            }
        }
        if (!AimMode.canSwitchWeaponAim || !PlayerWeapons.canSwitchWeapon)
        {
            return;
        }
        if (t1 && !this.touched1) //we hit the first button
        {
            this.CycleWeapon(-1); //previous weapon
            WeaponSelector.selectedWeapon = this.selected;
            this.touched1 = true;
            this.input.down = true;
        }
        else
        {
            if (t2 && !this.touched2) //we hit the second button
            {
                this.CycleWeapon(1);
                WeaponSelector.selectedWeapon = this.selected;
                this.touched2 = true;
                this.input.down = true;
            }
            else
            {
                if (!(t1 || t2)) //We are not touching
                {
                    this.input.down = false;
                    this.touched1 = false;
                    this.touched2 = false;
                }
            }
        }
        //Wraparound on weapon array
        if (this.selected < 0)
        {
            this.selected = this.numWeapons - 1;
            WeaponSelector.selectedWeapon = this.selected;
        }
        if (this.selected >= this.numWeapons)
        {
            this.selected = 0;
            WeaponSelector.selectedWeapon = this.selected;
        }
    }

    public virtual int CycleWeapon(int dir)
    {
        this.selected = this.selected + dir;
        if (this.selected >= PlayerWeapons.PW.weapons.Length)
        {
            this.selected = 0;
        }
        else
        {
            if (this.selected < 0)
            {
                this.selected = PlayerWeapons.PW.weapons.Length - 1;
            }
        }
        int temp = this.selected;
        int i = 0;
        while (i < PlayerWeapons.PW.weapons.Length)
        {
            if (PlayerWeapons.PW.weapons[this.selected] != null)
            {
                return this.selected;
            }
            this.selected = this.selected + dir;
            if (this.selected >= PlayerWeapons.PW.weapons.Length)
            {
                this.selected = 0;
            }
            else
            {
                if (this.selected < 0)
                {
                    this.selected = PlayerWeapons.PW.weapons.Length - 1;
                }
            }
            i++;
        }
        return temp;
    }

    public virtual void LateUpdate()
    {
        this.selected = PlayerWeapons.PW.selectedWeapon;
    }

    public virtual void OnGUI()
    {
        //Just buttons for display, not acutally used
        GUI.Button(new Rect(this.pos1.x, this.pos1.y, this.dimensions.x, this.dimensions.y), this.label1);
        GUI.Button(new Rect(this.pos2.x, this.pos2.y, this.dimensions.x, this.dimensions.y), this.label2);
    }

    public virtual bool Within(Vector2 pos, Rect bounds)
    {
        pos.y = Screen.height - pos.y;
        return (((pos.x > bounds.x) && (pos.x < (bounds.x + bounds.width))) && (pos.y > bounds.y)) && (pos.y < (bounds.y + bounds.height));
    }

}