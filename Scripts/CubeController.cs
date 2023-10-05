using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {
  public float raycastDistance =
      0.3f; // The distance for raycasting to detect objects.
  public float pushPullSpeed =
      2.0f; // The speed at which the cube is pushed or pulled.

  private GameObject targetCube; // The cube currently targeted for movement.

  private bool isMoving =
      false;               // Variable to determine if movement is in progress.
  private bool isRotating; // Variable to determine if rotation is in progress.

  void Start() {
    // Initialization code can be placed here.
  }

  void Update() {
    // Check if the player is rotating (obtained from the parent object).
    isRotating =
        transform.parent.GetComponent<PlayerMoveController>().isRotating;
    Debug.Log("Parent Value: " +
              isRotating); // Log the rotation state for debugging purposes.

    // Define a ray pointing in the forward direction of the cube.
    Ray ray = new Ray(transform.position, transform.forward);
    RaycastHit hit;

    // Check if the ray hits an object within the raycastDistance.
    if (Physics.Raycast(ray, out hit, raycastDistance)) {
      Debug.DrawRay(transform.position, transform.forward * raycastDistance,
                    Color.red);

      if (hit.collider.CompareTag("Cube")) {
        targetCube = hit.collider.gameObject;

        // Check for input to push the cube (W key + Space) and ensure no
        // movement or rotation is in progress.
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
            !isMoving && !isRotating) {
          StartCoroutine(pushingCube(targetCube));
        }
        // Check for input to pull the cube (Space key) and ensure no movement
        // or rotation is in progress.
        else if (Input.GetKeyDown(KeyCode.Space) && !isMoving && !isRotating) {
          StartCoroutine(pullingCube(targetCube));
        }
      }
    }
  }

  IEnumerator pullingCube(GameObject cubeToMove) {
    isMoving = true; // Set the movement flag to indicate the start of movement.

    Vector3 start =
        cubeToMove.transform.position; // Get the current position of the cube.
    Vector3 end =
        start -
        transform.forward *
            cubeToMove.transform.localScale.z; // Calculate the destination.
    float journeyLength =
        Vector3.Distance(start, end); // Calculate the total distance to move.

    float startTime = Time.time; // Record the start time of movement.
    float journeyDuration =
        journeyLength /
        pushPullSpeed; // Calculate the expected duration of movement.

    // While the elapsed time is less than the expected duration, move the cube.
    while (Time.time - startTime < journeyDuration) {
      float distanceCovered = (Time.time - startTime) *
                              pushPullSpeed; // Calculate the distance covered.
      float fractionOfJourney =
          distanceCovered /
          journeyLength; // Calculate the fraction of the journey completed.
      cubeToMove.transform.position = Vector3.Lerp(
          start, end, fractionOfJourney); // Interpolate the position.
      yield return null;                  // Wait for the next frame.
    }

    // After movement is complete, set the cube's position to the exact
    // destination.
    cubeToMove.transform.position = end;
    isMoving = false; // Reset the movement flag.
  }

  IEnumerator pushingCube(GameObject cubeToMove) {
    isMoving = true; // Set the movement flag to indicate the start of movement.

    Vector3 start =
        cubeToMove.transform.position; // Get the current position of the cube.
    Vector3 end =
        start +
        transform.forward *
            cubeToMove.transform.localScale.z; // Calculate the destination.
    float journeyLength =
        Vector3.Distance(start, end); // Calculate the total distance to move.

    float startTime = Time.time; // Record the start time of movement.
    float journeyDuration =
        journeyLength /
        pushPullSpeed; // Calculate the expected duration of movement.

    // While the elapsed time is less than the expected duration, move the cube.
    while (Time.time - startTime < journeyDuration) {
      float distanceCovered = (Time.time - startTime) *
                              pushPullSpeed; // Calculate the distance covered.
      float fractionOfJourney =
          distanceCovered /
          journeyLength; // Calculate the fraction of the journey completed.
      cubeToMove.transform.position = Vector3.Lerp(
          start, end, fractionOfJourney); // Interpolate the position.
      yield return null;                  // Wait for the next frame.
    }

    // After movement is complete, set the cube's position to the exact
    // destination.
    cubeToMove.transform.position = end;
    isMoving = false; // Reset the movement flag.
  }
}
