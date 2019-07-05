using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GlobalUpgrade : MonoBehaviour
{
    public Upgrade upgrade;
    public static WeaponInfo[] WeaponArray;
    private Upgrade[] Upgrades;
    private bool applied;
    public bool[] classesAllowed;
    public virtual void Start()
    {
        if (GlobalUpgrade.WeaponArray == null)
        {
            GlobalUpgrade.WeaponArray = ((WeaponInfo[]) UnityEngine.Object.FindObjectsOfType(typeof(WeaponInfo))) as WeaponInfo[];
        }
    }

    public virtual void Apply()
    {
        this.applied = true;
        Transform temp = null;
        Upgrade up = null;
        object[] upgradeArray = new object[0];
        int i = 0;
        while (i < GlobalUpgrade.WeaponArray.Length)
        {
            int enumIndex = (int) GlobalUpgrade.WeaponArray[i].weaponClass;
            if (this.classesAllowed[enumIndex])
            {
                temp = UnityEngine.Object.Instantiate(this.upgrade.gameObject, this.transform.position, this.transform.rotation).transform;
                temp.parent = GlobalUpgrade.WeaponArray[i].transform;
                temp.name = this.upgrade.upgradeName;
                up = (Upgrade) temp.GetComponent(typeof(Upgrade));
                up.Init();
                up.ApplyUpgrade();
                up.showInStore = false;
                upgradeArray.Push(up);
            }
            i++;
        }
        this.Upgrades = upgradeArray.ToBuiltin(typeof(Upgrade)) as Upgrade[];
        this.SendMessage("Apply");
    }

    public virtual void UnApply()
    {
        this.applied = false;
        this.SendMessage("Remove");
        int i = 0;
        while (i < this.Upgrades.Length)
        {
            this.Upgrades[i].DeleteUpgrade();
            i++;
        }
    }

}