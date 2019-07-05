using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CameraShake : MonoBehaviour
{
    public float multiplier;
    private float curAmp;
    private Vector3 lastVal;
    private float timeVal;
    private float amplitude;
    private float r;
    public static CameraShake[] shakers;
    public static object[] tempShakers;
    public static bool builtin;
    public virtual void Awake()
    {
        CameraShake.tempShakers.Add(this);
    }

    public virtual void Start()
    {
        if (!CameraShake.builtin)
        {
            CameraShake.shakers = CameraShake.tempShakers.ToBuiltin(typeof(CameraShake)) as CameraShake[];
            CameraShake.builtin = true;
        }
    }

    public static void ShakeCam(float a, float rT, float time)//}
    {
    }

    public virtual void Shake(float a, float rT, float time)
    {
        this.amplitude = a * this.multiplier;
        this.curAmp = this.amplitude;
        this.timeVal = time;
        this.r = rT;
    }

    public virtual void LateUpdate()
    {
        //if(InputDB.GetButtonDown("Interact")){
        //	ShakeCam(.14, 10, .4);
        //}
        if (this.curAmp > 0)
        {
            Vector3 amt = Random.insideUnitSphere * this.curAmp;
            this.transform.localPosition = this.transform.localPosition - this.lastVal;
            this.transform.localEulerAngles = this.transform.localEulerAngles - (this.lastVal * this.r);
            this.transform.localEulerAngles = this.transform.localEulerAngles + (amt * this.r);
            this.transform.localPosition = this.transform.localPosition + amt;
            this.lastVal = amt;
            this.curAmp = this.curAmp - ((Time.deltaTime * this.amplitude) / this.timeVal);
        }
    }

    public CameraShake()
    {
        this.multiplier = 1;
    }

    static CameraShake()
    {
        CameraShake.tempShakers = new object[0];
    }

}