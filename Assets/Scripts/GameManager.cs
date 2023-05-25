using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cueStickPrefab; // Prefab for the cue stick
    public Transform cueBall; // Reference to the cue ball
    public float ballStoppedThreshold = 0.1f; // Threshold velocity for determining if a ball has stopped
    public float stickSpeed = 5f; // Speed at which the cue stick moves

    private GameObject cueStick; // Reference to the instantiated cue stick
    private Vector3 spawnPoint; // Spawn point for the cue stick
    public float stickDistance = 1f; // Distance from the front side of the stick to stop
    public float offsetDistance = 1f; // Distance between the front side of the stick and the cue ball

    private Vector3 initialTargetPosition; // Initial target position for the cue stick
    private float initialDesiredHeight; // Initial desired height of the cue stick

    private bool ballsMoving; // Flag to track if any balls are currently moving

    public float rotationAmount = 0.5f;

    void Start()
    {
        spawnPoint = cueBall.position + new Vector3(0f, 0.2f, -1.5f); // Example spawn position, adjust as needed

        // Instantiate the cue stick at the spawn point with the desired rotation
        cueStick = Instantiate(cueStickPrefab, spawnPoint, Quaternion.Euler(93f, -90f, 0f));
    }

    void Update()
    {
        // Check if any balls are currently moving
        ballsMoving = !AllBallsStopped();

        // If no balls are moving, move the cue stick towards the cue ball
        if (!ballsMoving)
        {
            // Calculate the direction from the stick to the cue ball
            Vector3 direction = cueBall.position - cueStick.transform.position;
            direction.Normalize();

            // Calculate the target position for the stick with an offset distance from the cue ball's front side
            Vector3 targetPosition = cueBall.position - direction * (stickDistance + offsetDistance);

            // Move the stick towards the target position
            cueStick.transform.position = Vector3.MoveTowards(cueStick.transform.position, targetPosition, stickSpeed * Time.deltaTime);

            // Store the initial target position and desired height
            initialTargetPosition = targetPosition;
            initialDesiredHeight = targetPosition.y;
        }

       // AdjustCueStickRotationOnXAxis();
    }

    public void MoveCueStick(Vector3 targetPosition)
    {
        // Move the cue stick towards the target position
        cueStick.transform.position = Vector3.MoveTowards(cueStick.transform.position, targetPosition, stickSpeed * Time.deltaTime);
    }

    bool AllBallsStopped()
    {
        // Check if all balls are not moving
        BallController[] balls = FindObjectsOfType<BallController>();
        foreach (BallController ball in balls)
        {
            if (ball.GetComponent<Rigidbody>().velocity.magnitude > ballStoppedThreshold)
                return false;
        }
        return true;
    }

   
    public void MoveStick_Up( )
    {
        // Get the current rotation of the cue stick
        Quaternion currentRotation = cueStick.transform.rotation;

        // Calculate the new rotation by modifying the X component of the Euler angles
        Vector3 newEulerAngles = currentRotation.eulerAngles;
        newEulerAngles.x += rotationAmount;

        // Apply the new rotation to the cue stick
        cueStick.transform.rotation = Quaternion.Euler(newEulerAngles);
    }
    
    
    public void MoveStick_Down( )
    {
        // Get the current rotation of the cue stick
        Quaternion currentRotation = cueStick.transform.rotation;

        // Calculate the new rotation by modifying the X component of the Euler angles
        Vector3 newEulerAngles = currentRotation.eulerAngles;
        newEulerAngles.x -= rotationAmount;

        // Apply the new rotation to the cue stick
        cueStick.transform.rotation = Quaternion.Euler(newEulerAngles);
    }

    
    public void AdjustCueStickRotationOnXAxis()
    {
       

        // Check if the Up Arrow key is pressed
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Get the current rotation of the cue stick
            Quaternion currentRotation = cueStick.transform.rotation;

            // Calculate the new rotation by modifying the X component of the Euler angles
            Vector3 newEulerAngles = currentRotation.eulerAngles;
            newEulerAngles.x += rotationAmount;

            // Apply the new rotation to the cue stick
            cueStick.transform.rotation = Quaternion.Euler(newEulerAngles);
        }

        // Check if the Down Arrow key is pressed
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Get the current rotation of the cue stick
            Quaternion currentRotation = cueStick.transform.rotation;

            // Calculate the new rotation by modifying the X component of the Euler angles
            Vector3 newEulerAngles = currentRotation.eulerAngles;
            newEulerAngles.x -= rotationAmount;

            // Apply the new rotation to the cue stick
            cueStick.transform.rotation = Quaternion.Euler(newEulerAngles);
        }
    }

}
