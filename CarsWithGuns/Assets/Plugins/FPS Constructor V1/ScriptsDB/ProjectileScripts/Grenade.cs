using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Grenade : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float timeOut;
    public bool detachChildren;
    public Transform explosion;
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    public GameObject[] playerThings;
    public virtual void Start()
    {
        this.explodeTime = Time.time + this.timeOut;
    }

    public virtual IEnumerator OnCollisionEnter(Collision collision)
    {
        if (this.hasCollided || !this.explodeAfterBounce)
        {
            this.DestroyNow();
        }
        yield return new WaitForSeconds(this.delay);
        this.hasCollided = true;
    }

    public virtual void DestroyNow()
    {
        if (this.detachChildren)
        {
            this.transform.DetachChildren();
        }
        UnityEngine.Object.DestroyObject(this.gameObject);
        if (this.explosion)
        {
            UnityEngine.Object.Instantiate(this.explosion, this.transform.position, this.transform.rotation);
        }
    }

    public virtual void Update()
    {
        RaycastHit hit = default(RaycastHit);
        Vector3 direction = this.transform.TransformDirection(Vector3.forward);
        if (Time.time > this.explodeTime)
        {
            this.DestroyNow();
        }
    }

    public Grenade()
    {
        this.delay = 1f;
        this.timeOut = 1f;
    }

}