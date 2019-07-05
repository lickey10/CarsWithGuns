// ************************************************************************ 
// File Name:   ScreenManager.cs 
// Purpose:    	Transfers between scenes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;


// ************************************************************************ 
// Class: LoadingSceneManager
// ************************************************************************
public class LoadingSceneManager : Singleton<LoadingSceneManager> 
{
    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        throw new NotImplementedException();
    }

    // ********************************************************************
    // Function:	UnloadLoadingScene()
    // Purpose:		Destroys the loading scene
    // ********************************************************************
    public static void UnloadLoadingScene()
	{
		GameObject.Destroy(instance.gameObject);
	}
}
