using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HitBox : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public GunScript GunScript;
    public float damage;
    public float force;
    private bool isActive;
    private EffectsManager effectsManager;
    public virtual void Start()
    {
        this.effectsManager = (EffectsManager) GameObject.FindWithTag("Manager").GetComponent(typeof(EffectsManager));
    }

    public virtual void Update()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        if (this.GunScript.hitBox)
        {
            this.isActive = true;
            ((BoxCollider) this.GetComponent(typeof(BoxCollider))).isTrigger = false;
        }
        else
        {
            this.isActive = false;
            ((BoxCollider) this.GetComponent(typeof(BoxCollider))).isTrigger = true;
        }
    }

    public virtual void OnCollisionEnter(Collision c)
    {
        RaycastHit hit = default(RaycastHit);
        if (this.isActive)
        {
            object[] sendArray = new object[2];
            sendArray[0] = this.damage;
            sendArray[1] = true;
            c.collider.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
            if ((UseEffects) c.gameObject.GetComponent(typeof(UseEffects)))
            {
                int layer1 = 1 << PlayerWeapons.playerLayer;
                int layer2 = 1 << 2;
                int layerMask = layer1 | layer2;
                layerMask = ~layerMask;
                if (Physics.Raycast(this.GunScript.gameObject.transform.position, this.GunScript.gameObject.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    //The effectsManager needs five bits of information
                    Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    int hitSet = ((UseEffects) c.gameObject.GetComponent(typeof(UseEffects))).setIndex;
                    object[] hitInfo = hit.point;
                    this.effectsManager.SendMessage("ApplyDent", hitInfo, SendMessageOptions.DontRequireReceiver);
                }
            }
            if (((Rigidbody) c.collider.GetComponent(typeof(Rigidbody))) != null)
            {
                ((Rigidbody) c.collider.GetComponent(typeof(Rigidbody))).AddForce(c.relativeVelocity * this.force);
            }
            this.GunScript.hitBox = false;
            this.isActive = false;
            this.GetComponent<AudioSource>().loop = false;
        }
    }

}