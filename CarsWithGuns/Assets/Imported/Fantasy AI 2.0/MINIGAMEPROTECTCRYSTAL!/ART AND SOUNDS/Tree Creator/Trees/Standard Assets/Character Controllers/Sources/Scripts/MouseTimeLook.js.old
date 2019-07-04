#pragma strict

@script AddComponentMenu("Camera-Control/Smooth Mouse Look"); 

enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2};

var axes : RotationAxes  = RotationAxes.MouseXAndY;

var sensitivityX : float = 15;

var sensitivityY : float = 15;

 

var minimumX : float = -360F;

var maximumX : float = 360F;

 

var minimumY : float = -60F;

var maximumY : float = 60F;

 

var smoothTimeX : float = 5;

var smoothTimeY : float = 5;

 

 

var clampX : boolean = false;

var clampY : boolean = true;

@HideInInspector

var rotationX : float;

@HideInInspector

var rotationY : float;

 

 

function Start ()

    {

        Screen.lockCursor = true;

        // Make the rigid body not change rotation

        if (GetComponent.<Rigidbody>())

            GetComponent.<Rigidbody>().freezeRotation = true;

    }

    

function LateUpdate () {

            //transform.localEulerAngles.z = 0;

            

            if (axes == RotationAxes.MouseX)

                rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            else if (axes == RotationAxes.MouseXAndY){

                rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            }

            else if (axes == RotationAxes.MouseY)

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            

            if (clampY) 

                rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            if (clampX)

                rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);

            

            transform.localEulerAngles.y = Mathf.LerpAngle(transform.localEulerAngles.y, rotationX, Time.smoothDeltaTime*smoothTimeX);

            transform.localEulerAngles.x = Mathf.LerpAngle(transform.localEulerAngles.x, -rotationY, Time.smoothDeltaTime*smoothTimeY);

            

            

}