using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyMovement : MonoBehaviour
{
    //#pragma strict
    private Transform target;
    public Transform waypoint;
    private Quaternion targetRotation;
    private Vector3 targetVector;
    private bool move;
    private Vector3 playerLastSeen;
    private bool visitedLastSeen;
    private int targetPriority;
    private Vector3 curTarget;
    private float loseTime;
    public float turnSpeed;
    public float attackRange;
    public float targetBuffer;
    public float desiredSpeed;
    public float forceConstant;
    public float viewAngle;
    public float viewRange;
    public float hearRange;
    public LayerMask blocksVision;
    public bool moves;
    private bool sees;
    private bool hitOverride;
    public static int enemies;
    public virtual void Start()
    {
        this.target = PlayerWeapons.weaponCam.transform;
        ((EnemyDamageReceiver) this.GetComponent(typeof(EnemyDamageReceiver))).isEnemy = true;
    }

    public virtual void Update()
    {
        Vector3 relativePos = default(Vector3);
        if (MouseLookDBJS.freeze)
        {
            return;
        }
        this.sees = this.CanSeeTarget();
        if (this.hitOverride)
        {
            this.playerLastSeen = this.target.position;
            this.visitedLastSeen = false;
        }
        this.hitOverride = false;
        if (this.sees)
        {
            this.curTarget = this.target.position;
        }
        else
        {
            if (!this.visitedLastSeen)
            {
                this.curTarget = this.playerLastSeen;
                this.loseTime = this.loseTime + Time.deltaTime;
                if (this.loseTime > 3)
                {
                    this.visitedLastSeen = true;
                    this.loseTime = 0;
                    this.waypoint = Waypoint.GetClosestWaypoint(this.transform.position);
                }
            }
            else
            {
                if (this.waypoint)
                {
                    this.curTarget = this.waypoint.position;
                }
                else
                {
                    this.curTarget = new Vector3(0, 0, 0);
                }
            }
        }
        relativePos = this.curTarget - this.transform.position;
        this.targetRotation = Quaternion.LookRotation(relativePos);
        this.ToRotation(this.targetRotation.eulerAngles);
        if (this.move && this.moves)
        {
            // this reduces the amount of force that acts on the object if it is already
            // moving at speed.
            float forceMultiplier = Mathf.Clamp01((this.desiredSpeed - this.GetComponent<Rigidbody>().velocity.magnitude) / this.desiredSpeed);
            // now we actually perform the push
            this.GetComponent<Rigidbody>().AddForce(this.transform.forward * ((forceMultiplier * Time.deltaTime) * this.forceConstant));
        }
        if ((Vector3.Distance(this.transform.position, this.target.position) < this.attackRange) && this.sees)
        {
            this.SendMessage("Attack");
        }
        if (Vector3.Distance(this.transform.position, this.curTarget) < this.targetBuffer)
        {
            this.visitedLastSeen = true;
            this.move = false;
        }
        else
        {
            this.move = true;
        }
    }

    public virtual void ToRotation(Vector3 v3)
    {
        float xtarget = this.transform.localEulerAngles.x;
        float ztarget = this.transform.localEulerAngles.z;
        float ytarget = Mathf.MoveTowardsAngle(this.transform.localEulerAngles.y, v3.y, Time.deltaTime * this.turnSpeed);
        this.transform.localEulerAngles = new Vector3(xtarget, ytarget, ztarget);
    }

    public virtual bool CanSeeTarget()
    {
        RaycastHit hit = default(RaycastHit);
        //checks if target is visible, within field of view, or close enough to be heard
        bool canSee = false;
        float targetAngle = Vector3.Angle(this.target.position - this.transform.position, this.transform.forward);
        float targetDistance = Vector3.Distance(this.transform.position, this.target.position);
        //is target within range and angle
        if ((targetDistance < this.viewRange) && (Mathf.Abs(targetAngle) < (this.viewAngle / 2)))
        {
            if (!Physics.Linecast(this.transform.position, this.target.position, this.blocksVision))
            {
                this.playerLastSeen = this.target.position;
                canSee = true;
                this.visitedLastSeen = false;
            }
        }
        //is target close enough to hear?
        if (targetDistance < this.hearRange)
        {
            this.playerLastSeen = this.target.position;
            canSee = true;
            this.visitedLastSeen = false;
        }
        return canSee;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        //Draw field of view
        Quaternion leftRayRotation = Quaternion.AngleAxis(-this.viewAngle / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(this.viewAngle / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * this.transform.forward;
        Vector3 rightRayDirection = rightRayRotation * this.transform.forward;
        Gizmos.DrawRay(this.transform.position, leftRayDirection * this.viewRange);
        Gizmos.DrawRay(this.transform.position, rightRayDirection * this.viewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.hearRange);
    }

    public virtual void ApplyDamagePlayer(float damage)
    {
        this.hitOverride = true;
    }

    public EnemyMovement()
    {
        this.visitedLastSeen = true;
        this.moves = true;
    }

}