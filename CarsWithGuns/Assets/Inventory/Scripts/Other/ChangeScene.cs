using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("Inventory/Other/ChangeScene")]
public partial class ChangeScene : MonoBehaviour
{
    public string LevelName;
    public string PlayerName;
    public virtual void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(GameObject.Find(this.PlayerName));
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Application.LoadLevel("Level02");
        }
    }

    public ChangeScene()
    {
        this.LevelName = "Level02";
        this.PlayerName = "Player";
    }

}