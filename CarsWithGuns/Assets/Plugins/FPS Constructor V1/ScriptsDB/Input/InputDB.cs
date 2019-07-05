using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class InputDB : MonoBehaviour
{
    public static InputDB thisObj;
    public InputItem[] buttons;
    public InputItem[] axes;
    public static bool updated;
    public virtual void Awake()
    {
        InputDB.thisObj = this;
    }

    public static bool GetButtonDown(string s)
    {
        /*if(!updated){
		thisObj.BroadcastMessage("UpdateInput");
		updated = true;
	}*/
        int i = 0;
        while (i < InputDB.thisObj.buttons.Length)
        {
            if (s == InputDB.thisObj.buttons[i].id)
            {
                InputDB.thisObj.buttons[i].BroadcastMessage("UpdateInput", SendMessageOptions.DontRequireReceiver);
                return InputDB.thisObj.buttons[i].down;
            }
            i++;
        }
        return false;
    }

    public static bool GetButton(string s)
    {
        /*if(!updated){
		thisObj.BroadcastMessage("UpdateInput");
		updated = true;
	}*/
        int i = 0;
        while (i < InputDB.thisObj.buttons.Length)
        {
            if (s == InputDB.thisObj.buttons[i].id)
            {
                InputDB.thisObj.buttons[i].BroadcastMessage("UpdateInput", SendMessageOptions.DontRequireReceiver);
                return InputDB.thisObj.buttons[i].got;
            }
            i++;
        }
        return false;
    }

    public static bool GetButtonUp(string s)
    {
        /*if(!updated){
		thisObj.BroadcastMessage("UpdateInput");
		updated = true;
	}*/
        int i = 0;
        while (i < InputDB.thisObj.buttons.Length)
        {
            if (s == InputDB.thisObj.buttons[i].id)
            {
                InputDB.thisObj.buttons[i].BroadcastMessage("UpdateInput", SendMessageOptions.DontRequireReceiver);
                return InputDB.thisObj.buttons[i].up;
            }
            i++;
        }
        return false;
    }

    public static float GetAxis(string s)
    {
        /*if(!updated){
		thisObj.BroadcastMessage("UpdateInput");
		updated = true;
	}*/
        int i = 0;
        while (i < InputDB.thisObj.axes.Length)
        {
            if (s == InputDB.thisObj.axes[i].id)
            {
                InputDB.thisObj.axes[i].BroadcastMessage("UpdateInput", SendMessageOptions.DontRequireReceiver);
                return InputDB.thisObj.axes[i].axis;
            }
            i++;
        }
        return false;
    }

    public static void ResetInputAxes()
    {
        int i = 0;
        while (i < InputDB.thisObj.axes.Length)
        {
            InputDB.thisObj.axes[i].axis = 0;
            i++;
        }
    }

    public virtual void LateUpdate()
    {
        InputDB.updated = false;
    }

}