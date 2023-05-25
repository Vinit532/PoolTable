using UnityEngine;

public class ScorePoints : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;

        // Check if the entering object is a ball
        if (enteringObject.name.Contains("Ball"))
        {
            Debug.Log("Ball entered the pocket: " + enteringObject.name);
        }
    }
}
