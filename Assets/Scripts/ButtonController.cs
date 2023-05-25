using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public float moveBackDistance = 1f;
    public float moveAheadDistance = 3f;
    public float hitForce = 5f;

    private Vector3 initialPosition;
    private bool isStickMoving = false;

    private void Start()
    {
        initialPosition = transform.position;

        // Find the button with the name "HitButton" in the scene
        Button hitButton = GameObject.Find("HitButton").GetComponent<Button>();

        // Attach the OnButtonClick method to the onClick event of the button
        hitButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (!isStickMoving)
        {
            // Start the stick movement coroutine
            StartCoroutine(MoveStick());
        }
    }

    private System.Collections.IEnumerator MoveStick()
    {
        isStickMoving = true;

        // Move the stick back
        Vector3 targetPosition = transform.position - transform.up * moveBackDistance;
        float startTime = Time.time;
        while (Time.time - startTime < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (Time.time - startTime) / 1f);
            yield return null;
        }

        // Move the stick forward
        targetPosition = transform.position + transform.up * moveAheadDistance;
        startTime = Time.time;
        float speedMultiplier = 40f; // Adjust this value to change the speed multiplier
        while (Time.time - startTime < (moveAheadDistance / speedMultiplier))
        {
            float t = (Time.time - startTime) / (moveAheadDistance / speedMultiplier);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            yield return null;
        }

        // Hit the ball
        HitBall();

        isStickMoving = false;
    }

    private void HitBall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            if (hit.collider.CompareTag("Ball"))
            {
               
                Rigidbody ballRigidbody = hit.collider.GetComponent<Rigidbody>();
                Vector3 direction = (hit.point - transform.position).normalized;
                ballRigidbody.AddForce(direction * hitForce, ForceMode.Impulse);
            }
        }
    } 
}
