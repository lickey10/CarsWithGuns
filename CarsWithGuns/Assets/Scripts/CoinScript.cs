using UnityEngine;
using System.Collections;
//using Scripts;

public class CoinScript : MonoBehaviour
{
    public AudioClip CoinSound = null;
    public AudioClip GoalReachedSound = null;
    public int CoinValue = 10;
    private AudioSource mAudioSource = null;


    // Use this for initialization
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            Destroy(other.gameObject);

            if (mAudioSource != null && CoinSound != null)
            {
                mAudioSource.PlayOneShot(CoinSound);
            }

            GameObject scripts = GameObject.FindWithTag("ScreenManager");
            Scripts.ScreenManager screenManager = scripts.GetComponent<Scripts.ScreenManager>();
            screenManager.Coins = screenManager.Coins + CoinValue;
            DBStoreController.singleton.balance += CoinValue;

            //add time to timer
            //gamestate.Instance.SetTimer (10);

            //gamestate.Instance.SetCoinsCollected (gamestate.Instance.GetCoinsCollected () + 1);
        }
        else if (other.gameObject.tag.Equals("Goal"))
        {
            if (mAudioSource != null && GoalReachedSound != null)
            {
                mAudioSource.PlayOneShot(GoalReachedSound);
            }

        }
    }
}
