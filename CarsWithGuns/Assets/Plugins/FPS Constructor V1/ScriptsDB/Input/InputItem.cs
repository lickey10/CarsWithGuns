using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class InputItem : MonoBehaviour
{
    [UnityEngine.HideInInspector]
    public bool up; //GetButtonUp
    [UnityEngine.HideInInspector]
    public bool down; //GetButtonDown
    [UnityEngine.HideInInspector]
    public bool got; //GetButton
    [UnityEngine.HideInInspector]
    public float axis; //GetAxis
    public string id; //identifier for this input
}