using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    // Input values
    private float horizontalInput, verticalInput;
    private bool isBraking;

    // Settings
    [SerializeField] private float motorForce = 1500f;        // Power for the engine
    [SerializeField] private float brakeForce = 3000f;        // Braking power
    [SerializeField] private float maxSteerAngle = 30f;       // Maximum steering angle
    [SerializeField] private float respawnHeightThreshold = -10f; // Respawn height
    [SerializeField] private Vector3 respawnPosition = new Vector3(0, 5, 0); // Respawn position
    [SerializeField] private Quaternion respawnRotation = Quaternion.identity; // Respawn rotation

    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheel meshes
    [SerializeField] private Transform frontLeftWheelMesh, frontRightWheelMesh;
    [SerializeField] private Transform rearLeftWheelMesh, rearRightWheelMesh;

    private Rigidbody carRigidbody;

    private void Start()
    {
        // Cache the Rigidbody component
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody == null)
            Debug.LogError("Rigidbody component is missing on the car!");
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelMeshes();
        CheckRespawn();
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// Capture player inputs for steering, acceleration, and braking.
    /// </summary>
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow keys
        isBraking = Input.GetKey(KeyCode.Space);       // Spacebar for braking
    }

    /// <summary>
    /// Handles motor power and braking forces.
    /// </summary>
    private void HandleMotor()
    {
        // Apply motor torque to rear wheels
        float motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = motorTorque;
        rearRightWheelCollider.motorTorque = motorTorque;

        // Apply braking force to all wheels
        float brakeTorque = isBraking ? brakeForce : 0f;
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    /// <summary>
    /// Handles steering of the front wheels.
    /// </summary>
    private void HandleSteering()
    {
        float steerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    /// <summary>
    /// Updates the positions and rotations of the wheel meshes to match their respective colliders.
    /// </summary>
    private void UpdateWheelMeshes()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelMesh);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelMesh);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelMesh);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelMesh);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelMesh)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);

        wheelMesh.position = position;
        wheelMesh.rotation = rotation;
    }

    /// <summary>
    /// Checks if the car has fallen below a certain height and respawns it if necessary.
    /// </summary>
    private void CheckRespawn()
    {
        if (transform.position.y < respawnHeightThreshold)
        {
            RespawnCar();
        }
    }

    /// <summary>
    /// Respawns the car at the predefined position and rotation.
    /// </summary>
    private void RespawnCar()
    {
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;

        if (carRigidbody != null)
        {
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
        }

        Debug.Log("Car respawned!");
    }
}
