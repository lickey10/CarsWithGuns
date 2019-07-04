using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class testing : MonoBehaviour
{
    public GUIText[] guiAr;
    public virtual void Update()
    {
        this.guiAr[0].text = "Execution of the Update function: " + Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }

    public virtual void FixedUpdate()
    {
        this.guiAr[1].text = "Execution of the FixedUpdate function: " + Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }

    public virtual void OnGUI()
    {
        this.guiAr[2].text = "Execution of the OnGUI function: " + Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }

}