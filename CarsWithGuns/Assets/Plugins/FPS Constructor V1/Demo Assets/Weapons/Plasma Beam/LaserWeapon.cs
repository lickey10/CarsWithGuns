using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LaserWeapon : MonoBehaviour
{
    public GunScript gscript;
    public ParticleSystem emitters;
    public ParticleRenderer laser;
    public GameObject laserScript;
    private bool emitting;
    public float range;
    private GameObject targetObj;
    private Transform lastHit;
    private bool hasTarget;
    public float loseAngle;
    private bool hitEnemy;
    public float randomAngle;
    public float dps;
    public float overheatTime;
    public float force;
    [UnityEngine.HideInInspector]
    public float curHeat;
    private bool display;
    private float timeOnTarget;
    public float powerTime;
    public float damageMultiplier;
    public virtual void Update()
    {
        if (this.gscript.chargeLevel > 0.05f)
        {
            this.gscript.idleTime = 0;
            if (this.curHeat >= this.overheatTime)
            {
                this.gscript.chargeLevel = this.gscript.maxCharge;
                this.curHeat = 0;
                return;
            }
            this.gscript.chargeLevel = 1;
            this.FindTarget();
            this.curHeat = Mathf.Clamp(this.curHeat + Time.deltaTime, 0, this.overheatTime);
            if (!this.emitting)
            {
                this.EmitCharge(true);
            }
        }
        else
        {
            if (this.gscript.chargeLevel > 0)
            {
                this.GetComponent<AudioSource>().Play();
                this.FindTarget();
                this.EmitHit(false);
                this.EmitCharge(false);
            }
            else
            {
                this.timeOnTarget = 0;
                this.curHeat = Mathf.Clamp(this.curHeat - ((Time.deltaTime * ((this.overheatTime - this.curHeat) / this.overheatTime)) * 2), 0, this.overheatTime);
                this.EmitCharge(false);
                this.EmitHit(false);
                this.lastHit = null;
            }
        }
    }

    public virtual void EmitCharge(bool s)
    {
        this.laser.enabled = s;
        this.laserScript.SendMessage("EmitCharge", s);
        this.emitting = s;
    }

    public virtual void EmitHit(bool s)
    {
        //for(var i : int = 0; i < emitters.length; i++){
        //	emitters[i].emit = s;
        //}
        this.hasTarget = s;
    }

    public virtual void FindTarget()
    {
        RaycastHit hit = default(RaycastHit);
        if (this.targetObj == null)
        {
            this.targetObj = new GameObject();
            this.laserScript.SendMessage("Target", this.targetObj.transform);
        }
        int layer1 = 1 << PlayerWeapons.playerLayer;
        int layer2 = 1 << 2;
        int layerMask = layer1 | layer2;
        layerMask = ~layerMask;
        float tempAngle = 0;
        if ((this.lastHit != null) && this.hitEnemy)
        {
            Quaternion temp = Quaternion.LookRotation(this.targetObj.transform.position - this.transform.position);
            tempAngle = Quaternion.Angle(this.transform.rotation, temp);
        }
        else
        {
            tempAngle = this.loseAngle + 1;
        }
        if (this.lastHit == null)
        {
            this.lastHit = this.transform;
        }
        if (Physics.Raycast(PlayerWeapons.weaponCam.transform.position, this.SprayDirection(this.randomAngle), out hit, this.range, layerMask))
        {
            if ((tempAngle >= this.loseAngle) || (this.lastHit == hit.transform))
            {
                if (this.lastHit != hit.transform)
                {
                    this.timeOnTarget = 0;
                    if (((EnemyDamageReceiver) hit.transform.GetComponent(typeof(EnemyDamageReceiver))) != null)
                    {
                        this.hitEnemy = true;
                    }
                    else
                    {
                        this.hitEnemy = false;
                    }
                }
                else
                {
                    this.timeOnTarget = Mathf.Clamp(this.timeOnTarget + Time.deltaTime, 0, this.powerTime);
                }
                this.lastHit = hit.transform;
                this.targetObj.transform.position = hit.point;
                this.targetObj.transform.parent = hit.transform;
                this.SendDamage(hit);
            }
            else
            {
                this.timeOnTarget = Mathf.Clamp(this.timeOnTarget + Time.deltaTime, 0, this.powerTime);
                this.SendDamage(hit);
            }
            if (!this.hasTarget)
            {
                this.EmitHit(true);
            }
        }
        else
        {
            if (tempAngle < this.loseAngle)
            {
                this.timeOnTarget = Mathf.Clamp(this.timeOnTarget + Time.deltaTime, 0, this.powerTime);
                this.SendDamage(hit);
            }
            else
            {
                this.lastHit = null;
                if (this.hasTarget)
                {
                    this.EmitHit(false);
                }
                this.targetObj.transform.parent = null;
                this.targetObj.transform.position = this.transform.position + (this.SprayDirection(this.randomAngle) * this.range);
            }
        }
    }

    public virtual void SendDamage(RaycastHit hit)
    {
        object[] sendArray = new object[2];
        sendArray[0] = (this.dps + ((this.timeOnTarget / this.powerTime) * this.damageMultiplier)) * Time.deltaTime;
        sendArray[1] = true;
        if (hit.collider == null)
        {
            return;
        }
        hit.collider.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        if (hit.rigidbody && !(hit.transform.gameObject.layer == "Player"))
        {
            hit.rigidbody.AddForceAtPosition(this.force * this.SprayDirection(this.randomAngle), hit.point);
        }
    }

    public virtual Vector3 SprayDirection(float c)
    {
        /*
	var vx = (1 - 2 * Random.value) * c;
	var vy = (1 - 2 * Random.value) * c;
	var vz = 1.0;
	return PlayerWeapons.weaponCam.transform.TransformDirection(Vector3(vx,vy,vz));
	*/
        return PlayerWeapons.weaponCam.transform.TransformDirection(Vector3.forward);
    }

    public virtual void OnGUI()
    {
        if (this.display)
        {
            GUI.Box(new Rect(Screen.width - 130, Screen.height - 50, 120, 40), ((((("Heat: " + Mathf.Round((this.curHeat / this.overheatTime) * 100)) + "%") + "\n") + "Power: ") + Mathf.Round(((this.dps + ((this.timeOnTarget / this.powerTime) * this.damageMultiplier)) / (this.dps + this.damageMultiplier)) * 100)) + "%");
        }
    }

    public virtual void SelectWeapon()
    {
        this.display = true;
    }

    public virtual void DeselectWeapon()
    {
        this.display = false;
    }

    public LaserWeapon()
    {
        this.range = 50;
        this.hasTarget = true;
        this.loseAngle = 7;
        this.randomAngle = 0.01f;
    }

}