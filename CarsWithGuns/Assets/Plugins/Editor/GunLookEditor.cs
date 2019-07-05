using UnityEditor;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
/*
	private var sensitivityX : float = 15F;
	private var sensitivityY : float = 15F;
	var sensitivityStandardX : float = 15F;
	var sensitivityStandardY : float = 15F;
	var sensitivityAimingX : float = 15F;
	var sensitivityAimingY : float = 15F;
	var sensitivityZ : float = 0.7F;
	var retSensitivity : float = -.5F;

	@HideInInspector
	var minimumX : float = 5F;
	@HideInInspector
	var maximumX : float = 3F;
	
	var xRange : float = 5F;
	var xRangeAim : float = 3F;
	

	var yRange : float = 5F;
	var yRangeAim : float = 3F;
	
	var zMoveRange : float = 10;
	var zMoveSensitivity : float = .5f;
	var zMoveAdjustSpeed : float = 4;
	
	var xMoveRange : float = 10;
	var xMoveSensitivity : float = .5f;
	var xMoveAdjustSpeed : float = 4;
	
	var xAirMoveRange : float = 10;
	var xAirMoveSensitivity : float = .5f;
	var xAirAdjustSpeed : float = 4;
	
	var zPosMoveRange : float = .13;
	var zPosMoveSensitivity : float = .5f;
	var zPosAdjustSpeed : float = 4;
	
	var xPosMoveRange : float = .13;
	var xPosMoveSensitivity : float = .5f;
	var xPosAdjustSpeed : float = 4;
*/
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class GunLookEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("toolbar");
        this.target.lookMotionOpen = EditorGUILayout.Foldout(this.target.lookMotionOpen, "Look Motion");
        EditorGUILayout.EndVertical();
        if (this.target.lookMotionOpen != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.useLookMotion = EditorGUILayout.Toggle("Use Look Motion", this.target.useLookMotion);
            if (this.target.useLookMotion != null)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginVertical("toolbar");
                EditorGUILayout.LabelField("Standard");
                EditorGUILayout.EndVertical();
                this.target.sensitivityStandardX = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Sensitivity", "Sensitivity for x look movement"), (float) this.target.sensitivityStandardX);
                this.target.sensitivityStandardY = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Y Sensitivity", "Sensitivity for y look movement"), (float) this.target.sensitivityStandardY);
                this.target.sensitivityStandardZ = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Sensitivity", "Sensitivity for z look movement"), (float) this.target.sensitivityStandardZ);
                this.target.retSensitivity = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Return Sensitivity", "Speed at which weapon returns to standard position"), (float) this.target.retSensitivity);
                EditorGUILayout.Separator();
                this.target.xRange = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Range", "Range of motion in degrees for x motion"), (float) this.target.xRange);
                this.target.yRange = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Y Range", "Range of motion in degrees for y motion"), (float) this.target.yRange);
                this.target.zRange = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Range", "Range of motion in degrees for z motion"), (float) this.target.zRange);
                EditorGUILayout.Separator();
                EditorGUILayout.BeginVertical("toolbar");
                EditorGUILayout.LabelField("Aim");
                EditorGUILayout.EndVertical();
                this.target.sensitivityAimingX = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Sensitivity", "Sensitivity for x look movement when aiming"), (float) this.target.sensitivityAimingX);
                this.target.sensitivityAimingY = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Y Sensitivity", "Sensitivity for y look movement when aiming"), (float) this.target.sensitivityAimingY);
                this.target.sensitivityAimingZ = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Sensitivity", "Sensitivity for z look movement when aiming"), (float) this.target.sensitivityAimingZ);
                EditorGUILayout.Separator();
                this.target.xRangeAim = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Range", "Range of motion in degrees for x motion when aiming"), (float) this.target.xRangeAim);
                this.target.yRangeAim = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Y Range", "Range of motion in degrees for y motion when aiming"), (float) this.target.yRangeAim);
                this.target.zRangeAim = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Range", "Range of motion in degrees for z motion when aiming"), (float) this.target.zRangeAim);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.BeginVertical("toolbar");
        this.target.walkMotionOpen = EditorGUILayout.Foldout(this.target.walkMotionOpen, "Walk Motion");
        EditorGUILayout.EndVertical();
        if (this.target.walkMotionOpen != null)
        {
            EditorGUILayout.BeginVertical("textField");
            this.target.useWalkMotion = EditorGUILayout.Toggle("Use Walk Motion", this.target.useWalkMotion);
            if (this.target.useWalkMotion != null)
            {
                /*EditorGUILayout.Separator();
					EditorGUILayout.BeginVertical("toolbar");
						EditorGUILayout.LabelField("Rotation");
					EditorGUILayout.EndVertical();
					
					target.zMoveRange = EditorGUILayout.FloatField(GUIContent("X Range","Range of angle in degrees through which weapon will move for z motion"), target.zMoveRange);
					target.zMoveSensitivity = EditorGUILayout.FloatField(GUIContent("X Sensitivity","Determines how much the weapons move based on z movement"), target.zMoveSensitivity);
					target.zMoveAdjustSpeed = EditorGUILayout.FloatField(GUIContent("X Speed","Determines how quickly the weapons move"), target.zMoveAdjustSpeed);
					EditorGUILayout.Separator();	
					
					target.xMoveRange = EditorGUILayout.FloatField(GUIContent("Z Range","Range of angle in degrees through which weapon will move for x motion"), target.xMoveRange);
					target.xMoveSensitivity = EditorGUILayout.FloatField(GUIContent("Z Sensitivity","Determines how much the weapons move based on x movement"), target.xMoveSensitivity);
					target.xMoveAdjustSpeed = EditorGUILayout.FloatField(GUIContent("Z Speed","Determines how quickly the weapons move"), target.xMoveAdjustSpeed);
					EditorGUILayout.Separator();	
					
					target.xAirMoveRange = EditorGUILayout.FloatField(GUIContent("Z Air Range","Range of angle in degrees through which weapon will move when airborne"), target.xAirMoveRange);
					target.xAirMoveSensitivity = EditorGUILayout.FloatField(GUIContent("Z Air Sensitivity","Determines how much the weapons move whe airborne"), target.xAirMoveSensitivity);
					target.xAirAdjustSpeed = EditorGUILayout.FloatField(GUIContent("Z Air Speed","Determines how quickly the weapons move"), target.xAirAdjustSpeed);
					EditorGUILayout.Separator();
					*/
                EditorGUILayout.BeginVertical("toolbar");
                EditorGUILayout.LabelField("Position");
                EditorGUILayout.EndVertical();
                this.target.zPosMoveRange = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Range", "Range of distance through which weapon will move for z motion"), (float) this.target.zPosMoveRange);
                this.target.zPosMoveSensitivity = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Sensitivity", "Determines how much the weapons move based on z movement"), (float) this.target.zPosMoveSensitivity);
                this.target.zPosAdjustSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("Z Speed", "Determines how quickly the weapons move"), (float) this.target.zPosAdjustSpeed);
                EditorGUILayout.Separator();
                this.target.xPosMoveRange = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Range", "Range of distance through which weapon will move for x motion"), (float) this.target.xPosMoveRange);
                this.target.xPosMoveSensitivity = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Sensitivity", "Determines how much the weapons move based on z movement"), (float) this.target.xPosMoveSensitivity);
                this.target.xPosAdjustSpeed = EditorGUILayout.FloatField(new UnityEngine.GUIContent("X Speed", "Determines how quickly the weapons move"), (float) this.target.xPosAdjustSpeed);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Separator();
    }

}