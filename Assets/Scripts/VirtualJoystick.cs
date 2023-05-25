using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Transform joystickBackground; // Reference to the joystick background
    public Transform joystickHandle; // Reference to the joystick handle
    public GameManager gameManager; // Reference to the GameManager script

    private bool isJoystickActive = false; // Flag to track if the joystick is active

    private float joystickRadius; // Radius of the joystick background

    private GameObject cueStick; // Reference to the instantiated Cue Stick
    public Transform cueBall; // Reference to the Cue Ball

    public float rotationSpeed = 10f; // Adjust the rotation speed as desired

    private Quaternion initialRotation; // Store the initial rotation of the cue stick

    public bool revolveWithFrontSide = true; // Controls whether the cue stick revolves with its front side towards the cue ball

    private void Start()
    {
        joystickRadius = joystickBackground.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        cueStick = GameObject.FindGameObjectWithTag("CueStick");
        gameManager = FindObjectOfType<GameManager>();

        initialRotation = cueStick.transform.rotation;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isJoystickActive = true;
    }

     public void OnDrag(PointerEventData eventData)
     {
         if (isJoystickActive)
         {
             Vector3 direction = ((Vector3)eventData.position - joystickBackground.position).normalized * joystickRadius;

             joystickHandle.position = joystickBackground.position + direction;

             // Calculate the normalized direction vector of the joystick handle
             Vector3 stickDirection = new Vector3(direction.x, 17.7f, direction.y).normalized;



             // Calculate the target position for the CueStick based on the direction and distance from the joystick handle to the joystick background
             Vector3 targetPosition = cueBall.position + stickDirection * gameManager.stickDistance ;

             // Calculate the direction from the cue stick to the cue ball
             Vector3 cueToBallDirection = cueBall.position - cueStick.transform.position;
             cueToBallDirection.y = 0f; // Ignore the vertical component
             cueToBallDirection.Normalize();

             // Calculate the angle between the cue stick's forward direction and the cue-to-ball direction
             float angle = Vector3.SignedAngle(cueStick.transform.forward, cueToBallDirection, Vector3.up);

             // Rotate the cue stick around the cue ball on the Y-axis with the specified rotation speed
             cueStick.transform.RotateAround(cueBall.position, Vector3.up, angle * rotationSpeed * Time.deltaTime);


             // Calculate the direction from the front side of the cue stick to the cue ball
             Vector3 frontToBallDirection = cueBall.position - cueStick.transform.GetChild(0).position;
             frontToBallDirection.y = 0f; // Ignore the vertical component
             frontToBallDirection.Normalize();

             // Calculate the direction from the back side of the cue stick to the cue ball
             Vector3 backToBallDirection = cueBall.position - cueStick.transform.GetChild(1).position;
             backToBallDirection.y = 0f; // Ignore the vertical component
             backToBallDirection.Normalize();



             if (revolveWithFrontSide)
             {
                 // Calculate the rotation to make the front child object look at the cue ball
                 Quaternion targetRotation = Quaternion.LookRotation(frontToBallDirection, Vector3.up);

                 // Apply the rotation to the front child object
                 cueStick.transform.GetChild(0).rotation = targetRotation;
             }
             else
             {
                 // Calculate the rotation to make the back child object look at the cue ball
                 Quaternion targetRotation = Quaternion.LookRotation(backToBallDirection, Vector3.up);

                 // Apply the rotation to the back child object
                 cueStick.transform.GetChild(1).rotation = targetRotation;
             }

             // Calculate the target position for the front or back child object of the cue stick, considering the distance threshold
             Vector3 newTargetPosition = cueBall.position - (revolveWithFrontSide ? frontToBallDirection : backToBallDirection) * gameManager.stickDistance;

             // Calculate the desired height of the cue stick (adjust the desired height as needed)
             float desiredHeight = cueBall.position.y + 1f; // Adjust the desired height as desired

             // Move the front or back child object towards the target position while maintaining the desired height
             Vector3 moveDirection = (newTargetPosition - (revolveWithFrontSide ? cueStick.transform.GetChild(0).position : cueStick.transform.GetChild(1).position)).normalized;
             Vector3 newPosition = (revolveWithFrontSide ? cueStick.transform.GetChild(0).position : cueStick.transform.GetChild(1).position) + moveDirection * gameManager.stickDistance;
             newPosition.y = desiredHeight;
             gameManager.MoveCueStick(newPosition);
         }
     } 


  /*  public void OnDrag(PointerEventData eventData)
    {
        if (isJoystickActive)
        {
            Vector3 direction = ((Vector3)eventData.position - joystickBackground.position).normalized * joystickRadius;
            joystickHandle.position = joystickBackground.position + direction;

            Vector3 stickDirection = new Vector3(direction.x, 17.7f, direction.y).normalized;

            Vector3 targetPosition = cueBall.position + stickDirection * gameManager.stickDistance;

            Vector3 cueToBallDirection = cueBall.position - cueStick.transform.position;
            cueToBallDirection.y = 0f;
            cueToBallDirection.Normalize();

            float angle = Vector3.SignedAngle(cueStick.transform.forward, cueToBallDirection, Vector3.up);

            cueStick.transform.RotateAround(cueBall.position, Vector3.up, angle * rotationSpeed * Time.deltaTime);
            cueStick.transform.position -= new Vector3(0f, 0.5f, 0f);

            Vector3 frontToBallDirection = cueBall.position - cueStick.transform.GetChild(0).position;
            frontToBallDirection.y = 0f;
            frontToBallDirection.Normalize();

            Vector3 backToBallDirection = cueBall.position - cueStick.transform.GetChild(1).position;
            backToBallDirection.y = 0f;
            backToBallDirection.Normalize();

            Quaternion targetRotation;

            if (revolveWithFrontSide)
            {
                targetRotation = Quaternion.LookRotation(frontToBallDirection, Vector3.up);
                cueStick.transform.GetChild(0).rotation = targetRotation;
            }
            else
            {
                targetRotation = Quaternion.LookRotation(backToBallDirection, Vector3.up);
                cueStick.transform.GetChild(1).rotation = targetRotation;
            }

            Vector3 newTargetPosition = cueBall.position - (revolveWithFrontSide ? frontToBallDirection : backToBallDirection) * gameManager.stickDistance;

            float desiredHeight = cueBall.position.y + 1f;

            Vector3 moveDirection = (newTargetPosition - (revolveWithFrontSide ? cueStick.transform.GetChild(0).position : cueStick.transform.GetChild(1).position)).normalized;
            Vector3 newPosition = (revolveWithFrontSide ? cueStick.transform.GetChild(0).position : cueStick.transform.GetChild(1).position) + moveDirection * gameManager.stickDistance;
            newPosition.y = desiredHeight;
            gameManager.MoveCueStick(newPosition);
        }
    }
  */


    public void OnPointerUp(PointerEventData eventData)
    {
        isJoystickActive = false;
        joystickHandle.position = joystickBackground.position;

        // Store the current position and rotation of the cue stick
        Vector3 stickPosition = cueStick.transform.position;
        Quaternion stickRotation = cueStick.transform.rotation;

        // Reset the cue stick rotation to its initial rotation
        cueStick.transform.rotation = initialRotation;

        // Move the cue stick back to its stored position
        cueStick.transform.position = stickPosition;

        // Rotate the cue stick to its stored rotation
        cueStick.transform.rotation = stickRotation;
    }


}
