using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ReloadAnim : MonoBehaviour
{
    public string val;
    private string cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).reloadAnim;
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).reloadAnim = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        ((GunChildAnimation) gScript.GetComponentInChildren(typeof(GunChildAnimation))).reloadAnim = this.cache;
    }

}