using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float raycastDistance = 0.3f;
    public float pushPullSpeed = 2.0f;

    public bool isMoving = false;

    private GameObject targetCube;
    //PlayerMoveController에서 가져옴
    private bool isRotating, isHoldMode, isDigging; 

    void Start()
    {

    }

    void Update()
    {
        CubeControll();
    }

    void CubeControll()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Cube") || hit.collider.CompareTag("AvilityBlock"))
            {
                targetCube = hit.collider.gameObject;
                isRotating = transform.parent.GetComponent<PlayerMoveController>().isRotating;
                isHoldMode = transform.parent.GetComponent<PlayerMoveController>().isHoldMode;
                isDigging = transform.parent.GetComponent<PlayerMoveController>().isDigging;

                if (!isMoving && !isRotating && !isHoldMode && isDigging && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space))
                {
                    if (TryPushCubeInDirection(targetCube, targetCube.transform.forward)  ||
                        TryPushCubeInDirection(targetCube, -targetCube.transform.forward) ||
                        TryPushCubeInDirection(targetCube, targetCube.transform.right)    ||
                        TryPushCubeInDirection(targetCube, -targetCube.transform.right))
                    {
                        StartCoroutine(pushingCube(targetCube));
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) &&!isMoving && !isRotating && !isHoldMode && isDigging)
        {
            StartCoroutine(pullingCube(targetCube));
        }

    }
    bool TryPushCubeInDirection(GameObject cube, Vector3 direction)
    {
        Ray cubeRay = new Ray(cube.transform.position, direction);
        RaycastHit cubeHit;

        return Physics.Raycast(cubeRay, out cubeHit, 0.8f) &&
               cubeHit.collider.CompareTag("player") &&
               !Physics.Raycast(new Ray(cube.transform.position, -direction), 0.8f);
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

        cubeToMove.transform.position = end; 
        isMoving = false; 
    }

    IEnumerator pushingCube(GameObject cubeToMove)
    {
        isMoving = true; 

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

        cubeToMove.transform.position = end; 
        isMoving = false; 
    }
}
