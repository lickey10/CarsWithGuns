using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TimedObjectDestructorDB : MonoBehaviour
{
    public float timeOut;
    public bool detachChildren;
    public virtual void Awake()
    {
        this.Invoke("DestroyNow", this.timeOut);
    }

    public virtual void DestroyNow()
    {
        if (this.detachChildren)
        {
            this.transform.DetachChildren();
        }
        UnityEngine.Object.DestroyObject(this.gameObject);
    }

    public TimedObjectDestructorDB()
    {
        this.timeOut = 1f;
    }

}