using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Missile : MonoBehaviour
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
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    private float initiateTime;
    public GameObject[] playerThings;
    public Transform t;
    public float turnSpeed;
    public float flySpeed;
    public float initiatedSpeed;
    public ParticleEmitter em;
    public bool soundPlaying;
    public GameObject lockObj;
    private Camera cam;
    //private var hasExploded : boolean = false;
    public virtual void Start()
    {
        this.explodeTime = Time.time + this.timeOut;
        this.initiateTime = Time.time + this.delay;
        this.cam = GameObject.FindWithTag("WeaponCamera").GetComponent<Camera>();
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
        LockOnMissile temp = null;
        temp = (LockOnMissile) GameObject.FindWithTag("Missile").GetComponent(typeof(LockOnMissile));
        this.t = temp.Target();
        if (this.t != null)
        {
            this.lockObj.transform.position = this.t.position;
            this.lockObj.transform.parent = null;
        }
    }

    public virtual void DestroyNow()
    {
        if (this.detachChildren)
        {
            this.transform.DetachChildren();
        }
        if (this.lockObj != null)
        {
            UnityEngine.Object.Destroy(this.lockObj);
        }
        UnityEngine.Object.DestroyObject(this.gameObject);
        if (this.explosion)
        {
            UnityEngine.Object.Instantiate(this.explosion, this.transform.position, new Quaternion(0, 0, 0, 0));
        }
    }

    public virtual void LateUpdate()
    {
        Quaternion temp = default(Quaternion);
        if (this.lockObj != null)
        {
            if (this.t != null)
            {
                ((Renderer) this.lockObj.GetComponentInChildren(typeof(Renderer))).enabled = true;
                this.lockObj.transform.position = this.t.position;
            }
            else
            {
                ((Renderer) this.lockObj.GetComponentInChildren(typeof(Renderer))).enabled = false;
            }
            this.lockObj.transform.LookAt(this.cam.transform);
        }
        if (Time.time > this.initiateTime)
        {
            if (!this.soundPlaying)
            {
                this.GetComponent<AudioSource>().Play();
                this.soundPlaying = true;
            }
            if (this.t != null)
            {
                temp = Quaternion.LookRotation(this.t.position - this.transform.position, Vector3.up);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, temp, Time.deltaTime * this.turnSpeed);
            }
            else
            {
                UnityEngine.Object.Destroy(this.lockObj);
            }
            this.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(Vector3.forward) * this.initiatedSpeed;
            this.em.emit = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(Vector3.forward) * this.flySpeed;
            this.em.emit = false;
        }
        if (Time.time > this.explodeTime)
        {
            this.DestroyNow();
        }
    }

    public Missile()
    {
        this.delay = 1f;
        this.timeOut = 1f;
    }

}