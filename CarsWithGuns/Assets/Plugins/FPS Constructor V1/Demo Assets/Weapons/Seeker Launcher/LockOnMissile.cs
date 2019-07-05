using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LockOnMissile : MonoBehaviour
{
    public GunScript gscript;
    [UnityEngine.HideInInspector]
    public Transform targetT;
    [UnityEngine.HideInInspector]
    public Transform targetTwo;
    [UnityEngine.HideInInspector]
    public Transform targetThree;
    [UnityEngine.HideInInspector]
    public int targets;
    public float lockRange;
    [UnityEngine.HideInInspector]
    public bool lockedOn;
    public float lockTime;
    public float lockThreshold;
    [UnityEngine.HideInInspector]
    public LineRenderer line;
    public GameObject[] lockObjs;
    public float delayTime;
    public float firedDelayTime;
    private float nextLockTime;
    private Camera cam;
    /*for(var i : int = 0; i < lockObjs.length; i ++){
		lockObjs[i].transform.parent = null;
	}*/    public virtual void Start()
    {
        this.line = (LineRenderer) this.GetComponent(typeof(LineRenderer));
        this.cam = GameObject.FindWithTag("WeaponCamera").GetComponent<Camera>();
    }

    public virtual Transform Target()
    {
        this.nextLockTime = Time.time + (this.firedDelayTime / this.gscript.burstCount);
        if (this.targets == 1)
        {
            this.targets--;
            this.lockedOn = false;
            return this.targetT;
        }
        else
        {
            if (this.targets == 2)
            {
                this.targets--;
                return this.targetTwo;
            }
            else
            {
                if (this.targets == 3)
                {
                    this.targets--;
                    return this.targetThree;
                }
            }
        }
        return null;
    }

    public virtual void Update()
    {
        if (!this.lockedOn)
        {
            this.targets = 0;
        }
        if (this.lockedOn || (this.targetT != null))
        {
            if (((this.targets == 0) && (Random.value < this.lockTime)) || (((this.targets > 0) && (this.targetT != this.targetThree)) && (this.targetTwo != this.targetT)))
            {
                this.lockObjs[0].transform.position = this.targetT.position;
                this.lockObjs[0].SetActive(true);
            }
            else
            {
                this.lockObjs[0].SetActive(false);
            }
            if (((this.targets == 1) && (Random.value < this.lockTime)) || ((this.targets > 1) && (this.targetTwo != this.targetThree)))
            {
                this.lockObjs[1].transform.position = this.targetTwo.position;
                this.lockObjs[1].SetActive(true);
            }
            else
            {
                this.lockObjs[1].SetActive(false);
            }
            if (((this.targets == 2) && (Random.value < this.lockTime)) || (this.targets > 2))
            {
                this.lockObjs[2].transform.position = this.targetThree.position;
                this.lockObjs[2].SetActive(true);
            }
            else
            {
                this.lockObjs[2].SetActive(false);
            }
        }
        else
        {
            this.lockObjs[0].SetActive(false);
            this.lockObjs[1].SetActive(false);
            this.lockObjs[2].SetActive(false);
        }
        if (this.gscript.chargeLevel > 0)
        {
            if ((Random.value < this.lockTime) || (Random.value < 0.2f))
            {
                this.line.enabled = true;
            }
            else
            {
                this.line.enabled = false;
            }
            this.LockOn();
        }
        else
        {
            if (Time.time > this.nextLockTime)
            {
                this.lockedOn = false;
                this.targetT = null;
                this.lockTime = 0;
                this.line.enabled = false;
            }
            else
            {
                this.line.enabled = false;
                return;
            }
        }
    }

    public virtual void LockOn()
    {
        RaycastHit hit = default(RaycastHit);
        if (this.targets >= 3)
        {
            return;
        }
        if (Time.time < this.nextLockTime)
        {
            return;
        }
        if (this.lockTime > this.lockThreshold)
        {
            this.lockedOn = true;
            this.lockTime = 0;
            this.targets++;
            this.nextLockTime = Time.time + this.delayTime;
            this.gscript.burstCount = this.targets;
            //		audio.priority = 255;
            this.GetComponent<AudioSource>().Play();
            return;
        }
        if (this.targets == 0)
        {
            this.gscript.chargeLevel = 0.1f;
        }
        int layer1 = 1 << PlayerWeapons.playerLayer;
        int layer2 = 1 << 2;
        int layerMask = layer1 | layer2;
        layerMask = ~layerMask;
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(0, 0, 1), out hit, this.lockRange, layerMask))
        {
            if (this.targets == 0)
            {
                if (this.targetT != null)
                {
                    if (this.targetT == hit.transform)
                    {
                        this.line.enabled = true;
                        this.lockTime = this.lockTime + Time.deltaTime;
                    }
                    else
                    {
                        this.lockTime = 0;
                        this.targetT = null;
                    }
                }
            }
            else
            {
                if (this.targets == 1)
                {
                    if (this.targetTwo != null)
                    {
                        if (this.targetTwo == hit.transform)
                        {
                            this.line.enabled = true;
                            this.lockTime = this.lockTime + Time.deltaTime;
                        }
                        else
                        {
                            this.lockTime = 0;
                            this.targetTwo = null;
                        }
                    }
                }
                else
                {
                    if (this.targets == 2)
                    {
                        if (this.targetThree != null)
                        {
                            if (this.targetThree == hit.transform)
                            {
                                this.line.enabled = true;
                                this.lockTime = this.lockTime + Time.deltaTime;
                            }
                            else
                            {
                                this.lockTime = 0;
                                this.targetThree = null;
                            }
                        }
                    }
                }
            }
            if (((EnemyDamageReceiver) hit.transform.GetComponent(typeof(EnemyDamageReceiver))) != null)
            {
                if (this.targets == 0)
                {
                    this.targetT = hit.transform;
                    this.targetTwo = null;
                }
                else
                {
                    if (this.targets == 1)
                    {
                        this.targetTwo = hit.transform;
                        this.targetThree == null;
                    }
                    else
                    {
                        if (this.targets == 2)
                        {
                            this.targetThree = hit.transform;
                        }
                    }
                }
            }
        }
        else
        {
            if (this.targets == 0)
            {
                this.targetT = null;
            }
            else
            {
                if (this.targets == 1)
                {
                    this.targetTwo = null;
                }
                else
                {
                    if (this.targets == 2)
                    {
                        this.targetThree = null;
                    }
                }
            }
            this.lockTime = 0;
        }
    }

}