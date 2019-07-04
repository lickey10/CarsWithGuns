using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FadingImplementationHelp : MonoBehaviour
{
    /*
This is an implementation of the script Fade available on UnifyCommunity
http://www.unifycommunity.com/wiki/index.php?title=Fade
*/
    public float fadeTime;//You can tune its value in the inspector view from 0-1
    public float fadeAlpha;//You can tune its value in the inspector view from 0-1
    public GUITexture faderTxtr;
    public virtual void Awake()
    {
         /*I wrote this code to completely fill the faderTexture no matter whatever your screen size is*/
        this.faderTxtr.pixelInset.width = Screen.width;
        this.faderTxtr.pixelInset.height = Screen.height;
        this.faderTxtr.pixelInset.x = -(Screen.width / 2);
        this.faderTxtr.pixelInset.y = -(Screen.height / 2);
    }

     /*For fade out use this funtion*/ /*For fade in use this funtion*/ /*For more info visit the link provided above in this file*/ //Fade.use.Alpha(faderTxtr, 0.0, fadeAlpha, fadeTime, EaseType.In);
    public FadingImplementationHelp()
    {
        this.fadeTime = 0.3f;
        this.fadeAlpha = 0.7f;
    }

}