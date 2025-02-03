using UnityEngine;

public class TractorController : MonoBehaviour
{
    public float speed = 10f;          // Forward and backward speed
    public float turnSpeed = 50f;      // Turning speed
    public float brakeForce = 20f;     // Braking force
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleSteering();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical"); // W/S or Up/Down keys
        Vector3 move = transform.forward * moveInput * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    void HandleSteering()
    {
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right keys
        float turn = turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Apply braking
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
        }
    }
}