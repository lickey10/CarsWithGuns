using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Lean : MonoBehaviour
{
    public float leanAmount; //how far does the camera lean?
    public float leanRate; //how fast does the camera move when leaning? 
    public float leanRotate; //how much does the camera rotate when leaning
    private float rotateRate; //how fast do we rotate;
    private float startPos; //standard position of the camera
    private float targetPos; //current target position
    private float targetRot;
    private bool leaning; //are we currently leaning? 
    private bool left;
    private bool colliding;
    public LayerMask mask;
    public float skinWidth;
    public virtual void Awake()
    {
        this.startPos = this.transform.localPosition.x;
        this.leaning = false;
        this.rotateRate = this.leanRate * (this.leanRotate / this.leanAmount);
    }

    public virtual void LateUpdate()
    {
        float maxLean = 0.0f;
        if ((InputDB.GetButton("LeanRight") && PlayerWeapons.CM.grounded) && !CharacterMotorDB.walking)
        {
            if (!this.leaning || this.left)
            {
                this.leaning = true;
                this.targetPos = this.startPos + this.leanAmount;
                this.targetRot = -this.leanRotate;
                this.left = false;
                this.colliding = false;
            }
        }
        else
        {
            if ((InputDB.GetButton("LeanLeft") && PlayerWeapons.CM.grounded) && !CharacterMotorDB.walking)
            {
                if (!this.leaning || !this.left)
                {
                    this.leaning = true;
                    this.targetPos = this.startPos - this.leanAmount;
                    this.targetRot = this.leanRotate;
                    this.left = true;
                    this.colliding = false;
                }
            }
            else
            {
                if (this.leaning)
                {
                    this.colliding = false;
                    this.leaning = false;
                    this.targetPos = this.startPos;
                }
            }
        }
        if (this.left && this.leaning)
        {
            maxLean = this.Check(-1 * this.transform.right);
            this.targetPos = Mathf.Max(this.startPos - this.leanAmount, -maxLean + this.skinWidth);
        }
        else
        {
            if (this.leaning)
            {
                maxLean = this.Check(this.transform.right);
                this.targetPos = Mathf.Min(this.startPos + this.leanAmount, maxLean - this.skinWidth);
            }
        }
        //lerp into the lean
        this.transform.localPosition.x = Mathf.Lerp(this.transform.localPosition.x, this.targetPos, (Time.deltaTime * this.leanRate) * 4);
        this.transform.localEulerAngles.z = Mathf.LerpAngle(0, this.targetRot, Mathf.Abs(this.transform.localPosition.x) / this.leanAmount);
        //transform.localEulerAngles.z = Mathf.Clamp(transform.localEulerAngles.z,-leanRotate,leanRotate);
        //clamp our position if necessary
        if (this.colliding)
        {
            this.transform.localPosition.x = Mathf.Clamp(this.transform.localPosition.x, -maxLean, maxLean);
        }
    }

    public virtual float Check(Vector3 dir)
    {
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(this.transform.parent.position, dir, out hit, this.leanAmount, this.mask))
        {
            this.colliding = true;
            return hit.distance;
        }
        else
        {
            this.colliding = false;
            return this.leanAmount + this.skinWidth;
        }
    }

    public Lean()
    {
        this.mask = ~((1 << (PlayerWeapons.playerLayer + 1)) << PlayerWeapons.ignorePlayerLayer);
        this.skinWidth = 0.2f;
    }

}