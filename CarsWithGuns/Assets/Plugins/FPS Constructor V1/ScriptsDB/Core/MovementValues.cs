using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MovementValues : MonoBehaviour
{
    //This script stores values to be taken by CharacterMotorDB, to allow a simpler customEditor
    public float defaultForwardSpeed;
    public float maxCrouchSpeed;
    public float maxSprintSpeed;
    public float minSprintSpeed;
    public float maxAimSpeed;
    public float maxProneSpeed;
    public float defaultSidewaysSpeed;
    public float sprintSidewaysSpeed;
    public float crouchSidewaysSpeed;
    public float aimSidewaysSpeed;
    public float proneSidewaysSpeed;
    public float defaultBackwardsSpeed;
    public float crouchBackwardsSpeed;
    public float aimBackwardsSpeed;
    public float proneBackwardsSpeed;
    public bool sprintFoldout;
    public bool crouchFoldout;
    public bool defaultFoldout;
    public bool proneFoldout;
    public bool aimFoldout;
    public bool jumpFoldout;
    public CharacterMotorDB CM;
    public int sprintDuration; //how long can we sprint for?
    public float sprintAddStand; //how quickly does sprint replenish when idle?
    public float sprintAddWalk; //how quickly does sprint replenish when moving?
    public float sprintMin; //What is the minimum value ofsprint at which we can begin sprinting?
    public float recoverDelay; //how much time after sprinting does it take to start recovering sprint?
    public float exhaustedDelay; //how much time after sprinting to exhaustion does it take to start recovering sprint?
    public static MovementValues singleton;
    public virtual void Awake()
    {
        MovementValues.singleton = this;
    }

    public MovementValues()
    {
        this.defaultForwardSpeed = 10;
        this.maxCrouchSpeed = 6;
        this.maxSprintSpeed = 13;
        this.minSprintSpeed = 10;
        this.maxAimSpeed = 4;
        this.maxProneSpeed = 4;
        this.defaultSidewaysSpeed = 10;
        this.sprintSidewaysSpeed = 15;
        this.crouchSidewaysSpeed = 6;
        this.aimSidewaysSpeed = 4;
        this.proneSidewaysSpeed = 2;
        this.defaultBackwardsSpeed = 10;
        this.crouchBackwardsSpeed = 6;
        this.aimBackwardsSpeed = 4;
        this.proneBackwardsSpeed = 2;
        this.sprintDuration = 5;
        this.sprintAddStand = 1;
        this.sprintAddWalk = 0.3f;
        this.sprintMin = 1;
        this.recoverDelay = 0.7f;
        this.exhaustedDelay = 1;
    }

}