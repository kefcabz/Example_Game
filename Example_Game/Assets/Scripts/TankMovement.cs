using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed = 5f;   // Forward/back speed
    public float turnSpeed = 100f; // Left/right turn speed

    void Update()
    {
        // Get input (W/S for forward/back, A/D for turning)
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Move the tank forward/back
        transform.Translate(Vector3.forward * move);

        // Rotate the tank left/right
        transform.Rotate(Vector3.up * turn);
    }
}
