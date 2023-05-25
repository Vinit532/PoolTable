using UnityEngine;

public class StickController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float maxRotationAngle = 30f;

    private bool isRotating = false;
    private float initialMouseX;

    void Update()
    {
        // Check for user input to start rotating
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            initialMouseX = Input.mousePosition.x;
        }

        // Check for user input to stop rotating
        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        // Rotate the stick horizontally based on user mouse movement
        if (isRotating)
        {
            float mouseX = Input.mousePosition.x;
            float rotationAmount = (mouseX - initialMouseX) * rotationSpeed * Time.deltaTime;
            rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationAngle, maxRotationAngle);

            Vector3 newRotation = transform.rotation.eulerAngles;
            newRotation.z -= rotationAmount;
            transform.rotation = Quaternion.Euler(newRotation);
        }
    } 
}
