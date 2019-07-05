using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MouseButton : MonoBehaviour
{
    public int key;
    public InputItem input;
    public virtual void UpdateInput()
    {
        //Just get the values from Unity's input
        this.input.got = Input.GetMouseButton(this.key);
        this.input.down = Input.GetMouseButtonDown(this.key);
        this.input.up = Input.GetMouseButtonUp(this.key);
    }

}