using UnityEngine;
using System.Collections;
using Scripts;

public class TreasureChest : MonoBehaviour {

    public Animation OpenAnimation;
    private bool isOpen = false;
    public Transform Treasure;

	// Use this for initialization
	void Start () {
        //open treasure chest
        GetComponent<Animation>().Play("open");

        //pop out the diamond and add  money
        GameObject scripts = GameObject.FindWithTag("Scripts");
        //SceneManager gameManager = scripts.GetComponent<SceneManager>();

        //if (gameManager != null)
        //{
        //    gameManager.Coins = gameManager.Coins + 1000;
        //DBStoreController.singleton.balance += 1000;
        //}

        Instantiate(Treasure, new Vector3(this.transform.position.x+2, this.transform.position.y + 3.5f, this.transform.position.z+2), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isOpen)
        {
            //open treasure chest
            GetComponent<Animation>().Play("open");

            //pop out the diamond and add  money
            GameObject scripts = GameObject.FindWithTag("Scripts");
            //ScreenManager gameManager = scripts.GetComponent<ScreenManager>();
            //gameManager.Coins = gameManager.Coins + 1000;
            //DBStoreController.singleton.balance += 1000;
            Instantiate(Treasure, new Vector3(this.transform.position.x+2, this.transform.position.y + 3.5f,this.transform.position.z +2), Quaternion.identity);

            isOpen = true;
        }
    }
}
