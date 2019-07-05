using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayAnim : MonoBehaviour
{
    public string removeAnim;
    public string applyAnim;
    public virtual void Apply(GunScript g)
    {
        if ((this.applyAnim != "") && g.gunActive)
        {
            this.transform.parent.BroadcastMessage("PlayAnim", this.applyAnim);
        }
    }

    public virtual void Remove(GunScript g)
    {
        if ((this.removeAnim != "") && g.gunActive)
        {
            this.transform.parent.BroadcastMessage("PlayAnim", this.applyAnim);
        }
    }

}