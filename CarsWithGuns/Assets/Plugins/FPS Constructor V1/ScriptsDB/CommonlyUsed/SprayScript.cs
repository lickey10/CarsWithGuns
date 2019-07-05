using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SprayScript : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public GunScript gunScript;
    private float trueDamage;
    private float trueForce;
    [UnityEngine.HideInInspector]
    public bool isActive;
    private ParticleEmitter[] emitters;
    private int i;
    public virtual void Awake()
    {
        this.gunScript = (GunScript) this.transform.parent.GetComponent(typeof(GunScript));
        this.emitters = this.gameObject.GetComponentsInChildren<ParticleEmitter>();
        this.isActive = false;
    }

    public virtual void OnParticleCollision(GameObject hitObj)
    {
        float dist = Vector3.Distance(hitObj.transform.position, this.transform.position);
        this.trueDamage = this.gunScript.damage;
        if (dist > this.gunScript.maxFalloffDist)
        {
            this.trueDamage = (this.gunScript.damage * Mathf.Pow(this.gunScript.falloffCoefficient, (this.gunScript.maxFalloffDist - this.gunScript.minFalloffDist) / this.gunScript.falloffDistanceScale)) * Time.deltaTime;
        }
        else
        {
            if ((dist < this.gunScript.maxFalloffDist) && (dist > this.gunScript.minFalloffDist))
            {
                this.trueDamage = (this.gunScript.damage * Mathf.Pow(this.gunScript.falloffCoefficient, (dist - this.gunScript.minFalloffDist) / this.gunScript.falloffDistanceScale)) * Time.deltaTime;
            }
        }
        object[] sendArray = new object[2];
        sendArray[0] = this.trueDamage;
        sendArray[1] = true;
        hitObj.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        this.trueForce = this.gunScript.force * Mathf.Pow(this.gunScript.forceFalloffCoefficient, dist);
        if ((Rigidbody) hitObj.GetComponent(typeof(Rigidbody)))
        {
            Rigidbody rigid = (Rigidbody) hitObj.GetComponent(typeof(Rigidbody));
            Vector3 vectorForce = -Vector3.Normalize(this.transform.position - hitObj.transform.position) * this.trueForce;
            rigid.AddForce(vectorForce);
        }
    }

    public virtual void ToggleActive(bool activate)
    {
        if (activate == false)
        {
            this.isActive = false;
            foreach (ParticleEmitter emitter in this.emitters)
            {
                emitter.emit = false;
            }
        }
        else
        {
            this.isActive = true;
            foreach (ParticleEmitter emitter in this.emitters)
            {
                emitter.emit = true;
            }
        }
    }

}