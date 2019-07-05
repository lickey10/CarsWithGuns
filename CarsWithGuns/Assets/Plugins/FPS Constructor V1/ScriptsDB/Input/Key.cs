using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Key : MonoBehaviour
{
    public string key;
    public InputItem input;
    public virtual void UpdateInput()
    {
        //Just get the values from Unity's input
        this.input.got = Input.GetButton(this.key);
        this.input.down = Input.GetButtonDown(this.key);
        this.input.up = Input.GetButtonUp(this.key);
    }

}