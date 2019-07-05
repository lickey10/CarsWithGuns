using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class BulletPenetration : MonoBehaviour
{
    public int penetrateValue;
    public BulletPenetration()
    {
        this.penetrateValue = 1;
    }

}