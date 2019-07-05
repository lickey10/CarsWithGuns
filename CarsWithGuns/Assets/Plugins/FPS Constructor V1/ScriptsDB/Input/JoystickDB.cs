using UnityEngine;
using System.Collections;

//////////////////////////////////////////////////////////////
// Joystick.js
// Penelope iPhone Tutorial
//
// Joystick creates a movable joystick (via GUITexture) that 
// handles touch input, taps, and phases. Dead zones can control
// where the joystick input gets picked up and can be normalized.
//
// Optionally, you can enable the touchPad property from the editor
// to treat this Joystick as a TouchPad. A TouchPad allows the finger
// to touch down at any point and it tracks the movement relatively 
// without moving the graphic
//////////////////////////////////////////////////////////////
// A simple class for bounding how far the GUITexture will move
[System.Serializable]
public class Boundary : object
{
    public Vector2 min;
    public Vector2 max;
    public Boundary()
    {
        this.min = Vector2.zero;
        this.max = Vector2.zero;
    }

}
[System.Serializable]
[UnityEngine.RequireComponent(typeof(GUITexture))]
public partial class JoystickDB : MonoBehaviour
{
    private static JoystickDB[] joysticks; // A static collection of all joysticks
    private static bool enumeratedJoysticks;
    private static float tapTimeDelta; // Time allowed between taps
    public bool touchPad; // Is this a TouchPad?
    public Rect touchZone;
    public Vector2 deadZone; // Control when position is output
    public bool normalize; // Normalize output after the dead-zone?
    public Vector2 position; // [-1, 1] in x,y
    public int tapCount; // Current tap count
    public InputItem inputX; // Input item to modify
    public InputItem inputY; // Input item to modify
    public float sensitivity;
    private int lastFingerId; // Finger last used for this joystick
    private float tapTimeWindow; // How much time there is left for a tap to occur
    private Vector2 fingerDownPos;
    private float fingerDownTime;
    private float firstDeltaTime;
    private GUITexture gui; // Joystick graphic
    private Rect defaultRect; // Default position / extents of the joystick graphic
    private Boundary guiBoundary; // Boundary for joystick graphic
    private Vector2 guiTouchOffset; // Offset to apply to touch input
    private Vector2 guiCenter; // Center of joystick
    public virtual void Start()
    {
         // Cache this component at startup instead of looking up every frame	
        this.gui = (GUITexture) this.GetComponent(typeof(GUITexture));
        // Store the default rect for the gui, so we can snap back to it
        this.defaultRect = this.gui.pixelInset;
        this.defaultRect.x = this.defaultRect.x + (this.transform.position.x * Screen.width);// + gui.pixelInset.x; // -  Screen.width * 0.5;
        this.defaultRect.y = this.defaultRect.y + (this.transform.position.y * Screen.height);// - Screen.height * 0.5;
        this.transform.position.x = 0f;
        this.transform.position.y = 0f;
        if (this.touchPad)
        {
             // If a texture has been assigned, then use the rect ferom the gui as our touchZone
            if (this.gui.texture)
            {
                this.touchZone = this.defaultRect;
            }
        }
        else
        {
             // This is an offset for touch input to match with the top left
             // corner of the GUI
            this.guiTouchOffset.x = this.defaultRect.width * 0.5f;
            this.guiTouchOffset.y = this.defaultRect.height * 0.5f;
            // Cache the center of the GUI, since it doesn't change
            this.guiCenter.x = this.defaultRect.x + this.guiTouchOffset.x;
            this.guiCenter.y = this.defaultRect.y + this.guiTouchOffset.y;
            // Let's build the GUI boundary, so we can clamp joystick movement
            this.guiBoundary.min.x = this.defaultRect.x - this.guiTouchOffset.x;
            this.guiBoundary.max.x = this.defaultRect.x + this.guiTouchOffset.x;
            this.guiBoundary.min.y = this.defaultRect.y - this.guiTouchOffset.y;
            this.guiBoundary.max.y = this.defaultRect.y + this.guiTouchOffset.y;
        }
    }

    public virtual void Disable()
    {
        this.gameObject.SetActive(false);
        JoystickDB.enumeratedJoysticks = false;
    }

    public virtual void ResetJoystick()
    {
         // Release the finger control and set the joystick back to the default position
        this.gui.pixelInset = this.defaultRect;
        this.lastFingerId = -1;
        this.position = Vector2.zero;
        this.fingerDownPos = Vector2.zero;
        if (this.touchPad)
        {
            this.gui.color.a = 0.025f;
        }
    }

    public virtual bool IsFingerDown()
    {
        return this.lastFingerId != -1;
    }

    public virtual void LatchedFinger(int fingerId)
    {
         // If another joystick has latched this finger, then we must release it
        if (this.lastFingerId == fingerId)
        {
            this.ResetJoystick();
        }
    }

