using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class KEY : MonoBehaviour
{
    public GameObject TheKey;
    private bool playerNextToKey;
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (this.playerNextToKey == true))
        {
            this.TheKey.active = false;
        }
    }

    public virtual void OnTriggerEnter(Collider theCollider)
    {
        if (theCollider.tag == "Player")
        {
            this.playerNextToKey = true;
        }
    }

    public virtual void OnTriggerExit(Collider theCollider)
    {
        if (theCollider.tag == "Player")
        {
            this.playerNextToKey = false;
        }
    }

}