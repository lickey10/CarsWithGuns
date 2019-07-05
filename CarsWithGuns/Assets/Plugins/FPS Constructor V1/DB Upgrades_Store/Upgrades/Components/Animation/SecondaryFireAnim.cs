using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SecondaryFireAnim : MonoBehaviour
{
    public string val;
    private string cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryFireAnim;
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryFireAnim = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).secondaryFireAnim = this.cache;
    }

}