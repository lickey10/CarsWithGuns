using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class SurfaceEditorScript : Editor
{
    public virtual void OnSceneGUI()
    {
        if (UnityEngine.Event.current.control)
        {
            Selection.activeGameObject = this.target.transform.parent.parent.parent.gameObject;
        }
        else
        {
            Selection.activeGameObject = this.target.transform.parent.gameObject;
        }
    }

}