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
public class GunScriptEditor : Editor
{
    public bool displayGun;
    public bool gunDisplayed;
    public error gunTipo;
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        //EditorGUILayout.BeginVertical();
        this.target.gunType = EditorGUILayout.EnumPopup(new GUIContent("  Gun Type: ", "The basic type of weapon - choose 'hitscan' for a basic bullet-based weapon"), this.target.gunType);
        this.gunTipo = this.target.gunType;
        if (this.gunTipo == gunTypes.spray)
        {
            this.target.sprayObj = EditorGUILayout.ObjectField(new GUIContent("  Spray Object: ", "Spray weapons need an attached object with a particle collider and the script SprayScript"), this.target.sprayObj, typeof(GameObject), true);
        }
        else
        {
            if (this.gunTipo == gunTypes.launcher)
            {
                this.target.launchPosition = EditorGUILayout.ObjectField(new GUIContent("  Launch Position: ", "The projectile will be instantiated at the position of the object in this field"), this.target.launchPosition, typeof(GameObject), true);
            }
        }
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Open All", "Open all the foldout menus"), "miniButton"))
        {
            this.target.shotPropertiesFoldout = true;
            this.target.firePropertiesFoldout = true;
            this.target.accuracyFoldout = true;
            this.target.altFireFoldout = true;
            this.target.ammoReloadFoldout = true;
            this.target.audioVisualFoldout = true;
        }
        if (GUILayout.Button(new GUIContent("Close All", "Close all the foldout menus"), "miniButton"))
        {
            this.target.shotPropertiesFoldout = false;
            this.target.firePropertiesFoldout = false;
            this.target.accuracyFoldout = false;
            this.target.altFireFoldout = false;
            this.target.ammoReloadFoldout = false;
            this.target.audioVisualFoldout = false;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        //Shot properties
        if (this.gunTipo != gunTypes.melee)
        {
            EditorGUILayout.BeginVertical("toolbar");
            this.target.shotPropertiesFoldout = EditorGUILayout.Foldout(this.target.shotPropertiesFoldout, "Shot Properties (damage etc.):");
            EditorGUILayout.EndVertical();
        }
        else
        {
            this.target.shotPropertiesFoldout = false;
        }
        if (this.target.shotPropertiesFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            if (this.gunTipo == gunTypes.launcher)
            {
                this.target.projectile = EditorGUILayout.ObjectField(new GUIContent("  Projectile: ", "This is the actual GameObject to be instantiated from the launcher"), this.target.projectile, typeof(Rigidbody), false);
                this.target.initialSpeed = EditorGUILayout.FloatField(new GUIContent("  Initial Speed: ", "The initial speed each projectile will have when fired"), (float) this.target.initialSpeed);
            }
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.spray))
            {
                this.target.range = EditorGUILayout.FloatField(new GUIContent("  Range: ", "The maximum range this gun can hit at"), (float) this.target.range);
                this.target.force = EditorGUILayout.FloatField(new GUIContent("  Force: ", "The force which is applied to the object we hit"), (float) this.target.force);
                if (this.gunTipo == gunTypes.spray)
                {
                    this.target.damage = EditorGUILayout.FloatField(new GUIContent("  Damage Per Second:  ", "The damage done by the weapon over each second of continous fire"), (float) this.target.damage);
                }
                else
                {
                    this.target.penetrateVal = EditorGUILayout.FloatField(new GUIContent("  Penetration Level: ", "If your scene is set up to use bullet penetration, this determines the piercing ability of this weapon"), (float) this.target.penetrateVal);
                    this.target.damage = EditorGUILayout.FloatField(new GUIContent("  Damage: ", "The damage done per bullet by this weapon"), (float) this.target.damage);
                }
                if (this.target.chargeWeapon != null)
                {
                    this.target.chargeCoefficient = EditorGUILayout.FloatField(new GUIContent("  Charge Coefficient: ", "Multiply this weapon's damage by this number to the power of the current charge level (A value of 1.1 would lead to a 10% increase in damage after charging for one second, for example"), (float) this.target.chargeCoefficient);
                }
                if (this.gunTipo == gunTypes.spray)
                {
                    this.target.hasFalloff = true;
                }
                else
                {
                    this.target.hasFalloff = EditorGUILayout.Toggle(new GUIContent("  Use Damage Falloff: ", "Does this weapon's damage change with distance?"), this.target.hasFalloff);
                }
                if (this.target.hasFalloff != null)
                {
                    float tempDamageDisplay = (float) (((float) this.target.damage) * Mathf.Pow((float) this.target.falloffCoefficient, (float) ((this.target.maxFalloffDist - this.target.minFalloffDist) / this.target.falloffDistanceScale)));
                    EditorGUILayout.LabelField("  Damage at max falloff: ", "" + tempDamageDisplay);
                    float tempForceDisplay = (float) (((float) this.target.force) * Mathf.Pow((float) this.target.forceFalloffCoefficient, (float) ((this.target.maxFalloffDist - this.target.minFalloffDist) / this.target.falloffDistanceScale)));
                    EditorGUILayout.LabelField("  Force at max falloff: ", "" + tempForceDisplay);
                    this.target.minFalloffDist = EditorGUILayout.FloatField(new GUIContent("  Min. Falloff Distance: ", "Falloff is not applied to hits closer than this distance"), (float) this.target.minFalloffDist);
                    this.target.maxFalloffDist = EditorGUILayout.FloatField(new GUIContent("  Max. Falloff Distance: ", "Falloff is not applied to hits past this distance"), (float) this.target.maxFalloffDist);
                    this.target.falloffCoefficient = EditorGUILayout.FloatField(new GUIContent("  Damage Coefficient: ", "The weapon's damage is multiplied by this for every unit of distance"), (float) this.target.falloffCoefficient);
                    this.target.forceFalloffCoefficient = EditorGUILayout.FloatField(new GUIContent("  Force Coefficient: ", "The force applied is multiplied by this every unit of distance"), (float) this.target.forceFalloffCoefficient);
                    this.target.falloffDistanceScale = EditorGUILayout.FloatField(new GUIContent("  Falloff Distance Scale: ", "This defines how many unity meters make up one 'unit' of distance in falloff calculations"), (float) this.target.falloffDistanceScale);
                }
                if (this.gunTipo == gunTypes.melee)
                {
                    this.target.damage = 0;
                    this.target.force = 0;
                }
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
            }
        }
        //Fire Properties
        if (this.gunTipo != gunTypes.spray)
        {
            EditorGUILayout.BeginVertical("toolbar");
            this.target.firePropertiesFoldout = EditorGUILayout.Foldout(this.target.firePropertiesFoldout, "Fire Properties (fire rate etc.):");
            EditorGUILayout.EndVertical();
        }
        else
        {
            this.target.firePropertiesFoldout = false;
        }
        if (this.target.firePropertiesFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            if (this.gunTipo == gunTypes.melee)
            {
                this.target.shotCount = 0;
                this.target.fireRate = EditorGUILayout.FloatField(new GUIContent("  Attack Rate:  ", "Attacks per second"), (float) this.target.fireRate);
                this.target.delay = EditorGUILayout.FloatField(new GUIContent("  Damage Delay:  ", "How long into the attack does the hitbox activate, in seconds. The hit box is active during this time"), (float) this.target.delay);
                this.target.reloadTime = EditorGUILayout.FloatField(new GUIContent("  Recovery Time:  ", "The time, in seconds, after each attack before it is possible to attack again"), (float) this.target.reloadTime);
            }
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                if (this.gunTipo == gunTypes.hitscan)
                {
                    this.target.shotCount = EditorGUILayout.IntField(new GUIContent("  Shot Count: ", "The number of shots fired per pull of the trigger"), (int) this.target.shotCount);
                }
                else
                {
                    this.target.projectileCount = EditorGUILayout.FloatField(new GUIContent("  Projectile Count: ", "The number of projectiles fired with every pull of the trigger"), (float) this.target.projectileCount);
                }
                this.target.fireRate = EditorGUILayout.FloatField(new GUIContent("  Fire Rate: ", "The time in seconds after firing before the weapon can be fired again"), (float) this.target.fireRate);
                this.target.chargeWeapon = EditorGUILayout.Toggle(new GUIContent("  Charge Weapon: ", "Charge weapons are weapons which can be 'charged up' by holding down fire. Charge is measured in an internal variable called 'chargeLevel'"), this.target.chargeWeapon);
                if (this.target.chargeWeapon != null)
                {
                    this.target.autoFire = false;
                    this.target.minCharge = EditorGUILayout.FloatField(new GUIContent("  Minimum Charge: ", "The weapon cannot fire unless it has charged up to at least this value"), (float) this.target.minCharge);
                    this.target.maxCharge = EditorGUILayout.FloatField(new GUIContent("  Maximum Charge: ", "The weapon cannot charge up beyond this value"), (float) this.target.maxCharge);
                    this.target.forceFire = EditorGUILayout.Toggle(new GUIContent("  Must Fire When Charged: ", "If this is checked, the weapon will be automatically discharged the moment it is fully charged"), this.target.forceFire);
                    if (this.target.forceFire != null)
                    {
                        this.target.chargeAuto = EditorGUILayout.Toggle(new GUIContent("  Charge after force release: ", "When the weapon dischrages because it reaches maximum charge, and chargeAuto is enabled, the weapon will begin charging again immediately if able. If unchecked, the player will have to release the mouse and press it to start charging again"), this.target.chargeAuto);
                    }
                }
                if (this.target.autoFire == null)
                {
                    this.target.burstFire = EditorGUILayout.Toggle(new GUIContent("  Burst Fire: ", "Does this weapon fire in bursts?"), this.target.burstFire);
                }
                if (this.target.burstFire != null)
                {
                    this.target.burstCount = EditorGUILayout.IntField(new GUIContent("  Burst Count: ", "How many shots fired per burst?"), (int) this.target.burstCount);
                    this.target.burstTime = EditorGUILayout.FloatField(new GUIContent("  Burst Time: ", "How long does it take to fire the full burst?"), (float) this.target.burstTime);
                }
                if ((this.target.burstFire == null) && (this.target.chargeWeapon == null))
                {
                    this.target.autoFire = EditorGUILayout.Toggle(new GUIContent("  Full Auto: ", "Is this weapon fully automatic?"), this.target.autoFire);
                }
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        //Accuracy
        if (this.gunTipo != gunTypes.melee)
        {
            EditorGUILayout.BeginVertical("toolbar");
            this.target.accuracyFoldout = EditorGUILayout.Foldout(this.target.accuracyFoldout, "Accuracy Properties:");
            EditorGUILayout.EndVertical();
        }
        else
        {
            this.target.accuracyFoldout = false;
        }
        if (this.target.accuracyFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                this.target.standardSpread = EditorGUILayout.FloatField(new GUIContent("  Standard Spread: ", "The weapon's spread when firing from the hip and standing still. A spread of 1 means the shot could go 90 degrees in any direction; .5 would be 45 degrees"), (float) this.target.standardSpread);
                this.target.standardSpreadRate = EditorGUILayout.FloatField(new GUIContent("  Spread Rate: ", "The rate at which the weapon's spread increases while firing from the hip. This value is added to the spread after every shot"), (float) this.target.standardSpreadRate);
                this.target.spDecRate = EditorGUILayout.FloatField(new GUIContent("  Spread Decrease Rate: ", "The rate at which the spread returns to normal."), (float) this.target.spDecRate);
                this.target.maxSpread = EditorGUILayout.FloatField(new GUIContent("  Maximum Spread: ", "The weapon's spread will not naturally exceed this value."), (float) this.target.maxSpread);
                EditorGUILayout.Separator();
                this.target.aimSpread = EditorGUILayout.FloatField(new GUIContent("  Aim Spread: ", "The weapon's spread when firing in aim mode.  A spread of 1 means the shot could go 90 degrees in any direction; .5 would be 45 degrees"), (float) this.target.aimSpread);
                this.target.aimSpreadRate = EditorGUILayout.FloatField(new GUIContent("  Aim Spread Rate: ", "The rate at which the weapon's spread increases while firing in aim mode. This value is added to the spread after every shot"), (float) this.target.aimSpreadRate);
                this.target.crouchSpreadModifier = EditorGUILayout.FloatField(new GUIContent("  Crouch Spread Modifier: ", "How the weapon's spread is modified while crouching. The spread while crouching is multiplied by this number"), (float) this.target.crouchSpreadModifier);
                this.target.proneSpreadModifier = EditorGUILayout.FloatField(new GUIContent("  Prone Spread Modifier: ", "How the weapon's spread is modified while prone. The spread while prone is multiplied by this number"), (float) this.target.proneSpreadModifier);
                this.target.moveSpreadModifier = EditorGUILayout.FloatField(new GUIContent("  Move Spread Modifier: ", "How the weapon's spread is modified while moving. The spread while moving is multiplied by this number"), (float) this.target.moveSpreadModifier);
                EditorGUILayout.Separator();
            }
            if (this.gunTipo != gunTypes.melee)
            {
                this.target.kickbackAngle = EditorGUILayout.FloatField(new GUIContent("  Vertical Recoil (Angle): ", "The maximum vertical angle per shot which the user's view will be incremented by"), (float) this.target.kickbackAngle);
                this.target.xKickbackFactor = EditorGUILayout.FloatField(new GUIContent("  Horizontal Recoil (Factor): ", "Factor relative to vertical recoil which horizontal recoil will use"), (float) this.target.xKickbackFactor);
                this.target.maxKickback = EditorGUILayout.FloatField(new GUIContent("  Maximum Recoil: ", "The maximum TOTAL angle on the x axis the weapon can recoil"), (float) this.target.maxKickback);
                this.target.recoilDelay = EditorGUILayout.FloatField(new GUIContent("  Recoil Delay: ", "The time before the weapon returns to its normal position from recoil"), (float) this.target.recoilDelay);
                this.target.kickbackAim = EditorGUILayout.FloatField(new GUIContent("  Aim Recoil (Angle): ", "Aim recoil in degrees"), (float) this.target.kickbackAim);
                this.target.crouchKickbackMod = EditorGUILayout.FloatField(new GUIContent("  Crouch Recoil (Multi): ", "Crouch recoil multiplier"), (float) this.target.crouchKickbackMod);
                this.target.proneKickbackMod = EditorGUILayout.FloatField(new GUIContent("  Prone Recoil (Multi): ", "Prone recoil multiplier"), (float) this.target.proneKickbackMod);
                this.target.moveKickbackMod = EditorGUILayout.FloatField(new GUIContent("  Move Recoil (Multi): ", "Move recoil multiplier"), (float) this.target.moveKickbackMod);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        //Alt-Fire
        EditorGUILayout.BeginVertical("toolbar");
        this.target.altFireFoldout = EditorGUILayout.Foldout(this.target.altFireFoldout, "Alt-Fire Properties:");
        EditorGUILayout.EndVertical();
        if (this.target.altFireFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            this.target.isPrimaryWeapon = EditorGUILayout.Toggle(new GUIContent("  Primary Weapon:  ", "Is this a primary weapon? Uncheck only if this gunscript is for an alt-fire"), this.target.isPrimaryWeapon);
            if (this.target.isPrimaryWeapon != null)
            {
                this.target.secondaryWeapon = EditorGUILayout.ObjectField(new GUIContent("  Secondary Weapon: ", "Optional alt-fire for weapon"), this.target.secondaryWeapon, GunScript, true);
                if (!(this.target.secondaryWeapon == null))
                {
                    this.target.secondaryInterrupt = EditorGUILayout.Toggle(new GUIContent("  Secondary Interrupt: ", "Can you interrupt the firing animation to switch to alt-fire mode?"), this.target.secondaryInterrupt);
                    this.target.secondaryFire = EditorGUILayout.Toggle(new GUIContent("  Instant Fire: ", "Does this alt-fire fire immediately? If unchecked, it will have to be switched to"), this.target.secondaryFire);
                    if (this.target.secondaryFire == false)
                    {
                        this.target.enterSecondaryTime = EditorGUILayout.FloatField(new GUIContent("  Enter Secondary Time: ", "The time in seconds to transition to alt-fire mode"), (float) this.target.enterSecondaryTime);
                        this.target.exitSecondaryTime = EditorGUILayout.FloatField(new GUIContent("  Exit Secondary Time: ", "The time in seconds to transition out of alt-fire mode"), (float) this.target.exitSecondaryTime);
                    }
                }
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        //Ammo + Reload
        if (this.gunTipo != gunTypes.melee)
        {
            EditorGUILayout.BeginVertical("toolbar");
            this.target.ammoReloadFoldout = EditorGUILayout.Foldout(this.target.ammoReloadFoldout, "Ammo + Reloading:");
            EditorGUILayout.EndVertical();
        }
        else
        {
            this.target.ammoReloadFoldout = false;
        }
        if (this.target.ammoReloadFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                if (this.target.chargeWeapon != null)
                {
                    this.target.additionalAmmoPerCharge = EditorGUILayout.FloatField(new GUIContent("  Ammo Cost Per Charge: ", "For each charge level, increase the ammo cost of firing the weapon by this amount. Works on integer intervals of charge level only."), (float) this.target.additionalAmmoPerCharge);
                }
            }
            else
            {
                if (this.gunTipo == gunTypes.melee)
                {
                    this.target.ammoPerClip = 1;
                    this.target.infiniteAmmo = true;
                    this.target.sharesAmmo = false;
                }
            }
            if (this.gunTipo != gunTypes.melee)
            {
                if (this.target.progressiveReload == null)
                {
                    this.target.ammoType = EditorGUILayout.EnumPopup(new GUIContent("  Ammo Type: ", "Does the ammo count refer to the number of clips remaining, or the number of individual shots remaining?"), this.target.ammoType);
                }
                this.target.ammoPerClip = EditorGUILayout.IntField(new GUIContent("  Ammo Per Clip: ", "The number of shots that can be fired before needing to reload"), (int) this.target.ammoPerClip);
                if (this.gunTipo != gunTypes.spray)
                {
                    this.target.ammoPerShot = EditorGUILayout.IntField(new GUIContent("  Ammo Used Per Shot: ", "The amout of ammo used every time the gun is fired"), (int) this.target.ammoPerShot);
                }
                else
                {
                    this.target.ammoPerShot = EditorGUILayout.IntField(new GUIContent("  Ammo Used Per Tick: ", "The amount of ammo drained every time ammo is drained. By default, ammo is drained once per second"), (int) this.target.ammoPerShot);
                    this.target.deltaTimeCoefficient = EditorGUILayout.FloatField(new GUIContent("  Drain Coefficient: ", "By default, ammo is drained every second. The rate at which ammo is drained is multiplied by this value"), (float) this.target.deltaTimeCoefficient);
                }
                this.target.sharesAmmo = EditorGUILayout.Toggle(new GUIContent("  Shares Ammo:  ", "If checked, this gun will be able to have a shared ammo reserve with one or more other weapons"), this.target.sharesAmmo);
                if (this.target.sharesAmmo == null)
                {
                    this.target.infiniteAmmo = EditorGUILayout.Toggle(new GUIContent("  Infinite Ammo: ", "If checked, this weapon will have infinite ammo"), this.target.infiniteAmmo);
                    this.target.clips = EditorGUILayout.IntField(new GUIContent("  Clips: ", "The amount of ammo for this weapon that the player has - either clips or bullets, depending on settings"), (int) this.target.clips);
                    this.target.maxClips = EditorGUILayout.IntField(new GUIContent("  Max Clips: ", "The maximum amount of ammo for this weapon that the player can carry"), (int) this.target.maxClips);
                }
                else
                {
                    this.target.managerObject = GameObject.FindWithTag("Manager");
                    error popupContent = this.target.managerObject.GetComponent(AmmoManager).namesArray;
                    int tempAmmoSet = (int) this.target.ammoSetUsed;
                    if (this.target.managerObject.GetComponent(AmmoManager).namesArray[0] == this.name)
                    {
                        this.target.managerObject.GetComponent(AmmoManager).namesArray = this.target.managerObject.GetComponent(AmmoManager).tempNamesArray.ToBuiltin(string);
                    }
                    this.target.ammoSetUsed = EditorGUILayout.Popup("  Ammo Set Used:  ", tempAmmoSet, popupContent);
                    this.target.managerObject.GetComponent(AmmoManager).namesArray[this.target.ammoSetUsed] = EditorGUILayout.TextField(new GUIContent("  Rename Ammo Set:", "Type a new name for the ammo set"), this.target.managerObject.GetComponent(AmmoManager).namesArray[this.target.ammoSetUsed]);
                    this.target.managerObject.GetComponent(AmmoManager).infiniteArray[this.target.ammoSetUsed] = EditorGUILayout.Toggle(new GUIContent("  Infinite Ammo: ", "If checked, this set will have infinite ammo"), this.target.managerObject.GetComponent(AmmoManager).infiniteArray[this.target.ammoSetUsed]);
                    this.target.managerObject.GetComponent(AmmoManager).clipsArray[this.target.ammoSetUsed] = EditorGUILayout.IntField(new GUIContent("  Clips: ", "The amount of ammo the player has in this set - either clips or bullets, depending on settings"), this.target.managerObject.GetComponent(AmmoManager).clipsArray[this.target.ammoSetUsed]);
                    this.target.managerObject.GetComponent(AmmoManager).maxClipsArray[this.target.ammoSetUsed] = EditorGUILayout.IntField(new GUIContent("  Max Clips: ", "The maximum amount of this type of ammo that the player can carry"), this.target.managerObject.GetComponent(AmmoManager).maxClipsArray[this.target.ammoSetUsed]);
                }
                EditorGUILayout.Separator();
                this.target.reloadTime = EditorGUILayout.FloatField(new GUIContent("  Reload Time: ", "The time it takes to load the weapon if the user presses the reload key"), (float) this.target.reloadTime);
                this.target.progressiveReset = EditorGUILayout.Toggle(new GUIContent("  Clear Reload: ", "If enabled, the gun will always start reloading at 0 rounds loaded, rather than the amount remaining in the clip"), this.target.progressiveReset);
                this.target.progressiveReload = EditorGUILayout.Toggle(new GUIContent("  Progressive Reloading: ", "Do you reload this weapon one bullet/shell/whatever at a time?"), this.target.progressiveReload);
                if (this.target.progressiveReload != null)
                {
                    this.target.reloadInTime = EditorGUILayout.FloatField(new GUIContent("  Enter Reload Time: ", "The time it takes to start the reload cycle"), (float) this.target.reloadInTime);
                    this.target.reloadOutTime = EditorGUILayout.FloatField(new GUIContent("  Exit Reload Time: ", "The time it takes to exit the reload cycle"), (float) this.target.reloadOutTime);
                    this.target.addOneBullet = false;
                }
                if (this.target.progressiveReload == null)
                {
                    this.target.addOneBullet = EditorGUILayout.Toggle(new GUIContent("  Partial Reload Bonus: ", "If enabled, the player will retain an additional round in the chamber when manually reloading"), this.target.addOneBullet);
                }
                //if(target.addOneBullet){
                this.target.emptyReloadTime = EditorGUILayout.FloatField(new GUIContent("  Empty Reload Time:  ", "The time it takes to reload the weapon if the user has completely emptied the weapon. This can be the same as the Reload Time"), (float) this.target.emptyReloadTime);
                //} else {
                //	target.emptyReloadTime = target.reloadTime;
                //}
                this.target.waitforReload = EditorGUILayout.FloatField(new GUIContent("  Wait For Reload: ", "The time between pressing the reload key, and actually starting to reload"), (float) this.target.waitforReload);
                EditorGUILayout.Separator();
            }
            EditorGUILayout.EndVertical();
        }
        //Audio/Visual
        EditorGUILayout.BeginVertical("toolbar");
        this.target.audioVisualFoldout = EditorGUILayout.Foldout(this.target.audioVisualFoldout, "Audio + Visual:");
        EditorGUILayout.EndVertical();
        if (this.target.audioVisualFoldout != null)
        {
            EditorGUILayout.BeginVertical("textField");
            EditorGUILayout.Separator();
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                this.target.delay = EditorGUILayout.FloatField(new GUIContent("  Delay: ", "The delay between when the fire animation starts and when the gun actually fires"), (float) this.target.delay);
                EditorGUILayout.Separator();
                if (this.gunTipo != gunTypes.launcher)
                {
                    this.target.tracer = EditorGUILayout.ObjectField(new GUIContent("  Tracer: ", "This optional field takes game object with a particle emitter to be used for tracer fire"), this.target.tracer, typeof(GameObject), true);
                    this.target.traceEvery = EditorGUILayout.IntField(new GUIContent("  Shots Per Tracer: ", "How many shots before displaying the tracer effect?"), (int) this.target.traceEvery);
                    this.target.simulateTime = EditorGUILayout.FloatField(new GUIContent("  Simulate Tracer for: ", "The amount of time to simulate your tracer particle system before rendering the particles. Useful if your tracers have to start being emit from behind the muzzle of the gun"), (float) this.target.simulateTime);
                    EditorGUILayout.Separator();
                }
                if (this.gunTipo != gunTypes.hitscan)
                {
                    this.target.shellEjection = false;
                }
                else
                {
                    this.target.shellEjection = EditorGUILayout.Toggle(new GUIContent("  Shell Ejection: ", "Does this weapon have shell ejection?"), this.target.shellEjection);
                }
                if (this.target.shellEjection != null)
                {
                    EditorGUILayout.BeginVertical("textfield");
                    this.target.shell = EditorGUILayout.ObjectField(new GUIContent("  Shell: ", "The GameObject to be instantiated when a shell is ejected"), this.target.shell, typeof(GameObject), false);
                    this.target.ejectorPosition = EditorGUILayout.ObjectField(new GUIContent("  Ejector Position: ", "Shells will be instantiated from the position of this GameObject"), this.target.ejectorPosition, typeof(GameObject), true);
                    this.target.ejectDelay = EditorGUILayout.FloatField("Delay", (float) this.target.ejectDelay);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                }
            }
            this.target.sway = EditorGUILayout.Toggle(new GUIContent("  Sway: ", "Does this weapon use coded weapon sway?"), this.target.sway);
            if (this.target.sway != null)
            {
                EditorGUILayout.BeginVertical("textField");
                this.target.overwriteSway = EditorGUILayout.Toggle(new GUIContent("  Override Rates: ", "Does this weapon override the global sway rates?"), this.target.overwriteSway);
                if (this.target.overwriteSway != null)
                {
                    this.target.moveSwayRate = EditorGUILayout.Vector2Field("  Move Sway Rate:  ", this.target.moveSwayRate);
                }
                this.target.moveSwayAmplitude = EditorGUILayout.Vector2Field("  Move Sway Amplitude:  ", this.target.moveSwayAmplitude);
                if (this.target.overwriteSway != null)
                {
                    this.target.runSwayRate = EditorGUILayout.Vector2Field("  Run Sway Rate:  ", this.target.runSwayRate);
                }
                this.target.runAmplitude = EditorGUILayout.Vector2Field("  Run Sway Amplitude:  ", this.target.runAmplitude);
                if (this.target.overwriteSway != null)
                {
                    this.target.idleSwayRate = EditorGUILayout.Vector2Field("  Idle Sway Rate:  ", this.target.idleSwayRate);
                }
                this.target.idleAmplitude = EditorGUILayout.Vector2Field("  Idle Sway Amplitude:  ", this.target.idleAmplitude);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }
            this.target.useZKickBack = EditorGUILayout.Toggle(new GUIContent("  Use Z KickBack: ", "Does the gun move along the z axis when firing"), this.target.useZKickBack);
            if (this.target.useZKickBack != null)
            {
                EditorGUILayout.BeginVertical("textfield");
                this.target.kickBackZ = EditorGUILayout.FloatField(new GUIContent("  Z Kickback: ", "The rate at which the gun moves backwards when firing"), (float) this.target.kickBackZ);
                this.target.zRetRate = EditorGUILayout.FloatField(new GUIContent("  Z Return: ", "The rate at which the gun returns to position when not"), (float) this.target.zRetRate);
                this.target.maxZ = EditorGUILayout.FloatField(new GUIContent("  Z Max: ", "The maximum amount the gun can kick back along the z axis"), (float) this.target.maxZ);
                EditorGUILayout.EndVertical();
            }
            this.target.timeToIdle = EditorGUILayout.FloatField(new GUIContent("  Idle Time: ", "The amount of time the player must be idle to start playing the idle animation"), (float) this.target.timeToIdle);
            this.target.overrideAvoidance = EditorGUILayout.Toggle(new GUIContent("  Override Avoidance: ", "Does this weapon override the global object avoidance settings"), this.target.overrideAvoidance);
            if (this.target.overrideAvoidance != null)
            {
                EditorGUILayout.BeginVertical("textField");
                this.target.avoids = EditorGUILayout.Toggle(new GUIContent("  Use Avoidance: ", "Does this weapon use object avoidance"), this.target.avoids);
                if (this.target.avoids != null)
                {
                    this.target.dist = EditorGUILayout.FloatField(new GUIContent("  Avoid Start Dist: ", "The distance from an object at which object avoidance will begin"), (float) this.target.dist);
                    this.target.minDist = EditorGUILayout.FloatField(new GUIContent("  Avoid Closest Dist: ", "The distance form an object at which avoidance will be maximized"), (float) this.target.minDist);
                    this.target.pos = EditorGUILayout.Vector3Field("  Avoid Position: ", this.target.pos);
                    this.target.rot = EditorGUILayout.Vector3Field("  Avoid Rotation: ", this.target.rot);
                }
                EditorGUILayout.EndVertical();
            }
            /*
				var overrideAvoidance : boolean = false; //Does this weapon override global object avoidance values
				var avoids : boolean = true;
				var rot : Vector3;
				var pos : Vector3;
				var dist : float = 2;
				var minDist : float = 1.5;
				*/
            this.target.takeOutTime = EditorGUILayout.FloatField(new GUIContent("  Take Out Time: ", "The time it takes to take out the weapon"), (float) this.target.takeOutTime);
            this.target.putAwayTime = EditorGUILayout.FloatField(new GUIContent("  Put Away Time: ", "The time it takes to put away the weapon"), (float) this.target.putAwayTime);
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                if ((this.target.autoFire == null) && (this.target.burstFire == null))
                {
                    this.target.fireAnim = EditorGUILayout.Toggle(new GUIContent("  Morph Fire Anim to Fit: ", "Maches the fire animation's speed to the time it takes to fire"), this.target.fireAnim);
                }
            }
            EditorGUILayout.Separator();
            this.target.fireSound = EditorGUILayout.ObjectField(new GUIContent("  Fire Sound: ", "The sound to play when each shot is fired"), this.target.fireSound, typeof(AudioClip), false);
            if (this.gunTipo == gunTypes.spray)
            {
                this.target.loopSound = EditorGUILayout.ObjectField(new GUIContent("  Looping Fire Sound: ", "The sound to loop while the weapon is firing"), this.target.loopSound, typeof(AudioClip), false);
                this.target.releaseSound = EditorGUILayout.ObjectField(new GUIContent("  Stop Firing Sound: ", "The sound to play when the weapon stops firing"), this.target.releaseSound, typeof(AudioClip), false);
            }
            if ((this.gunTipo == gunTypes.hitscan) || (this.gunTipo == gunTypes.launcher))
            {
                if (this.target.chargeWeapon != null)
                {
                    this.target.chargeLoop = EditorGUILayout.ObjectField(new GUIContent("  Charging Sound: ", "The sound to be looped while the weapon is charging"), this.target.chargeLoop, typeof(AudioClip), false);
                }
            }
            EditorGUILayout.LabelField(new GUIContent("  Sound Pitch: ", "The pitch to play the sound clip at. A value of 1 will play the sound at its natural pitch"));
            this.target.firePitch = EditorGUILayout.Slider(this.target.firePitch, -3, 3);
            EditorGUILayout.LabelField(new GUIContent("   Sound Volume", "Volume of Fire Sound"));
            this.target.fireVolume = EditorGUILayout.Slider(this.target.fireVolume, 0, 1);//EditorGUILayout.FloatField(GUIContent("  Sound Volume: ","The colume to play the sound clip at. A value of 1 will play the sound at max volume"), target.fireVolume);
            EditorGUILayout.Separator();
            if (this.gunTipo != gunTypes.melee)
            {
                this.target.emptySound = EditorGUILayout.ObjectField(new GUIContent("  Empty Sound: ", "The sound to play when sry firing"), this.target.emptySound, typeof(AudioClip), false);
                if (!(this.target.emptySound == null))
                {
                    EditorGUILayout.LabelField(new GUIContent("  Sound Pitch: ", "The pitch to play the sound clip at. A value of 1 will play the sound at its natural pitch"));
                    this.target.emptyPitch = EditorGUILayout.Slider(this.target.emptyPitch, -3, 3);
                    EditorGUILayout.LabelField(new GUIContent("   Sound Volume", "Volume of Empty Sound"));
                    this.target.emptyVolume = EditorGUILayout.Slider(this.target.emptyVolume, 0, 1);//EditorGUILayout.FloatField(GUIContent("  Sound Volume: ","The colume to play the sound clip at. A value of 1 will play the sound at max volume"), target.fireVolume);
                }
            }
            EditorGUILayout.Separator();
            this.target.crosshairObj = EditorGUILayout.ObjectField(new GUIContent("  Crosshair Plane: ", "Only use this if you are using a custom crosshair. Refer to documentation if needed"), this.target.crosshairObj, typeof(GameObject), true);
            if (!(this.target.crosshairObj == null))
            {
                this.target.crosshairSize = EditorGUILayout.FloatField(new GUIContent("  Crosshair Size: ", "The size of the default crosshair"), (float) this.target.crosshairSize);
                this.target.scale = EditorGUILayout.Toggle(new GUIContent("  Scale Crosshair: ", "Does the crosshair scale with accuracy? If disabled, the crosshair will always be a fixed size"), this.target.scale);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Separator();
        //		EditorGUILayout.Separator();
        string tempText = null;
        if (this.target.gunDisplayed != null)
        {
            tempText = "Deactivate Weapon";
        }
        else
        {
            tempText = "Activate Weapon";
        }
        if (GUILayout.Button(new GUIContent(tempText, "Toggle whether or not the gun is active"), "miniButton"))
        {
            if (this.target.gunDisplayed == null)
            {
                this.target.gunDisplayed = true;
                this.target.EditorSelect();
            }
            else
            {
                if (this.target.gunDisplayed != null)
                {
                    this.target.gunDisplayed = false;
                    this.target.EditorDeselect();
                }
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
        EditorGUILayout.Separator();
    }

    public GunScriptEditor()
    {
        this.gunTipo = gunTypes.hitscan;
    }

}