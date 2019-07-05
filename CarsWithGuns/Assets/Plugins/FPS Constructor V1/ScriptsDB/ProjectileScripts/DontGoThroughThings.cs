using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class DontGoThroughThings : MonoBehaviour
{
    //From the awesome unifycommunity wiki - http://www.unifycommunity.com/wiki/index.php?title=DontGoThroughThings
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth; //probably doesn't need to be changed 
    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    //initialize values 
    public virtual void Awake()
    {
        this.myRigidbody = this.GetComponent<Rigidbody>();
        this.previousPosition = this.myRigidbody.position;
        this.minimumExtent = Mathf.Min(Mathf.Min(this.GetComponent<Collider>().bounds.extents.x, this.GetComponent<Collider>().bounds.extents.y), this.GetComponent<Collider>().bounds.extents.z);
        this.partialExtent = this.minimumExtent * (1f - this.skinWidth);
        this.sqrMinimumExtent = this.minimumExtent * this.minimumExtent;
    }

    public virtual void FixedUpdate()
    {
        RaycastHit hitInfo = default(RaycastHit);
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = this.myRigidbody.position - this.previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
        if (movementSqrMagnitude > this.sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            //check for obstructions we might have missed 
            if (Physics.Raycast(this.previousPosition, movementThisStep, out hitInfo, movementMagnitude, this.layerMask.value))
            {
                this.myRigidbody.position = hitInfo.point - ((movementThisStep / movementMagnitude) * this.partialExtent);
            }
        }
        this.previousPosition = this.myRigidbody.position;
    }

    public DontGoThroughThings()
    {
        this.layerMask = ~(1 << PlayerWeapons.playerLayer);
        this.skinWidth = 0.1f;
    }

}