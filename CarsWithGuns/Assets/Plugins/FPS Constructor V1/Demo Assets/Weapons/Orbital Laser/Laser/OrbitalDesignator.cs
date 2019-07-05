using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class OrbitalDesignator : MonoBehaviour
{
    public GunScript gscript;
    public Transform designator;
    private Light desigLight;
    public Transform laser;
    public float lockTime;
    public float lockRange;
    public float targetError;
    private bool lockedOn;
    public float lockMax;
    public LineRenderer line;
    public virtual void Start()
    {
        this.desigLight = (Light) this.designator.GetComponentInChildren(typeof(Light));
        this.line = (LineRenderer) this.GetComponent(typeof(LineRenderer));
    }

    public virtual void Update()
    {
        RaycastHit hit = default(RaycastHit);
        if (Random.value < (this.lockTime / this.lockMax))
        {
            this.line.enabled = true;
            this.desigLight.enabled = true;
            this.desigLight.transform.GetComponent<AudioSource>().volume = this.lockTime / this.lockMax;
            this.desigLight.transform.GetComponent<AudioSource>().pitch = (this.lockTime / this.lockMax) * 3;
            if (!this.desigLight.transform.GetComponent<AudioSource>().isPlaying)
            {
                this.desigLight.transform.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            this.line.enabled = false;
            this.desigLight.enabled = false;
        }
        if (this.lockTime <= 0)
        {
            this.desigLight.transform.GetComponent<AudioSource>().Stop();
        }
        if (this.gscript.chargeLevel <= 0)
        {
            this.lockTime = Mathf.Clamp(this.lockTime - Time.deltaTime, 0, this.lockMax);
            this.lockedOn = false;
            //desigLight.enabled = false;
            this.line.enabled = false;
            return;
        }
        if ((this.lockTime >= this.lockMax) || this.lockedOn)
        {
            this.lockedOn = true;
            if (this.gscript.chargeLevel < this.gscript.minCharge)
            {
                this.gscript.chargeLevel = this.gscript.minCharge;
            }
            this.lockTime = 0;
            return;
        }
        if (!this.lockedOn)
        {
            this.gscript.chargeLevel = 0.1f;
        }
        if (this.lockTime > 0)
        {
            Quaternion temp = Quaternion.LookRotation(this.designator.position - this.transform.position);
            float tAngle = Quaternion.Angle(this.transform.rotation, temp);
            if (tAngle <= this.targetError)
            {
                this.lockTime = Mathf.Clamp(this.lockTime + Time.deltaTime, 0, this.lockMax);
            }
            else
            {
                this.lockTime = Mathf.Clamp(this.lockTime - Time.deltaTime, 0, this.lockMax);
            }
        }
        else
        {
            int layer1 = 1 << PlayerWeapons.playerLayer;
            int layer2 = 1 << 2;
            int layerMask = layer1 | layer2;
            layerMask = ~layerMask;
            if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(0, 0, 1), out hit, this.lockRange, layerMask))
            {
                if (this.lockTime <= 0)
                {
                    this.designator.position = hit.point;
                    this.lockTime = Mathf.Clamp(this.lockTime + Time.deltaTime, 0, this.lockMax);
                }
            }
        }
    }

}