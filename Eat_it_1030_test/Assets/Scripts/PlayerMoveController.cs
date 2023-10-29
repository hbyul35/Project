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
    private bool isHanging = false; // Hanging Action_Flag
    private bool isHangingMoving = false; // HangingMoving Action_Flag
    private GameObject targetCube;
    private Quaternion targetRotation = Quaternion.identity;

    private CubeController cubeController; // CubeController 정보를 담을 변수
    private HeadCollisionCheck headCollisionCheck; //HeadCollisionCheck 정보를 담을 변수

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }


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

        if (Physics.Raycast(forwardRay, out forwarhit, 0.3f)&& Input.GetKeyDown(KeyCode.LeftShift))
        {
            targetCube = forwarhit.collider.gameObject; // 타겟 오브젝트에 충돌 큐브 오브젝트 값 추가

            // 등산 조건 검사 추가 09/30 
            cubeController = GetComponentInChildren<CubeController>();
            bool isMoving = cubeController.isMoving; // 큐브의 이동이 종료됐을때

            headCollisionCheck = GetComponentInChildren<HeadCollisionCheck>(); // 머리 충돌체크용
            bool isHeadColliding = headCollisionCheck.isColliding;

            Ray targetCubeRay = new Ray(targetCube.transform.position, targetCube.transform.up); // 타겟 큐브 위쪽 방향으로 레이생성
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

    void HangingMode() // 큐브 매달리기 & 매달린 상태에서 이동
    {
        Ray downwardRay = new Ray(transform.position, -transform.up);
        Ray forwardRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        RaycastHit downwarhit, forwarhit;

        //if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.W) && Physics.Raycast(downwardRay, out downwarhit, 0.5f) && !isHoldMode && (downwarhit.collider.CompareTag("Cube") || downwarhit.collider.CompareTag("TrapBlock") || downwarhit.collider.CompareTag("HeavyBlock") || downwarhit.collider.CompareTag("DisposableBlock")))
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.W) && Physics.Raycast(downwardRay, out downwarhit, 0.5f) && !isHoldMode)
        {
            if (!Physics.Raycast(forwardRay, out forwarhit, 1.1f) && forwarhit.collider == null)
            {
                targetCube = downwarhit.collider.gameObject;
                StartCoroutine(F_HangOnCube(targetCube));
            }
        }
        else if(Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.S) && Physics.Raycast(downwardRay, out downwarhit, 0.5f) && !isHoldMode)
        {
            Ray backRay = new Ray(transform.position, -transform.forward);
            RaycastHit backhit;
            if (!Physics.Raycast(backRay, out backhit, 1.1f) && backhit.collider == null)
            {
                targetCube = downwarhit.collider.gameObject;
                StartCoroutine(B_HangOnCube(targetCube));
            }
        }
        else if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.A) && Physics.Raycast(downwardRay, out downwarhit, 0.5f) && !isHoldMode)
        {
            Ray leftRay = new Ray(transform.position, -transform.right);
            RaycastHit lefthit;
            if (!Physics.Raycast(leftRay, out lefthit, 1.1f) && lefthit.collider == null)
            {
                targetCube = downwarhit.collider.gameObject;
                StartCoroutine(L_HangOnCube(targetCube));
            }
        }
        else if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.D) && Physics.Raycast(downwardRay, out downwarhit, 0.5f) && !isHoldMode)
        {
            Ray rightRay = new Ray(transform.position, transform.right);
            RaycastHit righthit;
            if (!Physics.Raycast(rightRay, out righthit, 1.1f) && righthit.collider == null)
            {
                targetCube = downwarhit.collider.gameObject;
                StartCoroutine(R_HangOnCube(targetCube));
            }
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            isHoldMode = false; 
            playerRb.useGravity = true; 
        }      

        if (Physics.Raycast(forwardRay, out forwarhit, 0.8f))
        {
            //Debug.Log("현재 플레이어가 위치한 큐브");
            targetCube = forwarhit.collider.gameObject;
            
            Ray f_Ray = new Ray(targetCube.transform.position, targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.forward*1.5f, Color.red); // 앞

            Ray b_Ray = new Ray(targetCube.transform.position, -targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.forward*1.5f, Color.blue); // 뒤

            Ray r_Ray = new Ray(targetCube.transform.position, targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.right*1.5f, Color.yellow); //오른쪽

            Ray l_Ray = new Ray(targetCube.transform.position, -targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.right*1.5f, Color.magenta); //왼쪽

            //Ray u_Ray = new Ray(targetCube.transform.position, targetCube.transform.up);
            //Debug.DrawRay(targetCube.transform.position, targetCube.transform.up*0.5f, Color.white); //위쪽

            //Ray d_Ray = new Ray(targetCube.transform.position, -targetCube.transform.up);
            //Debug.DrawRay(targetCube.transform.position, -targetCube.transform.up*1.5f, Color.black); //아래쪽
            RaycastHit f_Hit, b_Hit, l_Hit, r_Hit; //, u_Hit, d_Hit


            //---------Left/Right Movement---------
            Ray playerBodyRay_R = new Ray(transform.position, transform.right);
            Ray playerBodyRay_L = new Ray(transform.position, -transform.right);
            RaycastHit bodyRayHit_R, bodyRayHit_L;
            //forward
            if (Physics.Raycast(f_Ray, out f_Hit, 0.8f) && f_Hit.collider.CompareTag("player")) // 큐브 앞쪽에 플레이어가 충돌하면
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && Physics.Raycast(r_Ray, out r_Hit, 0.8f))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(r_Ray, out r_Hit, 0.8f) && r_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_L, out bodyRayHit_L, 0.8f) && bodyRayHit_L.collider == null)
                {
                    StartCoroutine(RotateRightOnCube(targetCube));
                }

                if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && Physics.Raycast(l_Ray, out l_Hit, 0.8f))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
                if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(l_Ray, out l_Hit, 0.8f) && l_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_R, out bodyRayHit_R, 0.8f) && bodyRayHit_R.collider == null)
                {
                    StartCoroutine(RotateLeftOnCube(targetCube));
                }
            }
            //back
            if (Physics.Raycast(b_Ray, out b_Hit, 0.8f) && b_Hit.collider.CompareTag("player"))
            {              
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && Physics.Raycast(l_Ray, out l_Hit, 0.8f))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving &&
                    !Physics.Raycast(l_Ray, out l_Hit, 0.8f) && l_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_L, out bodyRayHit_L, 0.8f) && bodyRayHit_L.collider == null)
                {
                    StartCoroutine(RotateLeftOnCube(targetCube));
                }

                if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && Physics.Raycast(r_Ray, out r_Hit, 0.8f))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(r_Ray, out r_Hit, 0.8f) && r_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_R, out bodyRayHit_R, 0.8f) && bodyRayHit_R.collider == null)
                {
                    StartCoroutine(RotateRightOnCube(targetCube));
                }
            }
            //right
            if (Physics.Raycast(r_Ray, out r_Hit, 0.8f) && r_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && Physics.Raycast(b_Ray, out b_Hit, 0.8f))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(b_Ray, out b_Hit, 0.8f) && b_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_L, out bodyRayHit_L, 0.8f) && bodyRayHit_L.collider == null)
                {
                    StartCoroutine(RotateBackOnCube(targetCube));
                }

                if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && Physics.Raycast(f_Ray, out f_Hit, 0.8f))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(f_Ray, out f_Hit, 0.8f) && f_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_R, out bodyRayHit_R, 0.8f) && bodyRayHit_R.collider == null)
                {
                    StartCoroutine(RotateForwardOnCube(targetCube));
                }
            }
            //left
            if (Physics.Raycast(l_Ray, out l_Hit, 0.8f) && l_Hit.collider.CompareTag("player"))
            {
                if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && Physics.Raycast(f_Ray, out f_Hit, 0.8f))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveLeftHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.A) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(f_Ray, out f_Hit, 0.8f) && f_Hit.collider== null &&
                    !Physics.Raycast(playerBodyRay_L, out bodyRayHit_L, 0.8f) && bodyRayHit_L.collider == null)
                {
                    StartCoroutine(RotateForwardOnCube(targetCube));
                }

                if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && Physics.Raycast(b_Ray, out b_Hit, 0.8f))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveRightHangingOnCube());
                }
                else if (Input.GetKeyDown(KeyCode.D) && isHoldMode && !isHangingMoving && 
                    !Physics.Raycast(b_Ray, out b_Hit, 0.8f) && b_Hit.collider == null &&
                    !Physics.Raycast(playerBodyRay_R, out bodyRayHit_R, 0.8f) && bodyRayHit_R.collider == null)
                {
                    StartCoroutine(RotateBackOnCube(targetCube));
                }
            }

            // 매달린상태에서 다시 위로 올라감
            Vector3 rayStartPoint = transform.position + new Vector3(0, 0.7f, 0); // ray 시작 위치 변경
            RaycastHit headHit;
            Debug.DrawRay(rayStartPoint, transform.forward * 0.5f, Color.red);

            if (isHoldMode && !isHangingMoving && !isHanging && 
                !Physics.Raycast(rayStartPoint, transform.forward, out headHit, 0.5f) && 
                headHit.collider == null && Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(UnhangOnCube(targetCube));
                isHoldMode = false;
                playerRb.useGravity = true;
            }
        }
    }

    //블록 등산 및 매달림 해제
    //IEnumerator MovementClimbing(GameObject targetCube)
    //{
    //    Vector3 start = transform.position;
    //    Vector3 end = start + (transform.forward + (transform.up * 1.2f)) * (targetCube.transform.localScale.y * 0.55f);
    //    end.x = start.x; // X 축 값 변경 없이 유지
    //    float journeyLength = Vector3.Distance(start, end);

    //    float startTime = Time.time;
    //    float journeyDuration = journeyLength / moveSpeed;

    //    // 이동 중이면
    //    while (Time.time - startTime < journeyDuration)
    //    {
    //        float distanceCovered = (Time.time - startTime) * moveSpeed;
    //        float fractionOfJourney = distanceCovered / journeyLength;
    //        Vector3 newPosition = Vector3.Lerp(start, end, fractionOfJourney);
    //        newPosition.x = start.x; // X 축 값 변경 없이 유지
    //        transform.position = newPosition;
    //        yield return null;
    //    }

    //    // 마지막 위치에서 X 축 값 변경 없이 유지
    //    end.x = start.x;
    //    transform.position = end;
    //}
    IEnumerator MovementClimbing(GameObject targetCube)
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward + (transform.up * 1.2f)) * (targetCube.transform.localScale.y * 0.55f);
        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }

        transform.position = end;
    }

    IEnumerator UnhangOnCube(GameObject targetCube)
    {
        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position + transform.up;
        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / moveSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney); 
            yield return null;
        }

        transform.position = end;
    }

    //플레이어가 밟고 있는 큐브에 매달림
    IEnumerator B_HangOnCube(GameObject targetCube) 
    {
        isHoldMode = true; // 정상적인 이동 불가
        playerRb.useGravity = false; // 중력 제거 
        isHanging = true;

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
        isHanging = false;
    }
    IEnumerator F_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; 
        playerRb.useGravity = false; 
        isHanging = true;

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
        isHanging = false;
    }
    IEnumerator L_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; 
        playerRb.useGravity = false; 
        isHanging = true;

        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position - transform.right * 0.8f;

        float journeyLength = Vector3.Distance(start, end);

        float startTime = Time.time;
        float journeyDuration = journeyLength / motionSpeed;

        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * motionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); 
        isHanging = false;
    }
    IEnumerator R_HangOnCube(GameObject targetCube)
    {
        isHoldMode = true; 
        playerRb.useGravity = false;
        isHanging = true;

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

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); 
        isHanging = false;
    }

    // 큐브에 매달린 상태로 좌,우로 움직임
    IEnumerator MoveLeftHangingOnCube()
    {
        isHangingMoving = true;
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
        isHangingMoving = false;
    }
    IEnumerator MoveRightHangingOnCube()
    {
        isHangingMoving = true;
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
        isHangingMoving = false;
    }
    //MoveUpHangingOnCube, MoveDownHangingOnCube 예비 코드
    IEnumerator MoveUpHangingOnCube()
    {
        isHangingMoving = true;
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
        isHangingMoving = false;
    }
    IEnumerator MoveDownHangingOnCube()
    {
        isHangingMoving = true;
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
        isHangingMoving = false;
    }

    // 좌,우 끝부분으로 갔을때 다른방향으로 이동
    // 2차 베지어 곡선을 계산하는 함수
    Vector3 CalculateQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
    IEnumerator RotateRightOnCube(GameObject targetCube)
    {
        isHangingMoving = true; // 시작 플래그
        Vector3 start = transform.position; 
        Vector3 end = targetCube.transform.position + targetCube.transform.right * 0.5f;

        
        Vector3 middlePoint = (start + end) / 2f; // 중간 지점 계산
        middlePoint += Vector3.right;// *1.1f; // 해당 지점을 늘림

        float startTime = Time.time;
        float journeyDuration = 1.0f; // 이동에 걸리는 시간 

        while (Time.time - startTime < journeyDuration)
        {
            float elapsedTime = (Time.time - startTime) / journeyDuration;
            Vector3 curvePoint = CalculateQuadraticBezierPoint(start, middlePoint, end, elapsedTime);
            transform.position = curvePoint;

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); // 최종 회전 설정
        isHangingMoving = false;   
    }
    IEnumerator RotateLeftOnCube(GameObject targetCube)
    {
        isHangingMoving = true;
        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position - targetCube.transform.right * 0.6f;


        Vector3 middlePoint = (start + end) / 2f; 
        middlePoint += Vector3.left;

        float startTime = Time.time;
        float journeyDuration = 1.0f; 

        while (Time.time - startTime < journeyDuration)
        {
            float elapsedTime = (Time.time - startTime) / journeyDuration;
            Vector3 curvePoint = CalculateQuadraticBezierPoint(start, middlePoint, end, elapsedTime);
            transform.position = curvePoint;

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); 
        isHangingMoving = false;
    }
    IEnumerator RotateBackOnCube(GameObject targetCube)
    {
        isHangingMoving = true;
        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position - targetCube.transform.forward * 0.6f;


        Vector3 middlePoint = (start + end) / 2f; 
        middlePoint += Vector3.back;

        float startTime = Time.time;
        float journeyDuration = 1.0f; 

        while (Time.time - startTime < journeyDuration)
        {
            float elapsedTime = (Time.time - startTime) / journeyDuration;
            Vector3 curvePoint = CalculateQuadraticBezierPoint(start, middlePoint, end, elapsedTime);
            transform.position = curvePoint;

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); 
        isHangingMoving = false;
    }
    IEnumerator RotateForwardOnCube(GameObject targetCube)
    {
        isHangingMoving = true;
        Vector3 start = transform.position;
        Vector3 end = targetCube.transform.position + targetCube.transform.forward * 0.6f;


        Vector3 middlePoint = (start + end) / 2f; 
        middlePoint += Vector3.forward;

        float startTime = Time.time;
        float journeyDuration = 1.0f; 

        while (Time.time - startTime < journeyDuration)
        {
            float elapsedTime = (Time.time - startTime) / journeyDuration;
            Vector3 curvePoint = CalculateQuadraticBezierPoint(start, middlePoint, end, elapsedTime);
            transform.position = curvePoint;

            Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = end;
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); 
        isHangingMoving = false;
    }

}