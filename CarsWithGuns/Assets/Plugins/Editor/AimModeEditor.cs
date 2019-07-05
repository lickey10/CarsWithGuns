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
public class AimModeEditor : Editor
{
    public bool foldout;
    public bool foldout1;
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.BeginVertical();
        this.target.aim = EditorGUILayout.Toggle("  Weapon Aims: ", this.target.aim);
        if (this.target.aim != null)
        {
            if (this.target.sightsZoom1 == null)
            {
                this.target.scoped1 = EditorGUILayout.Toggle("  Has Scope: ", this.target.scoped1);
            }
            if (this.target.scoped1 == null)
            {
                this.target.sightsZoom1 = EditorGUILayout.Toggle("  Zoom Down Sights: ", this.target.sightsZoom1);
                this.target.crosshairWhenAiming = EditorGUILayout.Toggle("  Show Crosshair: ", this.target.crosshairWhenAiming);
            }
            else
            {
                this.target.scopeTexture = EditorGUILayout.ObjectField("  Scope Texture: ", this.target.scopeTexture, typeof(Texture), false);
                this.target.st169 = EditorGUILayout.ObjectField("  Scope Texture 16:9: ", this.target.st169, typeof(Texture), false);
                this.target.st1610 = EditorGUILayout.ObjectField("  Scope Texture 16:10: ", this.target.st1610, typeof(Texture), false);
                this.target.st54 = EditorGUILayout.ObjectField("  Scope Texture 5:4: ", this.target.st54, typeof(Texture), false);
                this.target.st43 = EditorGUILayout.ObjectField("  Scope Texture 4:3: ", this.target.st43, typeof(Texture), false);
            }
            if ((this.target.scoped1 != null) || (this.target.sightsZoom1 != null))
            {
                this.target.zoomFactor1 = EditorGUILayout.FloatField("  Zoom Factor: ", (float) this.target.zoomFactor1);
            }
        }
        this.target.aimRate = EditorGUILayout.FloatField("  Aim Rate: ", (float) this.target.aimRate);
        this.target.sprintRate = EditorGUILayout.FloatField("  Sprint Rate: ", (float) this.target.sprintRate);
        this.target.retRate = EditorGUILayout.FloatField("  Return Rate: ", (float) this.target.retRate);
        EditorGUILayout.Separator();
        this.target.overrideSprint = EditorGUILayout.Toggle("  Override Sprint: ", this.target.overrideSprint);
        if (this.target.overrideSprint != null)
        {
            this.target.sprintDuration = EditorGUILayout.FloatField("  Sprint Duration: ", (float) this.target.sprintDuration);
            this.target.sprintAddStand = EditorGUILayout.FloatField("  Standing Sprint Return Rate: ", (float) this.target.sprintAddStand);
            this.target.sprintAddWalk = EditorGUILayout.FloatField("  Walking Sprint Return Rate: ", (float) this.target.sprintAddWalk);
            this.target.sprintMin = EditorGUILayout.FloatField("  Sprint Minimum: ", (float) this.target.sprintMin);
            this.target.recoverDelay = EditorGUILayout.FloatField("  Sprint Recovery Delay: ", (float) this.target.recoverDelay);
            this.target.exhaustedDelay = EditorGUILayout.FloatField("  Exhausted Recovery Delay: ", (float) this.target.exhaustedDelay);
        }
        EditorGUILayout.Separator();
        this.target.hasSecondary = EditorGUILayout.Toggle("  Has Secondary: ", this.target.hasSecondary);
        this.target.secondaryAim = EditorGUILayout.Toggle("  Weapon Aims: ", this.target.secondaryAim);
        if ((this.target.secondaryAim != null) && (this.target.hasSecondary != null))
        {
            if (this.target.sightsZoom2 == null)
            {
                this.target.scoped2 = EditorGUILayout.Toggle("  Has Scope: ", this.target.scoped2);
            }
            if (this.target.scoped2 == null)
            {
                this.target.sightsZoom2 = EditorGUILayout.Toggle("  Zoom Down Sights: ", this.target.sightsZoom2);
            }
            if ((this.target.scoped2 != null) || (this.target.sightsZoom2 != null))
            {
                this.target.zoomFactor2 = EditorGUILayout.FloatField("  Zoom Factor: ", (float) this.target.zoomFactor2);
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        this.foldout = EditorGUILayout.Foldout(this.foldout, "Configure Primary Weapon Positions");
        if (this.foldout)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Move to Hip Position", "Move Weapon to Hip Position"), "miniButton"))
            {
                this.target.transform.localPosition = this.target.hipPosition1;
                this.target.transform.localEulerAngles = this.target.hipRotation1;
            }
            if (GUILayout.Button(new GUIContent("Configure Hip Position", "Set Hip Position to Current Position"), "miniButton"))
            {
                this.target.hipPosition1 = this.target.transform.localPosition;
                this.target.hipRotation1 = this.target.transform.localEulerAngles;
                EditorUtility.SetDirty(this.target);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("textField");
            this.target.hipPosition1 = EditorGUILayout.Vector3Field("hipPosition", this.target.hipPosition1);
            this.target.hipRotation1 = EditorGUILayout.Vector3Field("hipRotation", this.target.hipRotation1);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
            if (this.target.aim != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Move to Aim Position", "Move Weapon to Aim Position"), "miniButton"))
                {
                    this.target.transform.localPosition = this.target.aimPosition1;
                    this.target.transform.localEulerAngles = this.target.aimRotation1;
                }
                if (GUILayout.Button(new GUIContent("Configure Aim Position", "Set Aim Position to Current Position"), "miniButton"))
                {
                    this.target.aimPosition1 = this.target.transform.localPosition;
                    this.target.aimRotation1 = this.target.transform.localEulerAngles;
                    EditorUtility.SetDirty(this.target);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical("textField");
                this.target.aimPosition1 = EditorGUILayout.Vector3Field("aimPosition", this.target.aimPosition1);
                this.target.aimRotation1 = EditorGUILayout.Vector3Field("aimRotation", this.target.aimRotation1);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Move to Sprint Position", "Move Weapon to Sprint Position"), "miniButton"))
            {
                this.target.transform.localPosition = this.target.sprintPosition1;
                this.target.transform.localEulerAngles = this.target.sprintRotation1;
            }
            if (GUILayout.Button(new GUIContent("Configure Sprint Position", "Set Sprint Position to Position"), "miniButton"))
            {
                this.target.sprintPosition1 = this.target.transform.localPosition;
                this.target.sprintRotation1 = this.target.transform.localEulerAngles;
                EditorUtility.SetDirty(this.target);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("textField");
            this.target.sprintPosition1 = EditorGUILayout.Vector3Field("sprintPosition", this.target.sprintPosition1);
            this.target.sprintRotation1 = EditorGUILayout.Vector3Field("sprintRotation", this.target.sprintRotation1);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
        ///****************************
        ///****************************
        if (this.target.hasSecondary != null)
        {
            this.foldout1 = EditorGUILayout.Foldout(this.foldout1, "Configure Secondary Weapon Positions");
            if (this.foldout1)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Move to Hip Position", "Move Weapon to Hip Position"), "miniButton"))
                {
                    this.target.transform.localPosition = this.target.hipPosition2;
                    this.target.transform.localEulerAngles = this.target.hipRotation2;
                }
                if (GUILayout.Button(new GUIContent("Configure Hip Position", "Set Hip Position to Current Position"), "miniButton"))
                {
                    this.target.hipPosition2 = this.target.transform.localPosition;
                    this.target.hipRotation2 = this.target.transform.localEulerAngles;
                    EditorUtility.SetDirty(this.target);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical("textField");
                this.target.hipPosition2 = EditorGUILayout.Vector3Field("hipPosition", this.target.hipPosition2);
                this.target.hipRotation2 = EditorGUILayout.Vector3Field("hipRotation", this.target.hipRotation2);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
                if (this.target.secondaryAim != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("Move to Aim Position", "Move Weapon to Aim Position"), "miniButton"))
                    {
                        this.target.transform.localPosition = this.target.aimPosition2;
                        this.target.transform.localEulerAngles = this.target.aimRotation2;
                    }
                    if (GUILayout.Button(new GUIContent("Configure Aim Position", "Set Aim Position to Current Position"), "miniButton"))
                    {
                        this.target.aimPosition2 = this.target.transform.localPosition;
                        this.target.aimRotation2 = this.target.transform.localEulerAngles;
                        EditorUtility.SetDirty(this.target);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginVertical("textField");
                    this.target.aimPosition2 = EditorGUILayout.Vector3Field("aimPosition", this.target.aimPosition2);
                    this.target.aimRotation2 = EditorGUILayout.Vector3Field("aimRotation", this.target.aimRotation2);
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Move to Sprint Position", "Move Weapon to Sprint Position"), "miniButton"))
                {
                    this.target.transform.localPosition = this.target.sprintPosition2;
                    this.target.transform.localEulerAngles = this.target.sprintRotation2;
                }
                if (GUILayout.Button(new GUIContent("Configure Sprint Position", "Set Sprint Position to Position"), "miniButton"))
                {
                    this.target.sprintPosition2 = this.target.transform.localPosition;
                    this.target.sprintRotation2 = this.target.transform.localEulerAngles;
                    EditorUtility.SetDirty(this.target);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical("textField");
                this.target.sprintPosition2 = EditorGUILayout.Vector3Field("sprintPosition", this.target.sprintPosition2);
                this.target.sprintRotation2 = EditorGUILayout.Vector3Field("sprintRotation", this.target.sprintRotation2);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}