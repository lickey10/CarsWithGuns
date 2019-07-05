using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LockCursor : MonoBehaviour
{
    public static bool canLock;
    public static object mobile;
    public static bool unPaused;
    public virtual void Awake()
    {
        LockCursor.mobile = true;
    }

    public virtual void Start()
    {
        if (LockCursor.mobile == null)
        {
            LockCursor.SetPause(true);
            LockCursor.canLock = true;
            PlayerWeapons.playerActive = false;
        }
        else
        {
            LockCursor.SetPause(false);
            LockCursor.canLock = false;
            PlayerWeapons.playerActive = true;
        }
    }

    public virtual void OnApplicationQuit()
    {
        Time.timeScale = 1;
    }

    public static void SetPause(bool pause)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (LockCursor.mobile != null)
        {
            return;
        }
        InputDB.ResetInputAxes();
        if (pause)
        {
            PlayerWeapons.playerActive = false;
            //Screen.lockCursor = false;
            Time.timeScale = 0;
            player.BroadcastMessage("Freeze", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            LockCursor.unPaused = true;
            Time.timeScale = 1;
            Screen.lockCursor = true;
            PlayerWeapons.playerActive = true;
            player.BroadcastMessage("UnFreeze", SendMessageOptions.DontRequireReceiver);
        }
    }

    public static void HardUnlock()
    {
        LockCursor.canLock = false;
        Screen.lockCursor = false;
    }

    public static void HardLock()
    {
        LockCursor.canLock = false;
        Screen.lockCursor = true;
    }

    private bool wasLocked;
    public virtual void Update()
    {
        if (!LockCursor.canLock)
        {
            return;
        }
        if (Input.GetMouseButton(0) && (Screen.lockCursor == false))
        {
            LockCursor.SetPause(false);
        }
        if (InputDB.GetButton("Escape"))
        {
            LockCursor.SetPause(true);
        }
        // Did we lose cursor locking?
        // eg. because the user pressed escape
        // or because he switched to another application
        // or because some script set Screen.lockCursor = false;
        if (!Screen.lockCursor && this.wasLocked)
        {
            this.wasLocked = false;
            LockCursor.SetPause(true);
        }
        else
        {
            // Did we gain cursor locking?
            if (Screen.lockCursor && !this.wasLocked)
            {
                this.wasLocked = true;
                LockCursor.SetPause(false);
            }
        }
    }

    public virtual void LateUpdate()
    {
        LockCursor.unPaused = false;
    }

    static LockCursor()
    {
        LockCursor.canLock = true;
    }

}