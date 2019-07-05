using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TurretScript : MonoBehaviour
{
    //Turret Script
    public GameObject projectile;
    public Transform myTransform;
    public Transform target;
    public Transform turretControl;
    public Transform[] muzzlePositions;
    public bool placed;
    public float reloadTime;
    public float turnSpeed;
    public float firePauseTime;
    public float errorAmount;
    public float damage;
    public float speed;
    private float nextFireTime;
    private float nextMoveTime;
    private float aimError;
    private Quaternion desiredRotation;
    private GameObject[] players;
    private Vector3 targetPos;
    private float targetDistance;
    public int FiringRange;
    public virtual void Start()
    {
        this.myTransform = this.transform;
    }

    public virtual void Update()
    {
        this.players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in this.players)
        {
            float dist = Vector3.Distance(player.transform.position, this.transform.position);
            MonoBehaviour.print("Distance to other: " + dist);
            if (dist < this.FiringRange)// && (targetDistance == 0 || dist < targetDistance))
            {
                this.target = player.transform;
                this.targetDistance = dist;
            }
        }
        if (this.target)
        {
            if (Time.time >= this.nextMoveTime)
            {
                this.CalculateAimPosition(this.target.position);
                //turretControl.rotation = Quaternion.Lerp(turretControl.rotation, desiredRotation, Time.deltaTime * turnSpeed);
                this.turretControl.transform.LookAt(this.target);
            }
            if (Time.time >= this.nextFireTime)
            {
                this.FireProjectile();
            }
        }
        if (this.placed == false)
        {
            float distance = this.transform.position.z - Camera.main.transform.position.z;
            this.targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            this.targetPos = Camera.main.ScreenToWorldPoint(this.targetPos);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.nextFireTime = Time.time + (this.reloadTime * 0.5f);
            this.target = other.gameObject.transform;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == this.target)
        {
            this.target = null;
        }
    }

    public virtual void CalculateAimPosition(Vector3 targetPos)
    {
        Vector3 aimPoint = new Vector3(targetPos.x + this.aimError, this.target.position.y + this.aimError, this.target.position.z + this.aimError);
        this.desiredRotation = Quaternion.LookRotation(aimPoint);
    }

    public virtual void CalculateAimError()
    {
        this.aimError = Random.Range(-this.errorAmount, this.errorAmount);
    }

    public virtual void FireProjectile()//projectileScript.damage = damage;
    {
         //GetComponent.<AudioSource>().Play();
        this.nextFireTime = Time.time + this.reloadTime;
        this.nextMoveTime = Time.time + this.firePauseTime;
        this.CalculateAimError();
        foreach (Transform theMuzzlePos in this.muzzlePositions)
        {
            GameObject newProjectile = UnityEngine.Object.Instantiate(this.projectile, theMuzzlePos.position, theMuzzlePos.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = theMuzzlePos.transform.TransformDirection(Vector3.forward * 50);
        }
    }

    public TurretScript()
    {
        this.reloadTime = 1;
        this.turnSpeed = 10;
        this.firePauseTime = 0.25f;
        this.errorAmount = 1;
        this.speed = 3;
        this.FiringRange = 500;
    }

}