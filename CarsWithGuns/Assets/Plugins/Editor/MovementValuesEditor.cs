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
public class MovementValuesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (this.target.CM == null)
        {
            this.target.CM = this.target.gameObject.GetComponent("CharacterMotorDB");
        }
        this.target.defaultFoldout = EditorGUILayout.Foldout(this.target.defaultFoldout, "Standard Movement");
        if (this.target.defaultFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.defaultForwardSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Forward Speed: ", "Speed in forward direction for normal movement"), (float) this.target.defaultForwardSpeed);
            this.target.defaultSidewaysSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sideways Speed: ", "Speed moving left or right for normal movement"), (float) this.target.defaultSidewaysSpeed);
            this.target.defaultBackwardsSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Backwards Speed: ", "Speed moving left or right for normal movement"), (float) this.target.defaultBackwardsSpeed);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        this.target.crouchFoldout = EditorGUILayout.Foldout(this.target.crouchFoldout, "Crouched Movement");
        if (this.target.crouchFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.maxCrouchSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Forward Speed: ", "Speed in forward direction while crouching"), (float) this.target.maxCrouchSpeed);
            this.target.crouchSidewaysSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sideways Speed: ", "Speed moving left or right while crouching"), (float) this.target.crouchSidewaysSpeed);
            this.target.crouchBackwardsSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Backwards Speed: ", "Speed moving backwards while crouching"), (float) this.target.crouchBackwardsSpeed);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        this.target.proneFoldout = EditorGUILayout.Foldout(this.target.proneFoldout, "Prone Movement");
        if (this.target.proneFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.maxProneSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Forward Speed: ", "Speed in forward direction while prone"), (float) this.target.maxProneSpeed);
            this.target.proneSidewaysSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sideways Speed: ", "Speed moving left or right while prone"), (float) this.target.proneSidewaysSpeed);
            this.target.proneBackwardsSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Backwards Speed: ", "Speed moving backwards while prone"), (float) this.target.proneBackwardsSpeed);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        this.target.aimFoldout = EditorGUILayout.Foldout(this.target.aimFoldout, "Aiming Movement");
        if (this.target.aimFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.maxAimSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Forward Speed: ", "Speed in forward direction while aiming"), (float) this.target.maxAimSpeed);
            this.target.aimSidewaysSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sideways Speed: ", "Speed moving left or right while aiming"), (float) this.target.aimSidewaysSpeed);
            this.target.aimBackwardsSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Backwards Speed: ", "Speed moving backwards while aiming"), (float) this.target.aimBackwardsSpeed);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        this.target.sprintFoldout = EditorGUILayout.Foldout(this.target.sprintFoldout, "Sprint Movement");
        if (this.target.sprintFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.maxSprintSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Forward Speed: ", "Speed in forward direction while sprinting"), (float) this.target.maxSprintSpeed);
            this.target.minSprintSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Minimum Speed: ", "Minimum speed to remain sprinting"), (float) this.target.minSprintSpeed);
            this.target.sprintSidewaysSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sideways Speed: ", "Speed moving left or right while sprinting"), (float) this.target.sprintSidewaysSpeed);
            EditorGUILayout.Separator();
            this.target.sprintDuration = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Sprint Duration: ", "How long can the player sprint (in seconds)?"), (float) this.target.sprintDuration);
            this.target.sprintAddStand = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Standing Sprint Return: ", "How quickly does sprint return when standing?"), (float) this.target.sprintAddStand);
            this.target.sprintAddWalk = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Walking Sprint Return: ", "How quickly does sprint return when walking?"), (float) this.target.sprintAddWalk);
            this.target.sprintMin = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Minimum Sprint: ", "Minimum sprint value to sprint"), (float) this.target.sprintMin);
            this.target.recoverDelay = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Recover Delay: ", "Time in seconds after sprinting until sprint begins returning"), (float) this.target.recoverDelay);
            this.target.exhaustedDelay = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Recover Delay: ", "Time in seconds after sprinting to exhaustion until sprint begins returning"), (float) this.target.exhaustedDelay);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        this.target.jumpFoldout = EditorGUILayout.Foldout(this.target.jumpFoldout, "Jumping Movement");
        if (this.target.jumpFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.CM.movement.gravity = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Gravity: ", "Gravity Factor"), (float) this.target.CM.movement.gravity);
            this.target.CM.movement.maxFallSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Max Fall Speed: ", "Maximum fall speed of player"), (float) this.target.CM.movement.maxFallSpeed);
            this.target.CM.movement.fallDamageStart = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Fall Damage Start: ", "Fall speed at which damage begins to be applied"), (float) this.target.CM.movement.fallDamageStart);
            this.target.CM.movement.fallDamageEnd = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Fall Damage End: ", "Fall speed at which maximum damage is applied"), (float) this.target.CM.movement.fallDamageEnd);
            this.target.CM.movement.fallDamageMax = EditorGUILayout.FloatField(new UnityEngine.GUIContent("  Max Fall Damage: ", "Fall Damage applied at end speed. Fall damage scales linearly toward this from start"), (float) this.target.CM.movement.fallDamageMax);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

}