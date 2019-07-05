using UnityEngine;
using System.Collections;
using Scripts;

public class ResetCar : MonoBehaviour {
    public GameObject Player;
    public GameObject ResetLocation;//the object where you want the player to be reset to

	// Use this for initialization
	void Start () {
        //if (ResetLocation == null)
        //    ResetLocation = GameObject.FindGameObjectWithTag("ResetLocation");

        if (ResetLocation != null)
        {
            var spawnPoint = ResetLocation.transform.position;
            Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Player.transform.position = spawnPoint;
            Player.transform.rotation = ResetLocation.transform.rotation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //find the road
        if (Input.GetKeyDown(KeyCode.R)) //reset the car
        {
            ResetTheCar();
        }
    }

    public void ResetTheCar()
    {
        Mesh mesh = ResetLocation.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        GameObject scripts = GameObject.FindWithTag("Scripts");
        ScreenManager gameManager = scripts.GetComponent<ScreenManager>();
        GameObject lastWayPoint = gameManager.LastWaypoint;

        var spawnPoint = ResetLocation.transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (lastWayPoint != null)
        {
            Player.transform.position = lastWayPoint.transform.position;
            Player.transform.rotation = lastWayPoint.transform.rotation;
        }
    }
}
