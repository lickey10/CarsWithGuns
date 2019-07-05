using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HitDirectional : MonoBehaviour
{
    private Transform hitPos;
    private Vector3 lastHitPos;
    private float dirEffectTime;
    public Transform obj;
    public Vector3 thePos;
    public Vector3 theRot;
    public virtual IEnumerator Init(Transform pos, float time)
    {
        this.hitPos = pos;
        this.dirEffectTime = time;
        this.transform.localPosition = this.thePos;
        this.transform.localEulerAngles = this.theRot;
        yield return new WaitForSeconds(time);
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public virtual void LateUpdate()
    {
        float temp = 0.0f;
        this.obj.GetComponent<Renderer>().material.color.a = this.dirEffectTime;
        this.dirEffectTime = this.dirEffectTime - Time.deltaTime;
        if (this.hitPos != null)
        {
            this.lastHitPos = this.hitPos.position;
        }
        if (((this.dirEffectTime > 0) && this.hitPos) && (this.obj != null))
        {
            Vector3 hitDir = new Vector3(this.lastHitPos.x, 0, this.lastHitPos.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z);
            Vector3 relativePoint = this.transform.InverseTransformPoint(this.lastHitPos);
            if (relativePoint.x < 0f)
            {
                temp = -Vector3.Angle(PlayerWeapons.mainCam.transform.forward, hitDir);
            }
            else
            {
                if (relativePoint.x > 0f)
                {
                    temp = Vector3.Angle(PlayerWeapons.mainCam.transform.forward, hitDir);
                }
                else
                {
                    temp = 0;
                }
            }
            this.obj.transform.localEulerAngles.y = temp + 180;
        }
    }

}