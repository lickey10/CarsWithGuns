using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Fire : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    private EffectsManager effectsManager;
    private int shotCountTracer;
    public int traceEvery;
    public virtual void Start()
    {
        this.effectsManager = (EffectsManager) GameObject.FindWithTag("Manager").GetComponent(typeof(EffectsManager));
    }

    public virtual void Fire(int penetration, float damage, float force, GameObject tracer, Vector3 direction, Vector3 firePosition)
    {
        //must pass in penetation level, damage, force, tracer object (optional), direction to fire in, and position top fire from.
        bool penetrate = true;
        int pVal = penetration;
        int layer2 = 1 << 2;
        int layerMask = layer2;
        layerMask = ~layerMask;
        RaycastHit[] hits = null;
        hits = Physics.RaycastAll(firePosition, direction, 100, layerMask);
        this.shotCountTracer = this.shotCountTracer + 1;
        if ((tracer != null) && (this.traceEvery <= this.shotCountTracer))
        {
            this.shotCountTracer = 0;
            if (hits.Length > 0)
            {
                tracer.transform.LookAt(hits[0].point);
            }
            else
            {
                tracer.transform.LookAt(this.transform.position + (90 * direction));
            }
            tracer.GetComponent(ParticleEmitter).Emit();
            tracer.GetComponent(ParticleEmitter).Simulate(0.02f);
        }
        System.Array.Sort(hits, this.Comparison);
        //	 Did we hit anything?
        int i = 0;
        while (i < hits.Length)
        {
            RaycastHit hit = hits[i];
            BulletPenetration BP = (BulletPenetration) hit.transform.GetComponent(typeof(BulletPenetration));
            if (penetrate)
            {
                if (BP == null)
                {
                    penetrate = false;
                }
                else
                {
                    if (pVal < BP.penetrateValue)
                    {
                        penetrate = false;
                    }
                    else
                    {
                        pVal = pVal - BP.penetrateValue;
                    }
                }
                //DAmage Array
                object[] sendArray = new object[2];
                sendArray[0] = damage;
                sendArray[1] = false;
                // Send a damage message to the hit object			
                hit.collider.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
                hit.collider.SendMessageUpwards("Direction", this.transform, SendMessageOptions.DontRequireReceiver);
                //And send a message to the decal manager, if the target uses decals
                if ((UseEffects) hit.transform.gameObject.GetComponent(typeof(UseEffects)))
                {
                    //The effectsManager needs five bits of information
                    Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    int hitSet = ((UseEffects) hit.transform.gameObject.GetComponent(typeof(UseEffects))).setIndex;
                    object[] hitInfo = hit.point;
                    this.effectsManager.SendMessage("ApplyDecal", hitInfo, SendMessageOptions.DontRequireReceiver);
                }
                // Apply a force to the rigidbody we hit
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
                }
            }
            i++;
        }
        this.BroadcastMessage("MuzzleFlash", true, SendMessageOptions.DontRequireReceiver);
        GameObject audioObj = new GameObject("GunShot");
        audioObj.transform.position = this.transform.position;
        audioObj.transform.parent = this.transform;
        ((TimedObjectDestructorDB) audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = this.GetComponent<AudioSource>().clip.length + 0.1f;
        AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource));
        aO.clip = this.GetComponent<AudioSource>().clip;
        aO.volume = this.GetComponent<AudioSource>().volume;
        aO.pitch = this.GetComponent<AudioSource>().pitch;
        aO.Play();
        aO.loop = false;
        aO.rolloffMode = AudioRolloffMode.Linear;
    }

    public virtual int Comparison(RaycastHit x, RaycastHit y)
    {
        float xDistance = x.distance;
        float yDistance = y.distance;
        return xDistance - yDistance;
    }

    public Fire()
    {
        this.traceEvery = 1;
    }

}