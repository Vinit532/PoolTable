using System;
using UnityEngine;

public class BallReactionController : MonoBehaviour
{
    public float hitForce = 1f; // The force to apply when hit by the cue stick
    public float collisionForceMultiplier = 0.5f; // Multiplier for the collision force between balls

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CueStick"))
        {
            // Calculate the direction and force based on the cue stick's hitting direction
            Vector3 forceDirection = transform.position - collision.gameObject.transform.position;
            forceDirection.Normalize();
            Vector3 force = forceDirection * hitForce;

            // Apply the force to the ball
            rb.AddForce(force, ForceMode.Impulse);
        }
        else if (collision.gameObject.CompareTag("Ball"))
        {
            // Calculate the collision force based on the relative velocity
            float collisionForce = collision.relativeVelocity.magnitude * collisionForceMultiplier;

            // Apply the collision force to both colliding balls
            rb.AddForce(collisionForce * collision.relativeVelocity.normalized, ForceMode.Impulse);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-collisionForce * collision.relativeVelocity.normalized, ForceMode.Impulse);
        }
    }
}
