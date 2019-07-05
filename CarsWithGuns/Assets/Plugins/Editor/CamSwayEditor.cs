using UnityEditor;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class CamSwayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        /*	if(target.CM == null){
			target.CM = target.gameObject.GetComponent("CharacterMotorDB");
		}*/
        EditorGUILayout.Separator();
        //Move Sway
        EditorGUILayout.BeginVertical("toolbar");
        EditorGUILayout.LabelField("Move Sway");
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("textField");
        this.target.moveSwayRate = EditorGUILayout.Vector2Field("   Move Sway Rate: ", this.target.moveSwayRate);
        this.target.moveSwayAmplitude = EditorGUILayout.Vector2Field("   Move Sway Amplitude: ", this.target.moveSwayAmplitude);
        EditorGUILayout.EndVertical();
        //Run Sway
        EditorGUILayout.BeginVertical("toolbar");
        EditorGUILayout.LabelField("Run Sway");
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("textField");
        this.target.runSwayRate = EditorGUILayout.Vector2Field("   Run Sway Rate: ", this.target.runSwayRate);
        this.target.runSwayAmplitude = EditorGUILayout.Vector2Field("   Run Sway Amplitude: ", this.target.runSwayAmplitude);
        EditorGUILayout.EndVertical();
        //Idle Sway
        EditorGUILayout.BeginVertical("toolbar");
        EditorGUILayout.LabelField("Idle Sway");
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("textField");
        this.target.idleSwayRate = EditorGUILayout.Vector2Field("   Idle Sway Rate: ", this.target.idleSwayRate);
        this.target.idleAmplitude = EditorGUILayout.Vector2Field("   Idle Sway Amplitude: ", this.target.idleAmplitude);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}