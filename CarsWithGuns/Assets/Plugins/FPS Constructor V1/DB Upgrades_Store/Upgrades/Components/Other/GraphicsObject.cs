using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GraphicsObject : MonoBehaviour
{
    public GameObject obj;
    public bool val;
    public bool instant;
    private bool cache;
    private bool applied;
    private bool tempInstant;
    public virtual void Apply()
    {
        this.cache = this.obj.activeSelf;
        if (this.val)
        {
            this.obj.SetActive(true);
        }
        else
        {
            this.obj.SetActive(false);
        }
        Component[] gos = this.obj.GetComponentsInChildren(typeof(Renderer));
        Renderer go = null;
        if (!this.instant && !this.tempInstant)
        {
            foreach (Renderer go_20 in gos)
            {
                go = go_20;
                go.enabled = false;
            }
        }
        else
        {
            foreach (Renderer go_24 in gos)
            {
                go = go_24;
                go.enabled = true;
            }
        }
        this.tempInstant = false;
        this.transform.parent.BroadcastMessage("reapply", SendMessageOptions.DontRequireReceiver);
    }

    public virtual void Remove()
    {
        this.obj.SetActive(this.cache);
        if (this.instant || this.tempInstant)
        {
            Component[] gos = this.obj.GetComponentsInChildren(typeof(Renderer));
            Renderer go = null;
            foreach (Renderer go_37 in gos)
            {
                go = go_37;
                go.enabled = true;
            }
            this.tempInstant = false;
        }
    }

    public virtual void TempInstant()
    {
        this.tempInstant = true;
    }

    public GraphicsObject()
    {
        this.val = true;
    }

}