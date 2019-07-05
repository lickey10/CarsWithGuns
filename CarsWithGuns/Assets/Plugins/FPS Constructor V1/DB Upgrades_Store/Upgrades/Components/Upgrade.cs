using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Upgrade : MonoBehaviour
{
    [UnityEngine.HideInInspector]
    public bool applied;
    public bool owned;
    public bool locked;
    public string upgradeType;
    public string upgradeName;
    public string description;
    public string lockedDescription;
    public float buyPrice;
    public float sellPrice;
    public int scriptID;
    public bool showInStore;
    private GunScript gScript;
    public virtual void Start()
    {
        this.Init();
    }

    public virtual void Init()
    {
        GunScript[] gscripts = this.transform.parent.GetComponents<GunScript>() as GunScript[];
        int q = 0;
        while (q < gscripts.Length)
        {
            if (q == this.scriptID)
            {
                this.gScript = gscripts[q];
            }
            q++;
        }
    }

    public virtual void ApplyUpgrade()
    {
        Upgrade[] upgrades = null;
        upgrades = this.transform.parent.GetComponentsInChildren<Upgrade>();
        int i = 0;
        while (i < upgrades.Length)
        {
            if ((upgrades[i].upgradeType == this.upgradeType) && (upgrades[i] != this))
            {
                upgrades[i].RemoveUpgrade();
            }
            i++;
        }
        if (this.applied)
        {
            return;
        }
        this.SendMessage("Apply", this.gScript);
        this.applied = true;
        this.SendMessageUpwards("ApplyUpgrade");
    }

    public virtual void ApplyUpgradeInstant()
    {
        if (this.applied)
        {
            return;
        }
        this.BroadcastMessage("TempInstant");
        this.ApplyUpgrade();
    }

    public virtual void RemoveUpgrade()
    {
        if (!this.applied)
        {
            return;
        }
        this.SendMessage("Remove", this.gScript);
        this.applied = false;
    }

    public virtual void RemoveUpgradeInstant()
    {
        if (!this.applied)
        {
            return;
        }
        this.SendMessage("TempInstant");
        this.RemoveUpgrade();
    }

    public virtual void DeleteUpgrade()
    {
        this.RemoveUpgrade();
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public Upgrade()
    {
        this.description = "Upgrade Locked";
        this.showInStore = true;
    }

}