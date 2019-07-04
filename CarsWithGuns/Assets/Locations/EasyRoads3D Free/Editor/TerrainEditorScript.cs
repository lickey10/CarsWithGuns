using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class TerrainEditorScript : Editor
{
    public virtual void OnSceneGUI()
    {
        if (UnityEngine.Event.current.shift && (RoadObjectScript.OQCQDODCQQ != null))
        {
            Selection.activeGameObject = RoadObjectScript.OQCQDODCQQ.gameObject;
        }
        else
        {
            RoadObjectScript.OQCQDODCQQ = null;
        }
    }

}