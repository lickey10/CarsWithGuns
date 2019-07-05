using UnityEngine;
using System.Collections;
using Scripts;

public class PickupScript : MonoBehaviour
{
    //public AudioClip CoinSound = null;
    //public AudioClip GoalReachedSound = null;
    //public int CoinValue = 10;
    private AudioSource mAudioSource = null;


    // Use this for initialization
    void Start()
    {
        //mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("pickupAble"))
        {
            Destroy(other.gameObject);

            PickupWeapon.Pickup(other.transform.root.gameObject);
        }
        else if (other.gameObject.tag.Equals("Goal"))
        {
            //if (mAudioSource != null && GoalReachedSound != null)
            //{
            //    mAudioSource.PlayOneShot(GoalReachedSound);
            //}

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Pickupable"))
        {
            Destroy(other.gameObject);

            PickupWeapon.Pickup(other.transform.root.gameObject);
        }
        else if (other.gameObject.tag.Equals("Goal"))
        {
            //if (mAudioSource != null && GoalReachedSound != null)
            //{
            //    mAudioSource.PlayOneShot(GoalReachedSound);
            //}

        }
    }
}
