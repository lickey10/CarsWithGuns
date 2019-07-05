using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ChargeTest : MonoBehaviour
{
    public GunScript gscript;
    public ParticleEmitter[] emitters;
    public bool emitting;
    public ParticleSystem specialEmitter;
    public float minSpecial;
    public virtual void LateUpdate()
    {
        ((Light) this.GetComponentInChildren(typeof(Light))).range = this.gscript.chargeLevel * 10;
        if ((this.gscript.chargeLevel > 0) || this.emitting)
        {
            ((AudioSource) this.gscript.gameObject.GetComponent(typeof(AudioSource))).pitch = this.gscript.chargeLevel;
            if (!this.emitting)
            {
                this.emitCharge(true);
            }
            else
            {
                if (this.emitting)
                {
                    this.emitCharge(false);
                }
            }
        }
        else
        {
            ((AudioSource) this.gscript.gameObject.GetComponent(typeof(AudioSource))).pitch = this.gscript.firePitch;
            this.specialEmitter.Stop();
        }
    }

    public virtual void emitCharge(bool s)
    {
        int i = 0;
        while (i < this.emitters.length)
        {
            if (s)
            {
                this.emitters[i].Play();
            }
            else
            {
                this.emitters[i].Stop();
            }
            i++;
        }
        if (this.gscript.chargeLevel > this.minSpecial)
        {
            this.specialEmitter.Play();
        }
        else
        {
            this.specialEmitter.Stop();
        }
        this.emitting = s;
    }

}