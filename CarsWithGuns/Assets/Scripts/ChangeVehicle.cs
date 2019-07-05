using UnityEngine;
using System.Collections;

public class ChangeVehicle : MonoBehaviour {

    GameObject currentVehiclePrefab;
    public GameObject Vehicle;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeCurrentVehicle(GameObject NewVehiclePrefab)
    {
        PlayerWeapons pw = GameObject.FindWithTag("WeaponCamera").GetComponent<PlayerWeapons>();
        Destroy(pw);

        currentVehiclePrefab = GameObject.FindGameObjectWithTag("Car GO");
        Vector3 carPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
        Quaternion carRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
        Destroy(currentVehiclePrefab);
        
        
        Instantiate(NewVehiclePrefab, carPosition, carRotation);

        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
        GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = false;
        GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = false;
        
        Time.timeScale = 1;

        Destroy(GameObject.FindGameObjectWithTag("ScrollingMenu"));
    }

    void OnClick()
    {
        if(enabled)
            ChangeCurrentVehicle(Vehicle);
    }
}
