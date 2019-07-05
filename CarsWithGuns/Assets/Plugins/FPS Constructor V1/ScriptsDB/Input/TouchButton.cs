using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TouchButton : MonoBehaviour
{
    public InputItem input;
    public Vector2 position; //position of button
    public Vector2 dimensions; //size of button
    public string label; //text in button
    public bool toggle; //is this button a toggle?
    [UnityEngine.HideInInspector]
    public bool toggled; //are we currently toggled on?
    public bool showInStore;
    private bool used;
    private bool touched; //had we already touched the button
    private bool touching; //are we currently touching the button
    [UnityEngine.HideInInspector]
    public int curTouch; //what touch id is this using?
    public bool useUpdate;
    public virtual void Update()
    {
        if (this.useUpdate)
        {
            this.UpdateFunction();
        }
    }

    public virtual void UpdateInput()
    {
        if (!this.useUpdate)
        {
            this.UpdateFunction();
        }
    }

    public virtual void UpdateFunction()
    {
        //are we touching the button this frame?
        if (Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                //Is this touch within our button?
                this.touching = this.Within(touch.position, new Rect(this.position.x, this.position.y, this.dimensions.x, this.dimensions.y));
                if (this.touching)
                {
                    this.curTouch = touch.fingerId; //save which touch we are using
                    break;
                }
            }
        }
        else
        {
            this.touching = false;
        }
        if (this.toggle) //Toggle button
        {
            this.input.got = this.toggled;
            if (this.touching)
            {
                if (!this.touched) //first frame touching the button
                {
                    this.touched = true;
                    this.input.up = this.toggled;
                    this.toggled = !this.toggled; //invert the toggle
                    this.input.down = this.toggled;
                }
                else
                {
                    this.input.down = false;
                    this.input.up = false;
                }
            }
            else
            {
                this.input.down = false;
                this.input.up = false;
                this.touched = false;
                this.curTouch = -1;
            }
        }
        else
        {
             //Normal Button
            if (this.touching) //We are touching
            {
                this.input.got = true; //the button is down
                this.input.up = false; //the button is not up
                if (!this.touched)// we hadn't already touched the button (first frame holding it)
                {
                    this.input.down = true; //the button was got
                    this.touched = true; //we have touched	
                }
                else
                {
                    this.input.down = false; //it isn't down because this isn't the first fram holding it
                }
            }
            else
            {
                 //We are not touching
                this.curTouch = -1;
                if (this.touched)
                {
                    this.input.up = true; //if we were holding the button last fram, then up is true because this is the frame it was released
                }
                else
                {
                    this.input.up = false;
                }
                this.touched = false;
                this.input.got = false;
                this.input.down = false;
            }
        }
    }

    public virtual void OnGUI()
    {
        if (!DBStoreController.inStore || this.showInStore)
        {
            GUI.Button(new Rect(this.position.x, this.position.y, this.dimensions.x, this.dimensions.y), this.label);
        }
    }

    public virtual bool Within(Vector2 pos, Rect bounds)
    {
        pos.y = Screen.height - pos.y;
        return (((pos.x > bounds.x) && (pos.x < (bounds.x + bounds.width))) && (pos.y > bounds.y)) && (pos.y < (bounds.y + bounds.height));
    }

    public TouchButton()
    {
        this.curTouch = -1;
        this.useUpdate = true;
    }

}