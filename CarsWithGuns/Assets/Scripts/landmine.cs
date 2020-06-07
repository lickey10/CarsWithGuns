using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landmine : MonoBehaviour
{
    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AI" || other.tag == "Player")
        {
            if (Explosion != null)
            {
                GameObject.Instantiate(Explosion, this.transform.position, this.transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
