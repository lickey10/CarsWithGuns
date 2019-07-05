using UnityEngine;
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
public class SmartCrosshairEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.target.useTexture = EditorGUILayout.Toggle("  Use Custom:", this.target.useTexture);
        if (this.target.useTexture != null)
        {
            this.target.crosshairObj = EditorGUILayout.ObjectField(new GUIContent("  Crosshair Object: ", "The object with the crosshair texture"), this.target.crosshairObj, typeof(GameObject), true);
            this.target.crosshairSize = EditorGUILayout.FloatField(new GUIContent("  Crosshair Size: ", "The base scale of the crosshair object"), (float) this.target.crosshairSize);
            this.target.scale = EditorGUILayout.Toggle(new GUIContent("  Scale Crosshair:", "Does the crosshair scale based on accuracy?"), this.target.scale);
            if (this.target.scale != null)
            {
                this.target.minimumSize = EditorGUILayout.FloatField(new GUIContent("  Min Size: ", "The minimum scale of custom crosshairs"), (float) this.target.minimumSize);
                this.target.maximumSize = EditorGUILayout.FloatField(new GUIContent("  Max Size: ", "The maximum scale of custom crosshairs"), (float) this.target.maximumSize);
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            Vector2 temp1 = EditorGUILayout.Vector2Field("Crosshair Box Size: ", new Vector2(this.target.length1, this.target.width1));
            EditorGUILayout.EndHorizontal();
            this.target.length1 = temp1.y;
            this.target.width1 = temp1.x;
            this.target.colorFoldout = EditorGUILayout.Foldout(this.target.colorFoldout, "Crosshair Color:");
            if (this.target.colorFoldout != null)
            {
                this.target.colorDist = EditorGUILayout.FloatField(new GUIContent("  Color Distance: ", "How far can an object be to cause color change? Negative value means infinite"), (float) this.target.colorDist);
                this.target.crosshairTexture = EditorGUILayout.ObjectField(new GUIContent("        Default: ", "This texture determines the color of the Smart Crosshair"), this.target.crosshairTexture, typeof(Texture), false);
                this.target.friendTexture = EditorGUILayout.ObjectField(new GUIContent("        Friend: ", "This texture determines the color of the Smart Crosshair when looking at friendly targets"), this.target.friendTexture, typeof(Texture), false);
                this.target.foeTexture = EditorGUILayout.ObjectField(new GUIContent("        Hostile: ", "This texture determines the color of the Smart Crosshair when looking at hostile targets"), this.target.foeTexture, typeof(Texture), false);
                this.target.otherTexture = EditorGUILayout.ObjectField(new GUIContent("        Other: ", "This texture determines the color of the Smart Crosshair when looking at 'other' targets"), this.target.otherTexture, typeof(Texture), false);
            }
        }
        this.target.hitEffectFoldout = EditorGUILayout.Foldout(this.target.hitEffectFoldout, "Hit Indicator:");
        if (this.target.hitEffectFoldout != null)
        {
            EditorGUILayout.BeginHorizontal();
            this.target.hitEffectTexture = EditorGUILayout.ObjectField(new GUIContent("        Texture: ", "The texture to be displayed over the crosshair when an enemy is shot"), this.target.hitEffectTexture, typeof(Texture), false);
            EditorGUILayout.BeginVertical();
            Vector2 temp = EditorGUILayout.Vector2Field("Size: ", new Vector2(this.target.hitLength, this.target.hitWidth));
            this.target.hitLength = temp.y;
            this.target.hitWidth = temp.x;
            this.target.hitEffectOffset = EditorGUILayout.Vector2Field("Hit Effect Offset: ", this.target.hitEffectOffset);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            this.target.hitSound = EditorGUILayout.ObjectField(new GUIContent("        Sound: ", "The sound to play with the hit indicator"), this.target.hitSound, typeof(AudioClip), false);
        }
        this.target.debug = EditorGUILayout.Toggle(new GUIContent("  Debug Mode: ", "When Debug Mode is on the crosshair does not disappear when aiming"), this.target.debug);
        EditorGUILayout.Separator();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}