    public virtual void Update()
    {
        if (!JoystickDB.enumeratedJoysticks)
        {
             // Collect all joysticks in the game, so we can relay finger latching messages
            JoystickDB.joysticks = ((JoystickDB[]) UnityEngine.Object.FindObjectsOfType(typeof(JoystickDB))) as JoystickDB[];
            JoystickDB.enumeratedJoysticks = true;
        }
        int count = Input.touchCount;
        // Adjust the tap time window while it still available
        if (this.tapTimeWindow > 0)
        {
            this.tapTimeWindow = this.tapTimeWindow - Time.deltaTime;
        }
        else
        {
            this.tapCount = 0;
        }
        if (count == 0)
        {
            this.ResetJoystick();
        }
        else
        {
            int i = 0;
            while (i < count)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 guiTouchPos = touch.position - this.guiTouchOffset;
                bool shouldLatchFinger = false;
                if (this.touchPad)
                {
                    if (this.touchZone.Contains(touch.position))
                    {
                        shouldLatchFinger = true;
                    }
                }
                else
                {
                    if (this.gui.HitTest(touch.position))
                    {
                        shouldLatchFinger = true;
                    }
                }
                // Latch the finger if this is a new touch
                if (shouldLatchFinger && ((this.lastFingerId == -1) || (this.lastFingerId != touch.fingerId)))
                {
                    if (this.touchPad)
                    {
                        this.gui.color.a = 0.15f;
                        this.lastFingerId = touch.fingerId;
                        this.fingerDownPos = touch.position;
                        this.fingerDownTime = Time.time;
                    }
                    this.lastFingerId = touch.fingerId;
                    // Accumulate taps if it is within the time window
                    if (this.tapTimeWindow > 0)
                    {
                        this.tapCount++;
                    }
                    else
                    {
                        this.tapCount = 1;
                        this.tapTimeWindow = JoystickDB.tapTimeDelta;
                    }
                    // Tell other joysticks we've latched this finger
                    foreach (JoystickDB j in JoystickDB.joysticks)
                    {
                        if (j != this)
                        {
                            j.LatchedFinger(touch.fingerId);
                        }
                    }
                }
                if (this.lastFingerId == touch.fingerId)
                {
                     // Override the tap count with what the iPhone SDK reports if it is greater
                     // This is a workaround, since the iPhone SDK does not currently track taps
                     // for multiple touches
                    if (touch.tapCount > this.tapCount)
                    {
                        this.tapCount = touch.tapCount;
                    }
                    if (this.touchPad)
                    {
                         // For a touchpad, let's just set the position directly based on distance from initial touchdown
                        this.position.x = Mathf.Clamp((touch.position.x - this.fingerDownPos.x) / (this.touchZone.width / 2), -1, 1);
                        this.position.y = Mathf.Clamp((touch.position.y - this.fingerDownPos.y) / (this.touchZone.height / 2), -1, 1);
                    }
                    else
                    {
                         // Change the location of the joystick graphic to match where the touch is
                        this.gui.pixelInset.x = Mathf.Clamp(guiTouchPos.x, this.guiBoundary.min.x, this.guiBoundary.max.x);
                        this.gui.pixelInset.y = Mathf.Clamp(guiTouchPos.y, this.guiBoundary.min.y, this.guiBoundary.max.y);
                    }
                    if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled))
                    {
                        this.ResetJoystick();
                    }
                }
                i++;
            }
        }
        if (!this.touchPad)
        {
             // Get a value between -1 and 1 based on the joystick graphic location
            this.position.x = ((this.gui.pixelInset.x + this.guiTouchOffset.x) - this.guiCenter.x) / this.guiTouchOffset.x;
            this.position.y = ((this.gui.pixelInset.y + this.guiTouchOffset.y) - this.guiCenter.y) / this.guiTouchOffset.y;
        }
        // Adjust for dead zone	
        float absoluteX = Mathf.Abs(this.position.x);
        float absoluteY = Mathf.Abs(this.position.y);
        if (absoluteX < this.deadZone.x)
        {
             // Report the joystick as being at the center if it is within the dead zone
            this.position.x = 0;
        }
        else
        {
            if (this.normalize)
            {
                 // Rescale the output after taking the dead zone into account
                this.position.x = (Mathf.Sign(this.position.x) * (absoluteX - this.deadZone.x)) / (1 - this.deadZone.x);
            }
        }
        if (absoluteY < this.deadZone.y)
        {
             // Report the joystick as being at the center if it is within the dead zone
            this.position.y = 0;
        }
        else
        {
            if (this.normalize)
            {
                 // Rescale the output after taking the dead zone into account
                this.position.y = (Mathf.Sign(this.position.y) * (absoluteY - this.deadZone.y)) / (1 - this.deadZone.y);
            }
        }
        this.inputX.axis = this.position.x * this.sensitivity;
        this.inputY.axis = this.position.y * this.sensitivity;
    }

    public JoystickDB()
    {
        this.deadZone = Vector2.zero;
        this.sensitivity = 1;
        this.lastFingerId = -1;
        this.firstDeltaTime = 0.5f;
        this.guiBoundary = new Boundary();
    }

    static JoystickDB()
    {
        JoystickDB.tapTimeDelta = 0.3f;
    }

}