using UnityEngine;
using System.Collections;

[System.Serializable]
//This is an example of how WeaponSwitching could be handled together with the Inventory System in a First Person game.
//This method is fairly taxing but works great for quickly switching between weapons and having the weapons displayed in the top left corner.
//To learn how to use/write this kind of script, please visit http://youtube.com/brackeys/ since we create a similar (not as advanced) script in our Survival Game Series.
//Attach the script to the object which the weapons are parented to when equipped.
 //The Weapon currently selected as an int.
 //The maximum number of weapons the Player can carry.
 //The default 'Fists' object to use when nothing is equipped. The system will make sure that there is always a 'Fists' object unless all weaponSlots are filled.
 //Are there a 'Fists' object?
 //This is where you can assign a custom GUI skin or use the one included (OtherSkin) under the Resources folder.
 //Set this to true if your Character/Arms has animations for when holding a weapon for using fists only. This technique can also be applied if you want different weapons to have different animations.
 //This is the Animator that we will use for the above.
[UnityEngine.AddComponentMenu("Inventory/Other/Weapon Switch")]
public partial class WeaponSwitch : MonoBehaviour
{
    private int currentWeapon;
    public int maxWeapons;
    public Transform Fists;
    public bool fistsOnObject;
    public GUISkin theSkin;
    public bool switchBetweenAnimations;
    public Animator theAnimator;
    //Load the default skin if nothing has been put in.
    public virtual void Awake()
    {
        if (this.theSkin == null)
        {
            this.theSkin = (GUISkin) Resources.Load("OtherSkin", typeof(GUISkin));
        }
    }

    public virtual void Update()
    {
         //Handle the Fists
        if (((this.transform.childCount - 1) < this.maxWeapons) && (this.fistsOnObject == false))
        {
            Transform Clone = UnityEngine.Object.Instantiate(this.Fists, this.transform.position, this.transform.rotation);
            Clone.transform.parent = this.transform;
            Clone.gameObject.name = "Fists";
            this.fistsOnObject = true;
        }
        if ((this.transform.childCount - 1) > this.maxWeapons)
        {
            UnityEngine.Object.Destroy(this.transform.Find("Fists").gameObject);
            this.fistsOnObject = false;
        }
        //Change weapons using the Scrollwheel.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if ((this.currentWeapon + 1) <= this.maxWeapons)
            {
                this.currentWeapon++;
            }
            else
            {
                this.currentWeapon = 0;
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if ((this.currentWeapon - 1) >= 0)
                {
                    this.currentWeapon--;
                }
                else
                {
                    this.currentWeapon = this.maxWeapons;
                }
            }
        }
        //Make the weapons "loop" when exceeding the maxWeapons value.
        if (this.currentWeapon > this.maxWeapons)
        {
            this.currentWeapon = 0;
        }
        if (this.currentWeapon <= -1)
        {
            this.currentWeapon = this.maxWeapons;
        }
        //Select a weapon using the number keys.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.currentWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && (this.maxWeapons >= 1))
        {
            this.currentWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && (this.maxWeapons >= 2))
        {
            this.currentWeapon = 2;
        }
        //Make sure that the currentWeapon doesn't exceed the number of weapons.
        while (this.currentWeapon > (this.transform.childCount - 1))
        {
            this.currentWeapon = this.currentWeapon - 1;
        }
        //Call the SelectWeapon function.
        this.SelectWeapon(this.currentWeapon);
    }

    //Selects the weapon based on the currentWeapon variable.
    public virtual void SelectWeapon(int index)
    {
        int i = 0;
        while (i < this.transform.childCount) //Loop through the weapons.
        {
             //Activate the selected weapon
            if (i == index)
            {
                if (this.switchBetweenAnimations == true) //If the 'switchBetweenAnimations' variable is true we change the animation to fit the Weapon. In this case if we are using one or not.
                {
                    if (this.transform.GetChild(i).name == "Fists")
                    {
                        this.theAnimator.SetBool("WeaponIsOn", false);
                    }
                    else
                    {
                        this.theAnimator.SetBool("WeaponIsOn", true);
                    }
                }
                //Activate the match
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                 //Deactivate all others
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
            i++;
        }
    }

    //Show the selected weapon in the top right corner and the others less visible.
    public virtual void OnGUI()
    {
        if (this.theSkin != null)
        {
            GUI.skin = this.theSkin;
        }
        GUILayout.BeginArea(new Rect(10, 10, 400, 50));
        GUILayout.BeginHorizontal();
        GUI.color = new Color(1, 1, 1, 0.7f);
        GUILayout.Box("Weapons:");
        int i = 0;
        while (i < this.transform.childCount)
        {
            Transform theChild = this.transform.GetChild(i);
            if (this.currentWeapon == i)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
            }
            else
            {
                GUI.color = new Color(1, 1, 1, 0.4f);
            }
            GUILayout.Box("" + theChild.name);
            i++;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public WeaponSwitch()
    {
        this.maxWeapons = 2;
        this.fistsOnObject = true;
    }

}