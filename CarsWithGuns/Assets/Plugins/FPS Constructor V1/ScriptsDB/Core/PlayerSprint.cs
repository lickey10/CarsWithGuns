using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerSprint : MonoBehaviour
{
    public static bool sprinting;
    private bool exhausted;
    private float sprintEndTime;
    private CharacterMotorDB CM;
    [UnityEngine.HideInInspector]
    public bool weaponsInactive;
    [UnityEngine.HideInInspector]
    public MovementValues values;
    public virtual void Start()
    {
        this.CM = PlayerWeapons.CM;
        this.values = MovementValues.singleton;
    }

    public virtual void Update()
    {
        float tempSprintTime = 0.0f;
        this.weaponsInactive = PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon] == null;
        if (!this.weaponsInactive)
        {
            this.weaponsInactive = ((GunScript) PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].GetComponent(typeof(GunScript))).gunActive == false;
        }
        if (!this.weaponsInactive)
        {
            return;
        }
        //Replenish Sprint time
        if (PlayerWeapons.controller.velocity.magnitude == 0)
        {
            tempSprintTime = this.sprintEndTime;
        }
        if (((AimMode.sprintNum < this.values.sprintDuration) && !AimMode.sprintingPublic) && (Time.time > tempSprintTime))
        {
            if (PlayerWeapons.controller.velocity.magnitude == 0)
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (this.values.sprintAddStand * Time.deltaTime), 0, this.values.sprintDuration);
            }
            else
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (this.values.sprintAddWalk * Time.deltaTime), 0, this.values.sprintDuration);
            }
        }
        if (AimMode.sprintNum > this.values.sprintMin)
        {
            this.exhausted = false;
        }
        //Handle sprint
        if (((((InputDB.GetButton("Sprint") && !InputDB.GetButton("Aim")) && PlayerWeapons.canSprint) && this.CM.grounded) && !this.exhausted) && ((PlayerWeapons.controller.velocity.magnitude/*CM.prone || */ > this.CM.movement.minSprintSpeed) || CharacterMotorDB.crouching))
        {
            AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum - Time.deltaTime, 0, this.values.sprintDuration);
            if (!AimMode.sprintingPublic)
            {
                AimMode.sprintingPublic = true;
                this.BroadcastMessage("Sprinting", SendMessageOptions.DontRequireReceiver);
                AimMode.canSwitchWeaponAim = false;
            }
            //Check if we're out of sprint
            if (AimMode.sprintNum <= 0)
            {
                this.exhausted = true;
                this.sprintEndTime = Time.time + this.values.recoverDelay;
            }
        }
        else
        {
            if (AimMode.sprintingPublic)
            {
                AimMode.sprintingPublic = false;
                this.BroadcastMessage("StopSprinting", SendMessageOptions.DontRequireReceiver);
                this.BroadcastMessage("NormalSpeed");
                AimMode.canSwitchWeaponAim = true;
            }
        }
    }

}