using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerControls : MonoBehaviour
{
    public float turnSpeed;
    public float maxTurnLean;
    public float maxTilt;
    public float sensitivity;
    public float forwardForce;
    public Transform guiSpeedElement;
    private float normalizedSpeed;
    private Vector3 euler;
    public bool horizontalOrientation;
    public virtual void Awake()
    {
        if (this.horizontalOrientation)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        if (this.guiSpeedElement != null)
        {
            this.guiSpeedElement.position = new Vector3(0, this.normalizedSpeed, 0);
        }
    }

    public virtual void Update()
    {
        foreach (Touch evt in Input.touches)
        {
            if (evt.phase == TouchPhase.Moved)
            {
                this.normalizedSpeed = evt.position.y / Screen.height;
                if (this.guiSpeedElement != null)
                {
                    this.guiSpeedElement.position = new Vector3(0, this.normalizedSpeed, 0);
                }
            }
        }
    }

    public virtual void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(0, 0, this.normalizedSpeed * this.forwardForce);
        Vector3 accelerator = Input.acceleration;
        // Rotate turn based on acceleration		
        this.euler.y = this.euler.y + (accelerator.x * this.turnSpeed);
        // Since we set absolute lean position, do some extra smoothing on it
        this.euler.z = Mathf.Lerp(this.euler.z, -accelerator.x * this.maxTurnLean, 0.2f);
        // Since we set absolute lean position, do some extra smoothing on it
        this.euler.x = Mathf.Lerp(this.euler.x, accelerator.y * this.maxTilt, 0.2f);
        // Apply rotation and apply some smoothing
        Quaternion rot = Quaternion.Euler(this.euler);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rot, this.sensitivity);
    }

    public PlayerControls()
    {
        this.turnSpeed = 10f;
        this.maxTurnLean = 50f;
        this.maxTilt = 50f;
        this.sensitivity = 10f;
        this.forwardForce = 1f;
        this.normalizedSpeed = 0.2f;
        this.euler = Vector3.zero;
        this.horizontalOrientation = true;
    }

}