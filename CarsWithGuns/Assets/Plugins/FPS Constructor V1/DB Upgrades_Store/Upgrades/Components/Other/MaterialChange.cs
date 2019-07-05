using UnityEngine;
using System.Collections;

[System.Serializable]
public class rendererChanged : object
{
    public Renderer r;
    public int index;
    public rendererChanged(Renderer render1, int num)
    {
        this.r = render1;
        this.index = num;
    }

}
[System.Serializable]
public partial class MaterialChange : MonoBehaviour
{
    private Renderer gscript;
    private rendererChanged[] materialsChanged;
    public Material startMat;
    public Material targetMat;
    private string name1;
    private float cache;
    private bool applied;
    public virtual void Start()
    {
        this.findMaterials();
    }

    public virtual void Apply()
    {
        this.findMaterials();
        this.applied = true;
        int i = 0;
        while (i < this.materialsChanged.Length)
        {
            Renderer renderer1 = this.materialsChanged[i].r;
            Material[] tempArray = new Material[renderer1.materials.Length];
            int q = 0;
            while (q < renderer1.materials.Length)
            {
                tempArray[q] = renderer1.materials[q];
                q++;
            }
            tempArray[this.materialsChanged[i].index] = this.targetMat;
            this.materialsChanged[i].r.materials = tempArray;
            i++;
        }
    }

    public virtual void Remove()
    {
        this.applied = false;
        int i = 0;
        while (i < this.materialsChanged.Length)
        {
            Renderer renderer1 = this.materialsChanged[i].r;
            Material[] tempArray = new Material[renderer1.materials.Length];
            int q = 0;
            while (q < renderer1.materials.Length)
            {
                tempArray[q] = renderer1.materials[q];
                q++;
            }
            tempArray[this.materialsChanged[i].index] = this.startMat;
            this.materialsChanged[i].r.materials = tempArray;
            i++;
        }
    }

    public virtual void findMaterials()
    {
        Renderer[] gscripts = this.transform.parent.GetComponentsInChildren<Renderer>() as Renderer[];
        object[] temp = new object[0];
        this.name1 = this.startMat.name + " (Instance)";
        int q = 0;
        while (q < gscripts.Length)
        {
            int w = 0;
            while (w < gscripts[q].materials.Length)
            {
                if (gscripts[q].materials[w].name.Equals(this.name1))
                {
                    rendererChanged rc = new rendererChanged(gscripts[q], w);
                    temp.Add(rc);
                }
                w++;
            }
            q++;
        }
        this.materialsChanged = temp.ToBuiltin(typeof(rendererChanged));
    }

    public virtual void reapply()
    {
        if (this.applied)
        {
            this.Remove();
            this.Apply();
        }
    }

}