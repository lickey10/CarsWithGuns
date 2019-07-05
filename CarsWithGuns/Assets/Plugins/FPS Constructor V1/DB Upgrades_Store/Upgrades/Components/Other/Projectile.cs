using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Projectile : MonoBehaviour
{
    public Rigidbody val;
    private Rigidbody cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.projectile;
        gScript.projectile = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.projectile = this.cache;
    }

}