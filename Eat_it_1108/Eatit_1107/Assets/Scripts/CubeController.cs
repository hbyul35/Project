using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float raycastDistance = 0.3f;
    public float pushPullSpeed = 2.0f;

    public bool isMoving = false;

    private GameObject targetCube;
    //PlayerMoveController에서 가져옴
    private bool isRotating; 
    private bool isHoldMode;

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
        int cubeLayerMask = 1 << LayerMask.NameToLayer("Cube");
        
        if (Physics.Raycast(ray, out hit, raycastDistance, cubeLayerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);

            //if (hit.collider.CompareTag("Cube") || hit.collider.CompareTag("AbilityCube") || hit.collider.CompareTag("TrapBlock") || hit.collider.CompareTag("DisposableBlock"))
            targetCube = hit.collider.gameObject;
            isRotating = transform.parent.GetComponent<PlayerMoveController>().isRotating; //PlayerMoveController::isRotating 회전 안할때
            isHoldMode = transform.parent.GetComponent<PlayerMoveController>().isHoldMode; // PlayerMoveController::holdmode 껏을때

            Ray f_targetCubeRay = new Ray(targetCube.transform.position, targetCube.transform.forward);
            Ray b_targetCubeRay = new Ray(targetCube.transform.position, -targetCube.transform.forward);
            Ray r_targetCubeRay = new Ray(targetCube.transform.position, targetCube.transform.right);
            Ray l_targetCubeRay = new Ray(targetCube.transform.position, -targetCube.transform.right);
            RaycastHit f_targetCubeHit, b_targetCubeHit, r_targetCubeHit, l_targetCubeHit;

            if (!isMoving && !isRotating && !isHoldMode && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
                Physics.Raycast(b_targetCubeRay, out b_targetCubeHit, 0.8f) && b_targetCubeHit.collider.CompareTag("player") &&
                !Physics.Raycast(f_targetCubeRay, out f_targetCubeHit, 0.8f) && f_targetCubeHit.collider == null)
            {
                StartCoroutine(pushingCube(targetCube));
            }
            else if (!isMoving && !isRotating && !isHoldMode && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
                Physics.Raycast(f_targetCubeRay, out f_targetCubeHit, 0.8f) && f_targetCubeHit.collider.CompareTag("player") &&
                !Physics.Raycast(b_targetCubeRay, out b_targetCubeHit, 0.8f) && b_targetCubeHit.collider == null)
            {
                StartCoroutine(pushingCube(targetCube));
            }
            else if (!isMoving && !isRotating && !isHoldMode && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
                Physics.Raycast(r_targetCubeRay, out r_targetCubeHit, 0.8f) && r_targetCubeHit.collider.CompareTag("player") &&
                !Physics.Raycast(l_targetCubeRay, out l_targetCubeHit, 0.8f) && l_targetCubeHit.collider == null)
            {
                StartCoroutine(pushingCube(targetCube));
            }
            else if (!isMoving && !isRotating && !isHoldMode && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) &&
                Physics.Raycast(l_targetCubeRay, out l_targetCubeHit, 0.8f) && l_targetCubeHit.collider.CompareTag("player") &&
                !Physics.Raycast(r_targetCubeRay, out r_targetCubeHit, 0.8f) && r_targetCubeHit.collider == null)
            {
                StartCoroutine(pushingCube(targetCube));
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !isMoving && !isRotating && !isHoldMode)
            {
                StartCoroutine(pullingCube(targetCube));
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
