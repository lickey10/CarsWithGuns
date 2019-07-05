using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Avoidance : MonoBehaviour
{
    /*var avoidPos : Vector3;
var avoidRot : Vector3;
private var standardPos : Vector3;
private var standardRot : Vector3;*/
    public Vector3 avoidRotation;
    public Vector3 avoidPosition;
    public float avoidStartDistance;
    public float avoidMaxDistance;
    private Vector3 rot;
    private Vector3 pos;
    private float dist;
    private float minDist;
    public LayerMask layerMask;
    private Vector3 targetRot;
    private Vector3 targetPos;
    public static bool collided;
    public static bool canAim;
    public static Avoidance singleton;
    public bool avoid;
    private bool startAvoid;
    public float stopTime;
    public virtual void Awake()
    {
        Avoidance.singleton = this;
        this.rot = this.avoidRotation;
        this.pos = this.avoidPosition;
        this.dist = this.avoidStartDistance;
        this.minDist = this.avoidMaxDistance;
        this.startAvoid = this.avoid;
    }

    //Sets values to given.
    public static void SetValues(Vector3 rotation, Vector3 position, float startDist, float maxDist, bool avoids)
    {
        if (!Avoidance.singleton)
        {
            return;
        }
        Avoidance.singleton.rot = rotation;
        Avoidance.singleton.pos = position;
        Avoidance.singleton.dist = startDist;
        Avoidance.singleton.minDist = maxDist;
        Avoidance.singleton.avoid = avoids;
    }

    //Reverts to default values
    public static void SetValues()
    {
        if (!Avoidance.singleton)
        {
            return;
        }
        Avoidance.singleton.rot = Avoidance.singleton.avoidRotation;
        Avoidance.singleton.pos = Avoidance.singleton.avoidPosition;
        Avoidance.singleton.dist = Avoidance.singleton.avoidStartDistance;
        Avoidance.singleton.minDist = Avoidance.singleton.avoidMaxDistance;
        Avoidance.singleton.avoid = Avoidance.singleton.startAvoid;
    }

    public virtual void Update()
    {
        RaycastHit hit = default(RaycastHit);
        if (!this.avoid)
        {
            Avoidance.collided = false;
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2) + (Screen.width / 65), Screen.height / 2, 0));
        Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2) - (Screen.width / 65), Screen.height / 2, 0));
        if (((Physics.Raycast(ray, out hit, this.dist, this.layerMask) && !GunScript.reloading) && !GunScript.takingOut) && !GunScript.puttingAway)
        {
            this.Collide(hit);
        }
        else
        {
            if (this.stopTime < 0)
            {
                this.stopTime = Time.time + 0.06f;
            }
        }
        /* else if(Physics.Raycast(ray2, hit, dist, layerMask) && collided && !GunScript.reloading && !GunScript.takingOut && !GunScript.puttingAway){
		Collide(hit);
	} else if(Physics.Raycast(ray3, hit, dist, layerMask) && collided && !GunScript.reloading && !GunScript.takingOut && !GunScript.puttingAway){
		Collide(hit);
	} else {
	*/
        if ((Time.time > this.stopTime) && (this.stopTime > 0))
        {
            this.targetRot = new Vector3(0, 0, 0);
            this.targetPos = new Vector3(0, 0, 0);
            Avoidance.canAim = true;
            if (this.transform.localPosition.magnitude < 0.3f)
            {
                Avoidance.collided = false;
            }
        }
        float rate = Time.deltaTime * 9;
        if (this.transform.localPosition != this.targetPos)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this.targetPos, rate);
        }
        if (this.transform.localEulerAngles != this.targetRot)
        {
            this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, this.targetRot.x, rate);
            this.transform.localEulerAngles.y = Mathf.LerpAngle(this.transform.localEulerAngles.y, this.targetRot.y, rate);
            this.transform.localEulerAngles.z = Mathf.LerpAngle(this.transform.localEulerAngles.z, this.targetRot.z, rate);
        }
    }

    public virtual void Collide(RaycastHit hit)
    {
        float val = 0.0f;
        this.stopTime = -1;
        val = ((this.dist - this.minDist) - (hit.distance - this.minDist)) / (this.dist - this.minDist);
        val = Mathf.Min(val, 1);
        this.targetRot = this.rot * val;
        this.targetPos = this.pos * val;
        Avoidance.collided = true;
        Avoidance.canAim = false;
    }

    public Avoidance()
    {
        this.avoidStartDistance = 4;
        this.avoidMaxDistance = 1.3f;
        this.dist = 2;
        this.minDist = 1.5f;
        this.layerMask = ~((1 << (PlayerWeapons.playerLayer + 1)) << PlayerWeapons.ignorePlayerLayer);
        this.avoid = true;
    }

    static Avoidance()
    {
        Avoidance.canAim = true;
    }

}