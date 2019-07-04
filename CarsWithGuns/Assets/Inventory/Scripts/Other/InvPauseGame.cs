using UnityEngine;
using System.Collections;

[System.Serializable]
/*This script can be attached if you want to do one of the following things:
1. Pause/Unpause the game.
2. Enable/Disable the MouseLook component.
3. Lock/Unlock the mouse cursor.
*/
 //Do we want to pause/unpause the game?
 //Do we want to enable/disable the MouseLook component?
//These two variables are used when disabling/enabling the MouseLook component.
 //Do we want to lock/unlock the mouse cursor?
//Storing the components
[UnityEngine.AddComponentMenu("Inventory/Other/Inv Pause Game")]
public partial class InvPauseGame : MonoBehaviour
{
    public bool pauseGame;
    public bool disableMouseLookComponent;
    public Transform ThePlayer;
    public Transform TheCamera;
    public bool lockUnlockCursor;
    private Behaviour lookAround01;
    private Behaviour lookAround02;
    //Checking for the Inventory object and loading in components.
    public virtual void Awake()
    {
        if (this.transform.name != "Inventory")
        {
            Debug.LogError(("A 'InvPauseGame' script is attached to " + this.transform.name) + ". It needs to be attached to an 'Inventory' object.");
        }
        if (this.disableMouseLookComponent == true)
        {
            if ((this.ThePlayer != null) && (this.TheCamera != null))
            {
                if ((this.ThePlayer.GetComponent("MouseLookDBJS") != null) && (this.TheCamera.GetComponent("MouseLookDBJS") != null))
                {
                    this.lookAround01 = (Behaviour) this.ThePlayer.GetComponent("MouseLookDBJS");
                    this.lookAround02 = (Behaviour) this.TheCamera.GetComponent("MouseLookDBJS");
                }
                else
                {
                    Debug.LogError(("The 'InvPauseGame' script on " + this.transform.name) + " has a variable called 'disableMouseLookComponent' which is set to true though no MouseLook component can be found under (either) the Player or Camera");
                    this.disableMouseLookComponent = false;
                }
            }
            else
            {
                Debug.LogError(("The variables of the 'InvPauseGame' script on '" + this.transform.name) + "' has not been assigned.");
                this.disableMouseLookComponent = false;
            }
        }
    }

    //This function is called from the InventoryDisplay and Character script.
    public virtual void PauseGame(bool pauseIt)
    {
         //Locking the cursor
        if (this.lockUnlockCursor == true)
        {
            if (pauseIt == true)
            {
                Screen.lockCursor = false;
            }
            else
            {
                Screen.lockCursor = true;
            }
        }
        //Pausing the game
        if (this.pauseGame == true)
        {
            if (pauseIt == true)
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }
        //Disabling the MouseLook component
        if (this.disableMouseLookComponent == true)
        {
            if ((this.ThePlayer != null) && (this.TheCamera != null))
            {
                if (pauseIt == true)
                {
                    this.lookAround01.enabled = false;
                    this.lookAround02.enabled = false;
                }
                else
                {
                    this.lookAround01.enabled = true;
                    this.lookAround02.enabled = true;
                }
            }
            else
            {
                Debug.LogError(("The variables of the 'InvPauseGame' script on '" + this.transform.name) + "' has not been assigned.");
            }
        }
    }

    public InvPauseGame()
    {
        this.pauseGame = true;
        this.disableMouseLookComponent = true;
        this.lockUnlockCursor = true;
    }

}