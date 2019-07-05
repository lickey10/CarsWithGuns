using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{

    public Rigidbody bulletPrefab;
    private Transform target;
    private GameObject bullet;
    private float nextFire;
    private Quaternion targetPos;

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            Debug.Log("in");
            target = otherCollider.transform;
            StartCoroutine("Fire");
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            Debug.Log("out");
            target = null;
            StopCoroutine("Fire"); // aborts the currently running Fire() coroutine
        }
    }

    IEnumerator Fire()
    {
        while (target != null)
        {
            nextFire = Time.time + 0.5f;
            while (Time.time < nextFire)
            {
                // smooth the moving of the turret
                //targetPos = Quaternion.LookRotation(target.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetPos, Time.deltaTime * 5);
                
                transform.LookAt(target);
                yield return new WaitForEndOfFrame();
            }
            // fire!
            Debug.Log("shoot");
            //bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            //bullet.rigidbody.velocity = transform.forward * bulletSpeed;
        }
    }
}