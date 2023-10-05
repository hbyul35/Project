using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public float moveSpeed = 5f, motionSpeed = 2f;
    public float maxMoveRange = 6.0f;
    public float rotationSpeed = 2.0f;
    public bool isRotating = true;
    public bool isHoldMode = false;

    private Rigidbody playerRb;
    private bool isHoldMoving = false;
    private GameObject targetCube;
    private Quaternion targetRotation = Quaternion.identity;

    private CubeController cubeController; // CubeController 정보를 담을 변수
    private HeadCollisionCheck headCollisionCheck; //HeadCollisionCheck 정보를 담을 변수

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHoldMode)
        {
            PlayerMovement();
            PlayerRotation();
        }
        ClimbPlayer();
        HangingMode();
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical"); 

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
     


        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -maxMoveRange, maxMoveRange),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -maxMoveRange, maxMoveRange));

        transform.Translate(movement);

    }

    void PlayerRotation()
    {
        Quaternion currentRotation = transform.rotation; 
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime); 

        if (isRotating)
        {
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = currentRotation;
          
            if (Quaternion.Angle(currentRotation, targetRotation) < 0.05f)
            {
                isRotating = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
               
                targetRotation *= Quaternion.Euler(0, 90.0f, 0);
                isRotating = true; 
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotation *= Quaternion.Euler(0, -90.0f, 0);
                isRotating = true; 
            }
        }
    }


    void ClimbPlayer()
    {
        Ray forwardRay = new Ray(transform.position, transform.forward);
        Ray downwardRay = new Ray(transform.position, -transform.up);
        
        Debug.DrawRay(transform.position, transform.up*1f, Color.green);

        RaycastHit forwarhit, downwarhit;

        if (Physics.Raycast(forwardRay, out forwarhit, 0.3f) && forwarhit.collider.CompareTag("Cube") && Input.GetKeyDown(KeyCode.LeftShift))
        {
            targetCube = forwarhit.collider.gameObject; // 타겟 오브젝트에 충돌 큐브 오브젝트 값 추가

            // 등산 조건 검사 추가 09/30 
            cubeController = GetComponentInChildren<CubeController>();
            bool isMoving = cubeController.isMoving; // 큐브의 이동이 종료됐을때

            headCollisionCheck = GetComponentInChildren<HeadCollisionCheck>(); // 머리 충돌체크용
            bool isHeadColliding = headCollisionCheck.isColliding;

            Ray targetCubeRay = new Ray(targetCube.transform.position, targetCube.transform.up); // 타겟 큐브 위쪽 방량으로 레이생성
            RaycastHit targetHit;
                
            // 1. 올라가려는 큐브 위에 연속된 큐브가 있으면 막음 2. 공중, 떨어지는 도중 등반하기 막음 3. 플레이어의 머리위에 큐브가 있을때 막음
            if (!Physics.Raycast(targetCubeRay, out targetHit, 1.0f) && 
                Physics.Raycast(downwardRay, out downwarhit, 0.6f) &&
                !isMoving && !isHeadColliding)
            {
                StartCoroutine(MovementClimbing(targetCube));
            }
        }
    }

    void HangingMode() // 매달려서 이동 10/05 좌우 방향만 구현
    {
        Ray upwardRay = new Ray(transform.position, -transform.up);
        RaycastHit upwarhit;

        if(Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.S) && Physics.Raycast(upwardRay, out upwarhit, 0.5f) && upwarhit.collider.CompareTag("Cube") && !isHoldMode)
        {
            targetCube = upwarhit.collider.gameObject;
            StartCoroutine(F_HangOnCube(targetCube));
        }
        else if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.W) && Physics.Raycast(upwardRay, out upwarhit, 0.5f) && upwarhit.collider.CompareTag("Cube") && !isHoldMode)
        {
            targetCube = upwarhit.collider.gameObject;
            StartCoroutine(B_HangOnCube(targetCube));
        }
        else if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.A) && Physics.Raycast(upwardRay, out upwarhit, 0.5f) && upwarhit.collider.CompareTag("Cube") && !isHoldMode)
        {
            targetCube = upwarhit.collider.gameObject;
            StartCoroutine(L_HangOnCube(targetCube));
        }
        else if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.D) && Physics.Raycast(upwardRay, out upwarhit, 0.5f) && upwarhit.collider.CompareTag("Cube") && !isHoldMode)
        {
            targetCube = upwarhit.collider.gameObject;
            StartCoroutine(R_HangOnCube(targetCube));
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            isHoldMode = false; 
            playerRb.useGravity = true; 
        }


        Ray forwardRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 0.4f, Color.red);
        RaycastHit forwarhit;

        if (Physics.Raycast(forwardRay, out forwarhit, 0.4f) && forwarhit.collider.CompareTag("Cube"))
        {
            targetCube = forwarhit.collider.gameObject;
            
            Ray f_Ray = new Ray(targetCube.transform.position, targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.forward*1.5f, Color.red); // 앞

            Ray b_Ray = new Ray(targetCube.transform.position, -targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.forward*1.5f, Color.blue); // 뒤

            Ray r_Ray = new Ray(targetCube.transform.position, targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.right*1.5f, Color.yellow); //오른쪽

            Ray l_Ray = new Ray(targetCube.transform.position, -targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.right*1.5f, Color.magenta); //왼쪽

            Ray u_Ray = new Ray(targetCube.transform.position, targetCube.transform.up);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.up*1.5f, Color.white); //위쪽

            Ray d_Ray = new Ray(targetCube.transform.position, -targetCube.transform.up);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.up*1.5f, Color.black); //아래쪽
            RaycastHit f_Hit, b_Hit, l_Hit, r_Hit, u_Hit, d_Hit;

            //Debug.Log("test0"); // 여기까지는 통과

            //---------Left/Right Movement---------
            //forward
            if (Physics.Raycast(f_Ray, out f_Hit, 0.7f) && f_Hit.collider.CompareTag("player")) // 큐브 앞쪽에 플레이어가 충돌하면
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHoldMoving && Physics.Raycast(r_Ray, out r_Hit, 0.5f) && r_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHoldMoving && Physics.Raycast(l_Ray, out l_Hit, 0.5f) && l_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
            }
            //back
            if (Physics.Raycast(b_Ray, out b_Hit, 0.7f) && b_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHoldMoving && Physics.Raycast(l_Ray, out l_Hit, 0.5f) && l_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHoldMoving && Physics.Raycast(r_Ray, out r_Hit, 0.5f) && r_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
            }
            //right
            if (Physics.Raycast(r_Ray, out r_Hit, 0.7f) && r_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHoldMoving && Physics.Raycast(b_Ray, out b_Hit, 0.5f) && b_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHoldMoving && Physics.Raycast(f_Ray, out f_Hit, 0.5f) && f_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
            }
            //left
            if (Physics.Raycast(l_Ray, out l_Hit, 0.7f) && l_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHoldMoving && Physics.Raycast(f_Ray, out f_Hit, 0.5f) && f_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHoldMoving && Physics.Raycast(b_Ray, out b_Hit, 0.5f) && b_Hit.collider.CompareTag("Cube"))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
            }

            //---------Up/Down Movement---------
            if (Physics.Raycast(f_Ray, out f_Hit, 0.7f) && f_Hit.collider.CompareTag("player") ||
                Physics.Raycast(b_Ray, out b_Hit, 0.7f) && b_Hit.collider.CompareTag("player") ||
                Physics.Raycast(r_Ray, out r_Hit, 0.7f) && r_Hit.collider.CompareTag("player") ||
                Physics.Raycast(l_Ray, out l_Hit, 0.7f) && l_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.S) && isHoldMode && !isHoldMoving && Physics.Raycast(d_Ray, out d_Hit, 0.5f) && d_Hit.collider.CompareTag("Cube"))
                {
                    Debug.Log("HangingDownMovement");
                    targetCube = d_Hit.collider.gameObject;
                    StartCoroutine(MoveDownHangingOnCube());
                }
            }
            if (Physics.Raycast(f_Ray, out f_Hit, 0.7f) && f_Hit.collider.CompareTag("player") ||
                Physics.Raycast(b_Ray, out b_Hit, 0.7f) && b_Hit.collider.CompareTag("player") ||
                Physics.Raycast(r_Ray, out r_Hit, 0.7f) && r_Hit.collider.CompareTag("player") ||
                Physics.Raycast(l_Ray, out l_Hit, 0.7f) && l_Hit.collider.CompareTag("player"))
            {
               // 구현해야함

            }
        }

    }


    IEnumerator MovementClimbing(GameObject targetCube)
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward + (transform.up *2)) * (targetCube.transform.localScale.y*0.5f);
        //Vector3 end = targetCube.transform.position + transform.up * 0.8f;
        float journeyLength = Vector3.Distance(start, end); 

        float startTime = Time.time; 
        float journeyDuration = journeyLength / moveSpeed;

        // 이동 중이면
        while(Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney); // 플레이어를 보간 이동함
            yield return null;
        }

        transform.position = end;
    } 


    IEnumerator F_HangOnCube(GameObject targetCube) // 매달리기 동작이 완성되면 종료됨 < 기억해
    {
        isHoldMode = true; // 정상적인 이동 불가
        playerRb.useGravity = false; // 중력 제거 

        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position - transform.forward * 0.8f;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration) // 현재까지의 시간 < 이동에 필요한 전체 시간
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed; // 현재까지 이동한 거리
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            yield return null;
        }
        transform.position = end;
    }

    IEnumerator B_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; // 정상적인 이동 불가
        playerRb.useGravity = false; // 중력 제거 

        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position + transform.forward * 0.8f;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / motionSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * motionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            // 매달릴때 캐릭터의 방향을 큐브쪽으로 돌림
            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); // 최종 회전 설정
    }

    IEnumerator L_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; // 정상적인 이동 불가
        playerRb.useGravity = false; // 중력 제거 

        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position - transform.right * 0.8f;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / motionSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * motionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            // 부드러운 회전 적용
            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); // 최종 회전 설정
    }

    IEnumerator R_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; // 정상적인 이동 불가
        playerRb.useGravity = false; // 중력 제거 

        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position + transform.right * 0.8f;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / motionSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * motionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            // 부드러운 회전 적용
            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); // 최종 회전 설정
    }


    IEnumerator MoveLeftHangingOnCube()
    {
        isHoldMoving = true;
        Vector3 start = transform.position;
        Vector3 end = transform.position + (-transform.right);

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }
        transform.position = end;
        isHoldMoving = false;
    }
    IEnumerator MoveRightHangingOnCube()
    {
        isHoldMoving = true;
        Vector3 start = transform.position;
        Vector3 end = transform.position + transform.right;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        transform.position = end;
        isHoldMoving = false;
    }

    IEnumerator MoveUpHangingOnCube()
    {
        isHoldMoving = true;
        Vector3 start = transform.position;
        Vector3 end = transform.position + transform.up;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        transform.position = end;
        isHoldMoving = false;
    }

    IEnumerator MoveDownHangingOnCube()
    {
        isHoldMoving = true;
        Vector3 start = transform.position;
        Vector3 end = transform.position - transform.up;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        transform.position = end;
        isHoldMoving = false;
    }
}