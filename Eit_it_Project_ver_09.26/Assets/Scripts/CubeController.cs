using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour {
    public float raycastDistance = 0.3f;
    public float pushPullSpeed = 2.0f;

    private GameObject targetCube;

    private bool isMoving = false;
    private bool isRotating;

    void Start() {}

    void Update() {
        isRotating =
            transform.parent.GetComponent<PlayerMoveController>().isRotating;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance)) {
            Debug.DrawRay(transform.position, transform.forward * raycastDistance,
                          Color.red);

            if (hit.collider.CompareTag("Cube")) {
                targetCube = hit.collider.gameObject;

                if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
                        !isMoving && !isRotating) {
                    StartCoroutine(pushingCube(targetCube));
                } else if (Input.GetKeyDown(KeyCode.Space) && !isMoving &&
                           !isRotating) {
                    StartCoroutine(pullingCube(targetCube));
                }
            }
        }
    }

    IEnumerator pullingCube(GameObject cubeToMove) {
        isMoving = true;

        Vector3 start = cubeToMove.transform.position;
        Vector3 end = start - transform.forward * cubeToMove.transform.localScale.z;
        float journeyLength = Vector3.Distance(
                                  start, end); // public static float Distance(Vector3 a, Vector3 b);

        float startTime = Time.time;
        float journeyDuration = journeyLength / pushPullSpeed;

        while (Time.time - startTime < journeyDuration) {
            float distanceCovered = (Time.time - startTime) * pushPullSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cubeToMove.transform.position =
                Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        cubeToMove.transform.position = end;
        isMoving = false;
    }

    IEnumerator pushingCube(GameObject cubeToMove) {
        isMoving = true;

        Vector3 start = cubeToMove.transform.position;
        Vector3 end = start + transform.forward * cubeToMove.transform.localScale.z;
        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / pushPullSpeed;

        while (Time.time - startTime < journeyDuration) {
            float distanceCovered = (Time.time - startTime) * pushPullSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cubeToMove.transform.position =
                Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        cubeToMove.transform.position = end;
        isMoving = false;
    }
}
