using UnityEngine.EventSystems;
using UnityEngine;

namespace CnControls
{
    public class SensitiveJoystick : SimpleJoystick, IPointerUpHandler
    {
        GameObject player;
        private CharacterController _characterController;
        private Transform _transform;
        private Transform _mainCameraTransform;

        public AnimationCurve SensitivityCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 1f, 1f),
            new Keyframe(1f, 1f, 1f, 1f));

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            _characterController = player.GetComponent<CharacterController>();
            _transform = player.GetComponent<Transform>();
            _mainCameraTransform = Camera.main.GetComponent<Transform>();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            var linearHorizontalValue = HorizintalAxis.Value;
            var linearVecticalValue = VerticalAxis.Value;

            var horizontalSign = Mathf.Sign(linearHorizontalValue);
            var verticalSign = Mathf.Sign(linearVecticalValue);

            HorizintalAxis.Value = horizontalSign * SensitivityCurve.Evaluate(horizontalSign * linearHorizontalValue);
            VerticalAxis.Value = verticalSign * SensitivityCurve.Evaluate(verticalSign * linearVecticalValue);

            //player.GetComponent<MouseLookDBJS>().Freeze();
        }

        public void OnMouseUp()
        {
            print("hi");
        }

        public void Update()
        {
            // Just use CnInputManager. instead of Input. and you're good to go
            var inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));
            Vector3 movementVector = Vector3.zero;

            // If we have some input
            if (inputVector.sqrMagnitude > 0.001f)
            {
                //movementVector = _mainCameraTransform.TransformDirection(inputVector);
                //movementVector.y = 0f;
                //movementVector.Normalize();
                //_transform.forward = movementVector;
                
                inputVector = _mainCameraTransform.TransformDirection(inputVector);
                //inputVector.y = 0f;
                inputVector.Normalize();
                //_transform.forward = inputVector;
            }
            //else
            //    player.GetComponent<MouseLookMultiFinger>().UnFreeze();

            //movementVector += Physics.gravity;
            //_characterController.Move(movementVector * Time.deltaTime);

            inputVector += Physics.gravity;
            _characterController.Move(inputVector * Time.deltaTime);
        }
    }
}