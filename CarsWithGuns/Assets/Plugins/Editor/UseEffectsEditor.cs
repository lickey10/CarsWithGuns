using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class UseEffectsEditor : Editor
{
    public EffectsManager manager;
    public int prevIndex;
    public override void OnInspectorGUI()
    {
        this.manager = FindObjectOfType(EffectsManager);
        if (this.manager != null)
        {
            if (this.manager.setNameArray != null)
            {
                if (this.manager.setArray[0] != null)
                {
                    this.prevIndex = (int) this.target.setIndex;
                    this.target.setIndex = EditorGUILayout.Popup(this.target.setIndex, this.manager.setNameArray);
                    if (!(this.prevIndex == this.target.setIndex))
                    {
                        // set all colliders of the same name to use the same set.
                        Collider[] colliders = (Collider[]) UnityEngine.Object.FindObjectsOfType(typeof(Collider));
                        foreach (Collider c in colliders)
                        {
                            if (c.name == this.target.transform.name)
                            {
                                if (c.GetComponent(UseEffects))
                                {
                                    c.GetComponent(UseEffects).setIndex = this.target.setIndex;
                                }
                            }
                        }
                        this.prevIndex = (int) this.target.setIndex;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No sets exist", "");
                }
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}