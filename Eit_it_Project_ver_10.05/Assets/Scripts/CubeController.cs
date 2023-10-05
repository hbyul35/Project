using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float raycastDistance = 0.3f;
    public float pushPullSpeed = 2.0f;

    private GameObject targetCube;

    public bool isMoving = false;

    //PlayerMoveController에서 가져옴
    private bool isRotating;
    private bool holdMode;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);

            if (hit.collider.CompareTag("Cube"))
            {
                targetCube = hit.collider.gameObject;
                isRotating = transform.parent.GetComponent<PlayerMoveController>().isRotating; //PlayerMoveController::isRotating 회전 안할때
                holdMode = transform.parent.GetComponent<PlayerMoveController>().isHoldMode; // PlayerMoveController::holdmode 껏을때

                if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) && !isMoving && !isRotating && !holdMode)
                {
                    StartCoroutine(pushingCube(targetCube));
                }
                else if (Input.GetKeyDown(KeyCode.Space) && !isMoving &&!isRotating && !holdMode)
                {
                    StartCoroutine(pullingCube(targetCube));
                }
            }
        }
    }

    IEnumerator pullingCube(GameObject cubeToMove)
    {
        isMoving = true;

        Vector3 start = cubeToMove.transform.position;
        Vector3 end = start - transform.forward * cubeToMove.transform.localScale.z;
        float journeyLength = Vector3.Distance(start, end); // public static float Distance(Vector3 a, Vector3 b);

        float startTime = Time.time;
        float journeyDuration = journeyLength / pushPullSpeed;


        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * pushPullSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cubeToMove.transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        //�̵��Ϸ� ��
        cubeToMove.transform.position = end;
        isMoving = false;
    }

    IEnumerator pushingCube(GameObject cubeToMove)
    {
        isMoving = true; // ���� �÷���

        Vector3 start = cubeToMove.transform.position;
        Vector3 end = start + transform.forward * cubeToMove.transform.localScale.z;
        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / pushPullSpeed;


        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * pushPullSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cubeToMove.transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        //�̵��Ϸ� ��
        cubeToMove.transform.position = end;
        isMoving = false;
    }
}
