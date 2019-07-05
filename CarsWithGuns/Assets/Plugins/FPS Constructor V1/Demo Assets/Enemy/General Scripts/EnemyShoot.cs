using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyShoot : MonoBehaviour
{
    private Transform target;
    private float nextAttackTime;
    public float damage;
    public float force;
    public float fireRate;
    public Fire fire;
    public GameObject tracer;
    public Transform shootPos;
    public float actualSpread;
    public virtual void Start()
    {
        this.target = PlayerWeapons.weaponCam.transform;
    }

    public virtual void Attack()
    {
        if (Time.time < this.nextAttackTime)
        {
            return;
        }
        this.nextAttackTime = Time.time + this.fireRate;
        //function Fire (penetration : int, damage : float, force : float, tracer : GameObject, direction : Vector3, firePosition : Vector3) {
        this.fire.Fire(0, this.damage, this.force, this.tracer, this.SprayDirection(), this.shootPos.position);
    }

    public virtual Vector3 SprayDirection()
    {
        float vx = (1 - (2 * Random.value)) * this.actualSpread;
        float vy = (1 - (2 * Random.value)) * this.actualSpread;
        float vz = 1f;
        return this.transform.TransformDirection(new Vector3(vx, vy, vz));
    }

}