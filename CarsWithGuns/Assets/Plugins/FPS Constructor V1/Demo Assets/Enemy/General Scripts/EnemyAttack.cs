using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyAttack : MonoBehaviour
{
    private Transform target;
    private float nextAttackTime;
    public float damage;
    public float attackTime;
    public virtual void Start()//    target = PlayerWeapons.weaponCam.transform;
    {
    }

    public virtual void Attack()
    {
        if (Time.time < this.nextAttackTime)
        {
            return;
        }
        object[] sendArray = new object[2];
        sendArray[0] = this.damage;
        sendArray[1] = false;
        this.target.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        this.target.SendMessageUpwards("Direction", this.transform, SendMessageOptions.DontRequireReceiver);
        this.nextAttackTime = Time.time + this.attackTime;
        this.GetComponent<Animation>().Play();
    }

}