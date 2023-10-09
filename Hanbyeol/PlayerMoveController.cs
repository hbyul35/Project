using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public float moveSpeed = 5f;             // Movement speed of the player.
    public float maxMoveRange = 4.0f;        // Maximum allowed movement range for the player.
    public float rotationSpeed = 2.0f;       // Rotation speed of the player.
    public bool isRotating = true;           // Flag to control player rotation.

    private Quaternion targetRotation = Quaternion.identity; // The target rotation for the player.

    void Start()
    {
        // Initialization code can be placed here.
    }

    void Update()
    {
        // Update the player's movement and rotation.
        PlayerMovement();
        PlayerRotation();
    }

    void PlayerMovement()
    {
        // Get input for horizontal and vertical movement.
        float horizontalInput = Input.GetAxis("Horizontal"); // Horizontal movement input (-1 to 1).
        float verticalInput = Input.GetAxis("Vertical");     // Vertical movement input (-1 to 1).

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;

        // Limit movement within the defined range.
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -maxMoveRange, maxMoveRange),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -maxMoveRange, maxMoveRange));

        // Apply movement.
        transform.Translate(movement);
    }

    void PlayerRotation()
    {
        Quaternion currentRotation = transform.rotation; // Current rotation of the player.
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (isRotating)
        {
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = currentRotation;

            // Check if rotation is complete and set the flag to false.
            if (Quaternion.Angle(currentRotation, targetRotation) < 0.05f)
            {
                isRotating = false;
            }
        }
        else
        {
            // If not rotating, check for rotation input (E and Q keys).
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Rotate the player around the Y-axis (yaw) to the left.
                targetRotation *= Quaternion.Euler(0, -90.0f, 0);
                isRotating = true; // Set the rotation flag to true to indicate rotation is in progress.
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                // Rotate the player around the Y-axis (yaw) to the right.
                targetRotation *= Quaternion.Euler(0, 90.0f, 0);
                isRotating = true;
            }
        }
    }

    // Additional rotation methods for testing (commented out).
    //...

    //------------------ Work in progress ------------------
    void ClimbPlayer()
    {
        // Raycasting and climbing logic can be added here.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Additional code for climbing can be implemented here.
        //...
    }
}