using UnityEditor;
using System.Collections;

//FPS Constructor - Weapons
//Copyright© Dastardly Banana Productions 2010
//This script, and all others contained within the Dastardly Banana Weapons Package, may not be shared or redistributed. They can be used in games, either commerical or non-commercial, as long as Dastardly Banana Productions is attributed in the credits.
//Permissions beyond the scope of this license may be available at mailto://info@dastardlybanana.com.
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class GlobalUpgradeEditor : Editor
{
    //var player : PlayerWeapons;
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.Separator();
        this.target.upgrade = EditorGUILayout.ObjectField(new UnityEngine.GUIContent("  Upgrade: ", "Upgrade object to be applied globally"), this.target.upgrade, Upgrade, true);
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("textField");
        EditorGUILayout.LabelField("Applicable Classes");
        EditorGUILayout.Separator();
        weaponClasses[] ws = weaponClasses.GetValues(weaponClasses);
        if (this.target.classesAllowed == null)
        {
            this.UpdateArray();
        }
        if (this.target.classesAllowed.length < ws.length)
        {
            this.UpdateArray();
        }
        int i = 0;
        while (i < ws.length)
        {
            weaponClasses w = ws[i];
            if (w == weaponClasses.Null)
            {
                break;
            }
            error className = w.ToString().Replace("_", " ");
            this.target.classesAllowed[i] = EditorGUILayout.Toggle(className, this.target.classesAllowed[i]);
            i++;
        }
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (UnityEngine.GUILayout.Button("Enable All", EditorStyles.miniButtonLeft))
        {
            i = 0;
            while (i < this.target.classesAllowed.length)
            {
                this.target.classesAllowed[i] = true;
                i++;
            }
        }
        if (UnityEngine.GUILayout.Button("Disable All", EditorStyles.miniButtonRight))
        {
            i = 0;
            while (i < this.target.classesAllowed.length)
            {
                this.target.classesAllowed[i] = false;
                i++;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
    }

    public virtual void UpdateArray()
    {
        object[] tempArray = this.target.classesAllowed;
        bool[] tempArray2 = tempArray.ToBuiltin(typeof(bool)) as bool[];
        this.target.classesAllowed = new bool[weaponClasses.GetValues(weaponClasses).length];
        int i = 0;
        while (i < tempArray2.Length)
        {
            this.target.calssesAllowed[i] = tempArray2[i];
            i++;
        }
        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}