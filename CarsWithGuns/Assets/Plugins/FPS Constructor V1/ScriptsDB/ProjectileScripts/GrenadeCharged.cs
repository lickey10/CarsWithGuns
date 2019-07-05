using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GrenadeCharged : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float timeOut;
    public bool detachChildren;
    public Transform explosion;
    public Transform explosionCharged;
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    public GameObject[] playerThings;
    public float chargeVal;
    public virtual void Start()
    {
        this.explodeTime = Time.time + this.timeOut;
        this.playerThings = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        while (i < this.playerThings.Length)
        {
            if (this.playerThings[i].GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), this.playerThings[i].GetComponent<Collider>());
            }
            i++;
        }
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

    public virtual void ChargeLevel(float charge)
    {
        if (charge >= this.chargeVal)
        {
            ((Rigidbody) this.GetComponent(typeof(Rigidbody))).useGravity = false;
            this.explosion = this.explosionCharged;
        }
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

    public GrenadeCharged()
    {
        this.delay = 1f;
        this.timeOut = 1f;
    }

}