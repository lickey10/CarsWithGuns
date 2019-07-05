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
public class GunChildAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Separator();
        this.target.gs = EditorGUILayout.ObjectField(new UnityEngine.GUIContent("  Gun Script: ", "This is the GunScript of this weapon"), this.target.gs, GunScript, true);
        this.target.hasSecondary = EditorGUILayout.Toggle("  Has Secondary: ", this.target.hasSecondary);
        this.target.melee = EditorGUILayout.Toggle("  Is Melee: ", this.target.melee);
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Animations", "Animation Name");
        EditorGUILayout.Separator();
        if (this.target.melee == null)
        {
            this.target.reloadAnim = EditorGUILayout.TextField("  Reload: ", this.target.reloadAnim);
            this.target.emptyReloadAnim = EditorGUILayout.TextField("  Empty Reload: ", this.target.emptyReloadAnim);
            this.target.fireAnim = EditorGUILayout.TextField("  Fire: ", this.target.fireAnim);
            this.target.emptyFireAnim = EditorGUILayout.TextField("  Dry Fire: ", this.target.emptyFireAnim);
        }
        this.target.takeOutAnim = EditorGUILayout.TextField("  Take Out: ", this.target.takeOutAnim);
        this.target.putAwayAnim = EditorGUILayout.TextField("  Put Away: ", this.target.putAwayAnim);
        if (this.target.melee == null)
        {
            this.target.reloadIn = EditorGUILayout.TextField("  Enter Reload: ", this.target.reloadIn);
            this.target.reloadOut = EditorGUILayout.TextField("  Exit Reload: ", this.target.reloadOut);
        }
        this.target.walkAnimation = EditorGUILayout.TextField("  Walk: ", this.target.walkAnimation);
        this.target.sprintAnimation = EditorGUILayout.TextField("  Sprint: ", this.target.sprintAnimation);
        this.target.walkSpeedModifier = EditorGUILayout.FloatField("  Walk Speed Modifier: ", (float) this.target.walkSpeedModifier);
        this.target.walkWhenAiming = EditorGUILayout.Toggle("  Walk When Aiming: ", this.target.walkWhenAiming);
        this.target.nullAnim = EditorGUILayout.TextField("  Null: ", this.target.nullAnim);
        this.target.idleAnim = EditorGUILayout.TextField("  Idle: ", this.target.idleAnim);
        if (this.target.hasSecondary != null)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Secondary Animations", "Animation Name");
            EditorGUILayout.Separator();
            this.target.secondaryReloadAnim = EditorGUILayout.TextField("  Reload: ", this.target.secondaryReloadAnim);
            this.target.secondaryReloadEmpty = EditorGUILayout.TextField("  Empty Reload: ", this.target.secondaryReloadEmpty);
            this.target.secondaryFireAnim = EditorGUILayout.TextField("  Fire: ", this.target.secondaryFireAnim);
            this.target.secondaryEmptyFireAnim = EditorGUILayout.TextField("  Dry Fire: ", this.target.secondaryEmptyFireAnim);
            this.target.enterSecondaryAnim = EditorGUILayout.TextField("  Enter Secondary: ", this.target.enterSecondaryAnim);
            this.target.exitSecondaryAnim = EditorGUILayout.TextField("  Exit Secondary: ", this.target.exitSecondaryAnim);
            this.target.secondaryWalkAnim = EditorGUILayout.TextField("  Walk: ", this.target.secondaryWalkAnim);
            this.target.secondarySprintAnim = EditorGUILayout.TextField("  Sprint: ", this.target.secondarySprintAnim);
            this.target.secondaryNullAnim = EditorGUILayout.TextField("  Null: ", this.target.secondaryNullAnim);
            this.target.secondaryIdleAnim = EditorGUILayout.TextField("  Idle: ", this.target.secondaryIdleAnim);
        }
        if (this.target.melee != null)
        {
            EditorGUILayout.Separator();
            this.target.animCount = EditorGUILayout.IntField("  Animations: ", (int) this.target.animCount);
            this.target.random = EditorGUILayout.Toggle("  Random: ", this.target.random);
            if (this.target.random == null)
            {
                this.target.resetTime = EditorGUILayout.FloatField("  Chain Reset Time: ", (float) this.target.resetTime);
            }
            int i = 0;
            while (i < this.target.animCount)
            {
                this.target.fireAnims[i] = EditorGUILayout.TextField("    Attack: ", this.target.fireAnims[i]);
                this.target.reloadAnims[i] = EditorGUILayout.TextField("    Return: ", this.target.reloadAnims[i]);
                EditorGUILayout.Separator();
                i++;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}