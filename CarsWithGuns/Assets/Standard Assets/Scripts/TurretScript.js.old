//Turret Script
#pragma strict
 
var projectile : GameObject;
 
var myTransform : Transform;
var target : Transform;
var turretControl : Transform;
var muzzlePositions : Transform[];
 
var placed : boolean = false;
 
var reloadTime : float = 1;
var turnSpeed : float = 10;
var firePauseTime : float = 0.25;
var errorAmount : float = 1;
var damage : float;
public var speed : float = 3;
private var nextFireTime : float;
private var nextMoveTime : float;
private var aimError : float;
 
private var desiredRotation : Quaternion;
private var players : GameObject[];
private var targetPos : Vector3;
private var targetDistance : float = 0;

public var FiringRange : int = 500;
 
function Start ()
{
    myTransform = transform;
}
 
function Update ()
{
    players = GameObject.FindGameObjectsWithTag("Player");

    for(var player : GameObject in players)
    {
        var dist = Vector3.Distance(player.transform.position, transform.position);
		print ("Distance to other: " + dist);

        if(dist < FiringRange)// && (targetDistance == 0 || dist < targetDistance))
        {
            target = player.transform;
            targetDistance = dist;
        }
    }

    if (target)
    {
        if (Time.time >= nextMoveTime)
        {
            CalculateAimPosition(target.position);
            //turretControl.rotation = Quaternion.Lerp(turretControl.rotation, desiredRotation, Time.deltaTime * turnSpeed);
            turretControl.transform.LookAt(target);
        }
       
        if (Time.time >= nextFireTime)
        {
            FireProjectile();
        }
    }
   
    if (placed == false)
    {
        var distance = transform.position.z - Camera.main.transform.position.z;
        targetPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
    }
}
 
function OnTriggerEnter (other : Collider)
    {
        if (other.gameObject.tag == "Player")
        {
            nextFireTime = Time.time + (reloadTime * 0.5);
            target = other.gameObject.transform;
        }
    }
 
    function OnTriggerExit (other : Collider)
        {
            if (other.gameObject.transform == target)
            {
                target = null;
            }
        }
 
        function CalculateAimPosition(targetPos : Vector3)
            {
                var aimPoint = Vector3 (targetPos.x + aimError, target.position.y + aimError, target.position.z + aimError);
                desiredRotation = Quaternion.LookRotation(aimPoint);
            }
 
            function CalculateAimError()
            {
                aimError = Random.Range(-errorAmount, errorAmount);
            }
 
            function FireProjectile()
            {
                //GetComponent.<AudioSource>().Play();
                nextFireTime = Time.time + reloadTime;
                nextMoveTime = Time.time + firePauseTime;
                CalculateAimError();
   
                for(theMuzzlePos in muzzlePositions)
                {
                    var newProjectile = Instantiate (projectile, theMuzzlePos.position, theMuzzlePos.rotation);
                    newProjectile.GetComponent.<Rigidbody>().velocity = theMuzzlePos.transform.TransformDirection (Vector3.forward * 50);

                    //var projectileScript : Projectile_BallTurret;
       
                    //projectileScript = newProjectile.GetComponent(Projectile_BallTurret);
                    //projectileScript.damage = damage;
                }
            }