using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AmmoManager : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    public object[] tempNamesArray;
    public string[] namesArray;
    public int[] clipsArray;
    public int[] maxClipsArray;
    public bool[] infiniteArray;
    public AmmoManager()
    {
        this.tempNamesArray = "Ammo Set 1";
        this.namesArray = new string[10];
        this.clipsArray = new int[10];
        this.maxClipsArray = new int[10];
        this.infiniteArray = new bool[10];
    }

}