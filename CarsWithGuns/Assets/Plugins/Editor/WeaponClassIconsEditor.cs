using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class WeaponClassIconsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.BeginVertical();
        foreach (weaponClasses w in weaponClasses.GetValues(weaponClasses))
        {
            if (w == weaponClasses.Null) // hide the Null Weapon				
            {
                break;
            }
            this.target.weaponClassTextures[w] = EditorGUILayout.ObjectField(w.ToString().Replace("_", " "), this.target.weaponClassTextures[w], UnityEngine.Texture, false);
        }
        EditorGUILayout.EndVertical();
    }

}