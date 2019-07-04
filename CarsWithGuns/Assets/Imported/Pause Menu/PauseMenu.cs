using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PauseMenu : MonoBehaviour
{
    //*******************************************************************************
    //*																							*
    //*							Written by Grady Featherstone								*
    //										© Copyright 2011										*
    //*******************************************************************************
    public string mainMenuSceneName;
    public Font pauseMenuFont;
    private bool pauseEnabled;
    public virtual void Start()
    {
        this.pauseEnabled = false;
        Time.timeScale = 1;
        AudioListener.volume = 1;
        Cursor.visible = false;
    }

    public virtual void Update()
    {
        //check if pause button (escape key) is pressed
        if (Input.GetKeyDown("escape"))
        {
            //check if game is already paused		
            if (this.pauseEnabled == true)
            {
                //unpause the game
                this.pauseEnabled = false;
                Time.timeScale = 1;
                AudioListener.volume = 1;
                Cursor.visible = false;
            }
            else
            {
                //else if game isn't paused, then pause it
                if (this.pauseEnabled == false)
                {
                    this.pauseEnabled = true;
                    AudioListener.volume = 0;
                    Time.timeScale = 0;
                    Cursor.visible = true;
                }
            }
        }
    }

    private bool showGraphicsDropDown;
    public virtual void OnGUI()
    {
        GUI.skin.box.font = this.pauseMenuFont;
        GUI.skin.button.font = this.pauseMenuFont;
        if (this.pauseEnabled == true)
        {
            //Make a background box
            GUI.Box(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 250, 200), "Pause Menu");
            //Make Main Menu button
            if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 50, 250, 50), "Main Menu"))
            {
                Application.LoadLevel(this.mainMenuSceneName);
            }
            //Make Change Graphics Quality button
            if (GUI.Button(new Rect((Screen.width / 2) - 100, Screen.height / 2, 250, 50), "Change Graphics Quality"))
            {
                if (this.showGraphicsDropDown == false)
                {
                    this.showGraphicsDropDown = true;
                }
                else
                {
                    this.showGraphicsDropDown = false;
                }
            }
            //Create the Graphics settings buttons, these won't show automatically, they will be called when
            //the user clicks on the "Change Graphics Quality" Button, and then dissapear when they click
            //on it again....
            if (this.showGraphicsDropDown == true)
            {
                if (GUI.Button(new Rect((Screen.width / 2) + 150, Screen.height / 2, 250, 50), "Fastest"))
                {
                    QualitySettings.currentLevel = QualityLevel.Fastest;
                }
                if (GUI.Button(new Rect((Screen.width / 2) + 150, (Screen.height / 2) + 50, 250, 50), "Fast"))
                {
                    QualitySettings.currentLevel = QualityLevel.Fast;
                }
                if (GUI.Button(new Rect((Screen.width / 2) + 150, (Screen.height / 2) + 100, 250, 50), "Simple"))
                {
                    QualitySettings.currentLevel = QualityLevel.Simple;
                }
                if (GUI.Button(new Rect((Screen.width / 2) + 150, (Screen.height / 2) + 150, 250, 50), "Good"))
                {
                    QualitySettings.currentLevel = QualityLevel.Good;
                }
                if (GUI.Button(new Rect((Screen.width / 2) + 150, (Screen.height / 2) + 200, 250, 50), "Beautiful"))
                {
                    QualitySettings.currentLevel = QualityLevel.Beautiful;
                }
                if (GUI.Button(new Rect((Screen.width / 2) + 150, (Screen.height / 2) + 250, 250, 50), "Fantastic"))
                {
                    QualitySettings.currentLevel = QualityLevel.Fantastic;
                }
                if (Input.GetKeyDown("escape"))
                {
                    this.showGraphicsDropDown = false;
                }
            }
            //Make quit game button
            if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) + 50, 250, 50), "Quit Game"))
            {
                Application.Quit();
            }
        }
    }

}