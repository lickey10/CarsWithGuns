using UnityEngine;
using System.Collections;

public enum axes
{
    x = 0,
    y = 1, //which rotation axis should we use?
    z = 2
}

public enum directions
{
    positive = 0, //should it be inverted?
    negative = 1
}

[System.Serializable]
public partial class Tilt : MonoBehaviour
{
    public axes axis;
    public directions direction;
    public InputItem input;
    public float sensitivity;
    public float offset;
    public float buffer;
    public virtual void UpdateInput()
    {
        if (this.axis == axes.x)
        {
            this.input.axis = Input.acceleration.x;
        }
        else
        {
            if (this.axis == axes.y)
            {
                this.input.axis = Input.acceleration.y;
            }
            else
            {
                if (this.axis == axes.z)
                {
                    this.input.axis = Input.acceleration.z;
                }
            }
        }
        this.input.axis = this.input.axis + this.offset;
        if (this.input.axis > 0)
        {
            this.input.axis = Mathf.Clamp(this.input.axis - this.buffer, 0, this.input.axis);
        }
        else
        {
            this.input.axis = Mathf.Clamp(this.input.axis + this.buffer, this.input.axis, 0);
        }
        this.input.axis = this.input.axis * this.sensitivity;
        if (this.direction == directions.negative)
        {
            this.input.axis = this.input.axis * -1;
        }
    }

    public Tilt()
    {
        this.axis = axes.z;
        this.direction = directions.positive;
    }

}