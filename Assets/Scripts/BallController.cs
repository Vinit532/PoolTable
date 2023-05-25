using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Hit(Vector3 direction, float force)
    {
        // Normalize the direction vector
        direction.Normalize();

        // Apply the force to the ball in the specified direction
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    internal void HitBall(Vector3 forward, float hitForce)
    {
        throw new NotImplementedException();
    }
}
