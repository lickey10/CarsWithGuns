using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ConstantShake : MonoBehaviour
{
    public float amplitude;
    public float time;
    public virtual void Update()
    {
        CameraShake.ShakeCam(this.amplitude, 10, this.time);
    }

}