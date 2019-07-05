using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SecondaryReloadAnim : MonoBehaviour
{
    public string val;
    private string cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryReloadAnim;
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryReloadAnim = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryReloadAnim = this.cache;
    }

}