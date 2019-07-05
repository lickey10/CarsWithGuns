using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyShootProjectile : MonoBehaviour
{
    private float nextAttackTime;
    public Transform pos;
    public Rigidbody projectile;
    public float initialSpeed;
    public float fireRate;
    public float actualSpread;
    public ParticleSystem emitter;
    public float backForce;
    public virtual void Attack()
    {
        if (Time.time < this.nextAttackTime)
        {
            return;
        }
        this.nextAttackTime = Time.time + this.fireRate;
        //function Fire (penetration : int, damage : float, force : float, tracer : GameObject, direction : Vector3, firePosition : Vector3) {
        this.FireProjectile();
    }

    public virtual void FireProjectile()
    {
        Vector3 direction = this.SprayDirection();
        Quaternion convert = Quaternion.LookRotation(direction + new Vector3(0, 0.04f, 0));
        Rigidbody instantiatedProjectile = null;
        instantiatedProjectile = UnityEngine.Object.Instantiate(this.projectile, this.pos.position, convert);
        instantiatedProjectile.velocity = instantiatedProjectile.transform.TransformDirection(new Vector3(0, 0, this.initialSpeed));
        Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), this.transform.root.GetComponent<Collider>());
        this.emitter.Play();//.Emit();
        this.transform.root.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, -this.backForce), ForceMode.Impulse);
    }

    public virtual Vector3 SprayDirection()
    {
        float vx = (1 - (2 * Random.value)) * this.actualSpread;
        float vy = (1 - (2 * Random.value)) * this.actualSpread;
        float vz = 1f;
        return this.transform.TransformDirection(new Vector3(vx, vy, vz));
    }

    public EnemyShootProjectile()
    {
        this.initialSpeed = 50;
        this.fireRate = 1;
        this.actualSpread = 0.2f;
        this.backForce = 10;
    }

}