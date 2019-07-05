using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyDamageReceiver : MonoBehaviour
{
    public float hitPoints;
    public Transform deathEffect;
    public float effectDelay;
    private GameObject[] gos;
    public float multiplier;
    public Rigidbody deadReplacement;
    [UnityEngine.HideInInspector]
    public GameObject playerObject;
    public bool useHitEffect;
    [UnityEngine.HideInInspector]
    public bool isEnemy;
    public virtual void ApplyDamage(object[] Arr)
    {
        float tempFloat = 0.0f;
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        if (Arr[1] == true)
        {
            if (!this.playerObject)
            {
                this.playerObject = GameObject.FindWithTag("Player");
            }
            if (this.useHitEffect)
            {
                this.playerObject.BroadcastMessage("HitEffect", SendMessageOptions.DontRequireReceiver);
            }
        }
        // We already have less than 0 hitpoints, maybe we got killed already?
        if (this.hitPoints <= 0f)
        {
            return;
        }
        tempFloat = Arr[0];
        //float.TryParse(Arr[0], tempFloat);
        this.hitPoints = this.hitPoints - (tempFloat * this.multiplier);
        if (this.hitPoints <= 0f)
        {
            // Start emitting particles
            ParticleSystem emitter = (ParticleSystem) this.GetComponentInChildren(typeof(ParticleSystem));
            if (emitter)
            {
                emitter.Play();//.emit = true;
            }
            this.Invoke("DelayedDetonate", this.effectDelay);
        }
    }

    public virtual void ApplyDamagePlayer(float damage)
    {
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        if (!this.playerObject)
        {
            this.playerObject = GameObject.FindWithTag("Player");
        }
        if (this.useHitEffect)
        {
            this.playerObject.BroadcastMessage("HitEffect", SendMessageOptions.DontRequireReceiver);
        }
        // We already have less than 0 hitpoints, maybe we got killed already?
        if (this.hitPoints <= 0f)
        {
            return;
        }
        //float.TryParse(Arr[0], tempFloat);
        this.hitPoints = this.hitPoints - (damage * this.multiplier);
        if (this.hitPoints <= 0f)
        {
            // Start emitting particles
            ParticleSystem emitter = (ParticleSystem) this.GetComponentInChildren(typeof(ParticleSystem));
            if (emitter)
            {
                emitter.Play();//.emit = true;
            }
            if (this.gameObject.tag == "Enemy")
            {
                error PointScript = this.gameObject.AddComponent<Point>();
                PointScript.Point = 50;
            }
            this.Invoke("DelayedDetonate", this.effectDelay);
        }
    }

    public virtual void ApplyDamage(float damage)
    {
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        // We already have less than 0 hitpoints, maybe we got killed already?
        if (this.hitPoints <= 0f)
        {
            return;
        }
        //float.TryParse(Arr[0], tempFloat);
        this.hitPoints = this.hitPoints - (damage * this.multiplier);
        if (this.hitPoints <= 0f)
        {
            // Start emitting particles
            ParticleSystem emitter = (ParticleSystem) this.GetComponentInChildren(typeof(ParticleSystem));
            if (emitter)
            {
                emitter.Play();//.emit = true;
            }
            this.Invoke("DelayedDetonate", this.effectDelay);
        }
    }

    public virtual void DelayedDetonate()
    {
        this.BroadcastMessage("Detonate");
    }

    public virtual void Detonate()
    {
        if (this.isEnemy)
        {
            EnemyMovement.enemies--;
        }
        // Create the deathEffect
        if (this.deathEffect)
        {
            UnityEngine.Object.Instantiate(this.deathEffect, this.transform.position, this.transform.rotation);
        }
        // If we have a dead replacement then replace ourselves with it!
        if (this.deadReplacement)
        {
            Rigidbody dead = UnityEngine.Object.Instantiate(this.deadReplacement, this.transform.position, this.transform.rotation);
            // For better effect we assign the same velocity to the exploded gameObject
            dead.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
            dead.angularVelocity = this.GetComponent<Rigidbody>().angularVelocity;
        }
        // If there is a particle emitter stop emitting and detach so it doesnt get destroyed right away
        ParticleSystem emitter = (ParticleSystem) this.GetComponentInChildren(typeof(ParticleSystem));
        if (emitter)
        {
            emitter.Stop();//.emit = false;
            emitter.transform.parent = null;
        }
        this.BroadcastMessage("Die", SendMessageOptions.DontRequireReceiver);
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public EnemyDamageReceiver()
    {
        this.hitPoints = 100f;
        this.multiplier = 1;
        this.useHitEffect = true;
    }

}