using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoZoom : MonoBehaviour
{
    public GameObject Player;
    public int PlayerMaxDistance = 10;
    public int PlayerMinDistance = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Player.transform.position);


    }
}
