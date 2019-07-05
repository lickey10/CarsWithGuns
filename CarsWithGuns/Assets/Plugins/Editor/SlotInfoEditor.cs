using UnityEditor;
using System.Collections;

//FPS Constructor - Weapons
//Copyright© Dastardly Banana Productions 2010
//This script, and all others contained within the Dastardly Banana Weapons Package, may not be shared or redistributed. They can be used in games, either commerical or non-commercial, as long as Dastardly Banana Productions is attributed in the credits.
//Permissions beyond the scope of this license may be available at mailto://info@dastardlybanana.com.
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class SlotInfoEditor : Editor
{
    public PlayerWeapons player;
    public bool[] foldoutState;
    public int[] tmpAllowed;
    public virtual void Awake()
    {
        this.player = FindObjectOfType(PlayerWeapons) as PlayerWeapons;
        this.foldoutState = new bool[this.player.weapons.length];
    }

    public virtual void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        int i = 0;
        //If our allowed array is the wrong length, we must correct it
        if (this.player.weapons.length != this.target.allowed.length)
        {
            //Create an array of the proper length
            this.tmpAllowed = new int[this.player.weapons.length];
            //Now iterate through and copy values
            int upperBound = (int) UnityEngine.Mathf.Min(this.target.allowed.length, this.player.weapons.length);
            int j = 0;
            while (j < upperBound)
            {
                this.tmpAllowed[j] = (int) this.target.allowed[j];
                j++;
            }
            this.target.allowed = this.tmpAllowed;
        }
        //If our slotName array is the wrong length, we must correct it
        if (this.player.weapons.length != this.target.slotName.length)
        {
            //Create an array of the proper length
            string[] tmpAllowedS = new string[this.player.weapons.length];
            //Now iterate through and copy values
            upperBound = (int) UnityEngine.Mathf.Min(this.target.slotName.length, this.player.weapons.length);
            j = 0;
            while (j < upperBound)
            {
                tmpAllowedS[j] = this.target.slotName[j];
                j++;
            }
            this.target.slotName = tmpAllowedS;
        }
        this.player = FindObjectOfType(PlayerWeapons) as PlayerWeapons;
        EditorGUIUtility.LookLikeInspector();
        while (i < this.player.weapons.length)
        {
            if (this.target.slotName[i] == null)
            {
                this.target.slotName[i] = "Slot " + (i + 1);
            }
            this.target.slotName[i] = EditorGUILayout.TextField("Slot Name:", this.target.slotName[i]);
            this.foldoutState[i] = EditorGUILayout.Foldout(this.foldoutState[i], "Allowed Weapon Classes");
            if (this.foldoutState[i])
            {
                foreach (weaponClasses w in weaponClasses.GetValues(weaponClasses))
                {
                    if (w == weaponClasses.Null)
                    {
                        break;
                    }
                    error className = w.ToString().Replace("_", " ");
                    bool allowed = this.target.isWCAllowed(i, w);
                    toggleState = UnityEngine.GUILayout.Toggle(allowed, className);
                    if (toggleState != allowed)
                    {
                        this.target.setAllowed(i, w, toggleState);
                        toggleState = allowed;
                    }
                }
            }
            i++;
        }
        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}