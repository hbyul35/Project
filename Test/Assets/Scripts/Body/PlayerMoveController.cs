using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public float moveSpeed = 5f, motionSpeed = 2f, rotationSpeed = 3.0f;
    public float maxMoveRange = 6.0f;
    public bool isRotating = true, isHoldMode = false, isDigging = false;
    public int[] pressedKeys = new int[4] {0, 1, 2, 3 }; // 0 = forward, 1 = left, 2 = back, 3 = right 

    private Rigidbody playerRb;
    private bool isHanging = false; // Hanging Action_Flag
    private bool isHangingMoving = false; // HangingMoving Action_Flag
    private GameObject targetCube;
    private Quaternion targetRotation = Quaternion.identity;

    private CubeController cubeController; // CubeController 정보를 담을 변수
    private HeadCollisionCheck headCollisionCheck; //HeadCollisionCheck 정보를 담을 변수
    private LoadCharacter loadCharacter;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        headCollisionCheck = GetComponentInChildren<HeadCollisionCheck>();
        loadCharacter = FindObjectOfType<LoadCharacter>();
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

        Vector3 castSize = new Vector3(transform.localScale.x/2.2f, 0.2f, transform.localScale.z/2.2f);
        float castDistance = 0.5f;
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, castSize * 0.5f, Vector3.down, out hit, Quaternion.identity, castDistance) && !loadCharacter.isDeath)
        {
            transform.Translate(movement);
            isDigging = true;
        }
        else
            isDigging = false;
    }

    void PlayerRotation()
    {
        Quaternion currentRotation = transform.rotation; 
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime); 

        if (isRotating && !loadCharacter.isDeath)
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

        RaycastHit forwarhit, downwarhit;

        if (Physics.Raycast(forwardRay, out forwarhit, 0.4f) && forwarhit.collider.CompareTag("Cube"))
        {
            targetCube = forwarhit.collider.gameObject; // 타겟 오브젝트에 충돌 큐브 오브젝트 값 추가

            cubeController = GetComponentInChildren<CubeController>();
            bool isMoving = cubeController.isMoving; 

            Ray targetCubeRay = new Ray(targetCube.transform.position, targetCube.transform.up); // 타겟 큐브 위쪽 방량으로 레이생성
            RaycastHit targetHit;

            Debug.DrawRay(targetCube.transform.position, targetCube.transform.up*0.6f, Color.magenta);

            // 1. 올라가려는 큐브 위에 연속된 큐브가 있으면 막음 2. 공중, 떨어지는 도중 등반하기 막음 3. 플레이어의 머리위에 큐브가 있을때 막음
            if (!Physics.Raycast(targetCubeRay, out targetHit, 0.6f) && Input.GetKeyDown(KeyCode.LeftShift) &&
                Physics.Raycast(downwardRay, out downwarhit, 0.6f)  && 
                !isMoving && !headCollisionCheck.isColliding)
            {
                StartCoroutine(MovementClimbing(targetCube));
            }
        }
    }

    void HangingMode() // 큐브 매달리기 & 매달린 상태에서 이동
    {
        Ray playerBodyRay_D = new Ray(transform.position, -transform.up); // 플레이어 아래
        Ray playerBodyRay_F = new Ray(transform.position, transform.forward); // 앞
        Ray playerBodyRay_B = new Ray(transform.position, -transform.forward); // 뒤
        Ray playerBodyRay_R = new Ray(transform.position, transform.right); // 오른쪽
        Ray playerBodyRay_L = new Ray(transform.position, -transform.right); // 왼쪽
        RaycastHit playerbodyRayHit_D, playerbodyRayHit_F, playerbodyRayHit_B, playerbodyRayHit_R, playerbodyRayHit_L;

        // 플레이어가 큐브를 밟고 있을때
        if (!isHoldMode &&  Physics.Raycast(playerBodyRay_D, out playerbodyRayHit_D, 0.5f) && playerbodyRayHit_D.collider.CompareTag("Cube"))
        {
            targetCube = playerbodyRayHit_D.collider.gameObject;

            Ray targetCubeRay_F = new Ray(targetCube.transform.position, targetCube.transform.forward);
            Ray targetCubeRay_B = new Ray(targetCube.transform.position, -targetCube.transform.forward);
            Ray targetCubeRay_L = new Ray(targetCube.transform.position, -targetCube.transform.right);
            Ray targetCubeRay_R = new Ray(targetCube.transform.position, targetCube.transform.right);
            RaycastHit targetCubeHit_F, targetCubeHit_B, targetCubeHit_L, targetCubeHit_R;

            //Vector3.Dot 두벡터의 내적을 계산-> 얼마나 유사한 방향을 가졌는지 / dot product = ax ​* bx ​+ ay ​* by ​+ az ​× bz
            if (Vector3.Dot(transform.forward, Vector3.forward) > 0.1f && Input.GetKey(KeyCode.G))
            {
                if (TryHangOnCube(KeyCode.W, playerBodyRay_F, out playerbodyRayHit_F) && TryHangOnCube(KeyCode.W, targetCubeRay_F, out targetCubeHit_F))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[0]));
                else if (TryHangOnCube(KeyCode.S, playerBodyRay_B, out playerbodyRayHit_B) && TryHangOnCube(KeyCode.S, targetCubeRay_B, out targetCubeHit_B))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[2]));
                else if (TryHangOnCube(KeyCode.D, playerBodyRay_R, out playerbodyRayHit_R) && TryHangOnCube(KeyCode.D, targetCubeRay_R, out targetCubeHit_R))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[3]));
                else if (TryHangOnCube(KeyCode.A, playerBodyRay_L, out playerbodyRayHit_L) && TryHangOnCube(KeyCode.A, targetCubeRay_L, out targetCubeHit_L))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[1]));
            }
            if (Vector3.Dot(transform.forward, Vector3.back) > 0.1f && Input.GetKey(KeyCode.G))
            {
                if (TryHangOnCube(KeyCode.W, playerBodyRay_F, out playerbodyRayHit_F) && TryHangOnCube(KeyCode.W, targetCubeRay_B, out targetCubeHit_B))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[0]));
                else if (TryHangOnCube(KeyCode.S, playerBodyRay_B, out playerbodyRayHit_B) && TryHangOnCube(KeyCode.S, targetCubeRay_F, out targetCubeHit_F))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[2]));
                else if (TryHangOnCube(KeyCode.D, playerBodyRay_R, out playerbodyRayHit_R) && TryHangOnCube(KeyCode.D, targetCubeRay_L, out targetCubeHit_L))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[3]));
                else if (TryHangOnCube(KeyCode.A, playerBodyRay_L, out playerbodyRayHit_L) && TryHangOnCube(KeyCode.A, targetCubeRay_R, out targetCubeHit_R))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[1]));
            }
            if (Vector3.Dot(transform.forward, Vector3.right) > 0.1f && Input.GetKey(KeyCode.G))
            {
                if (TryHangOnCube(KeyCode.W, playerBodyRay_F, out playerbodyRayHit_F) && TryHangOnCube(KeyCode.W, targetCubeRay_R, out targetCubeHit_R))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[0]));
                else if (TryHangOnCube(KeyCode.S, playerBodyRay_B, out playerbodyRayHit_B) && TryHangOnCube(KeyCode.S, targetCubeRay_L, out targetCubeHit_L))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[2]));
                else if (TryHangOnCube(KeyCode.D, playerBodyRay_R, out playerbodyRayHit_R) && TryHangOnCube(KeyCode.D, targetCubeRay_B, out targetCubeHit_B))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[3]));
                else if (TryHangOnCube(KeyCode.A, playerBodyRay_L, out playerbodyRayHit_L) && TryHangOnCube(KeyCode.A, targetCubeRay_F, out targetCubeHit_F))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[1]));
            }

            if (Vector3.Dot(transform.forward, Vector3.left) > 0.1f && Input.GetKey(KeyCode.G))
            {
                if (TryHangOnCube(KeyCode.W, playerBodyRay_F, out playerbodyRayHit_F) && TryHangOnCube(KeyCode.W, targetCubeRay_L, out targetCubeHit_L))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[0]));
                else if (TryHangOnCube(KeyCode.S, playerBodyRay_B, out playerbodyRayHit_B) && TryHangOnCube(KeyCode.S, targetCubeRay_R, out targetCubeHit_R))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[2]));
                else if (TryHangOnCube(KeyCode.D, playerBodyRay_R, out playerbodyRayHit_R) && TryHangOnCube(KeyCode.D, targetCubeRay_F, out targetCubeHit_F))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[3]));
                else if (TryHangOnCube(KeyCode.A, playerBodyRay_L, out playerbodyRayHit_L) && TryHangOnCube(KeyCode.A, targetCubeRay_B, out targetCubeHit_B))
                    StartCoroutine(HangOnCube(targetCube, pressedKeys[1]));
            }
        }
        // 매달리기 해제
        if (Input.GetKeyDown(KeyCode.F))
        {
            isHoldMode = false;
            playerRb.useGravity = true;
        }

        if (Physics.Raycast(playerBodyRay_F, out playerbodyRayHit_F, 0.8f) && playerbodyRayHit_F.collider.CompareTag("Cube"))
        {
            //Debug.Log("현재 플레이어가 위치한 큐브");
            targetCube = playerbodyRayHit_F.collider.gameObject;

            Ray f_Ray = new Ray(targetCube.transform.position, targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.forward*1.5f, Color.red); // 앞

            Ray b_Ray = new Ray(targetCube.transform.position, -targetCube.transform.forward);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.forward*1.5f, Color.blue); // 뒤

            Ray r_Ray = new Ray(targetCube.transform.position, targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, targetCube.transform.right*1.5f, Color.yellow); //오른쪽

            Ray l_Ray = new Ray(targetCube.transform.position, -targetCube.transform.right);
            Debug.DrawRay(targetCube.transform.position, -targetCube.transform.right*1.5f, Color.magenta); //왼쪽

            RaycastHit f_Hit, b_Hit, l_Hit, r_Hit;

            //---------Left/Right Movement---------
            //forward
            if (Physics.Raycast(f_Ray, out f_Hit, 0.8f) && f_Hit.collider.CompareTag("player"))
            {
                if (CanMoveOnCube(KeyCode.A, r_Ray, out r_Hit, playerBodyRay_L, out playerbodyRayHit_L, 1.1f))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[1]));
                }
                else if (CanRotateOnCube(KeyCode.A, r_Ray, out r_Hit, playerBodyRay_L, out playerbodyRayHit_L))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[3]));
                }

                if (CanMoveOnCube(KeyCode.D, l_Ray, out l_Hit, playerBodyRay_R, out playerbodyRayHit_R, 1.1f))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[3]));
                }
                else if (CanRotateOnCube(KeyCode.D, l_Ray, out l_Hit, playerBodyRay_R, out playerbodyRayHit_R))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[1]));
                }
            }
            //back
            if (Physics.Raycast(b_Ray, out b_Hit, 0.8f) && b_Hit.collider.CompareTag("player"))
            {
                if (CanMoveOnCube(KeyCode.A, l_Ray, out l_Hit, playerBodyRay_L, out playerbodyRayHit_L, 1.1f))
                {
                    targetCube = l_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[1]));
                }
                else if (CanRotateOnCube(KeyCode.A, l_Ray, out l_Hit, playerBodyRay_L, out playerbodyRayHit_L))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[1]));
                }

                if (CanMoveOnCube(KeyCode.D, r_Ray, out r_Hit, playerBodyRay_R, out playerbodyRayHit_R, 1.1f))
                {
                    targetCube = r_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[3]));
                }
                else if (CanRotateOnCube(KeyCode.D, r_Ray, out r_Hit, playerBodyRay_R, out playerbodyRayHit_R))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[3]));
                }
            }
            //right
            if (Physics.Raycast(r_Ray, out r_Hit, 0.8f) && r_Hit.collider.CompareTag("player"))
            {
                if (CanMoveOnCube(KeyCode.A, b_Ray, out b_Hit, playerBodyRay_L, out playerbodyRayHit_L, 1.1f))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[1]));
                }
                else if (CanRotateOnCube(KeyCode.A, b_Ray, out b_Hit, playerBodyRay_L, out playerbodyRayHit_L))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[2]));
                }

                if (CanMoveOnCube(KeyCode.D, f_Ray, out f_Hit, playerBodyRay_R, out playerbodyRayHit_R, 1.1f))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[3]));
                }
                else if (CanRotateOnCube(KeyCode.D, f_Ray, out f_Hit, playerBodyRay_R, out playerbodyRayHit_R))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[0]));
                }
            }
            //left
            if (Physics.Raycast(l_Ray, out l_Hit, 0.8f) && l_Hit.collider.CompareTag("player"))
            {
                if (CanMoveOnCube(KeyCode.A, f_Ray, out f_Hit, playerBodyRay_L, out playerbodyRayHit_L, 1.1f))
                {
                    targetCube = f_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[1]));
                }
                else if (CanRotateOnCube(KeyCode.A, f_Ray, out f_Hit, playerBodyRay_L, out playerbodyRayHit_L))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[0]));
                }

                if (CanMoveOnCube(KeyCode.D, b_Ray, out b_Hit, playerBodyRay_R, out playerbodyRayHit_R, 1.1f))
                {
                    targetCube = b_Hit.collider.gameObject;
                    StartCoroutine(MoveHangingOnCube(pressedKeys[3]));
                }
                else if (CanRotateOnCube(KeyCode.D, b_Ray, out b_Hit, playerBodyRay_R, out playerbodyRayHit_R))
                {
                    StartCoroutine(MovePlayerCurve(targetCube, pressedKeys[2]));
                }
            }

            // 매달린상태에서 다시 위로 올라감
            Vector3 rayStartPoint = transform.position + new Vector3(0, 0.7f, 0); // ray 시작 위치 변경
            RaycastHit headHit;
            Debug.DrawRay(rayStartPoint, transform.forward * 0.5f, Color.red);

            if (isHoldMode && !isHangingMoving && !isHanging && !headCollisionCheck.isColliding &&
                !Physics.Raycast(rayStartPoint, transform.forward, out headHit, 0.5f) &&
                headHit.collider == null && Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(UnhangOnCube(targetCube));
                isHoldMode = false;
                playerRb.useGravity = true;
            }
        }
    }
    bool CanMoveOnCube(KeyCode key, Ray ray, out RaycastHit hit, Ray playerBodyRay, out RaycastHit playerBodyRayHit, float cubeRaycastDistance)
    {
        hit = new RaycastHit(); // 초기화 추가
        playerBodyRayHit = new RaycastHit(); // 초기화 추가

        bool canMove = false;

        if (Input.GetKeyDown(key) && isHoldMode && !isHangingMoving &&
            Physics.Raycast(ray, out hit, 0.8f) && hit.collider.CompareTag("Cube") &&
            !Physics.Raycast(playerBodyRay, out playerBodyRayHit, cubeRaycastDistance) && playerBodyRayHit.collider == null)
        {
            canMove = true;
        }

        return canMove;
    }
    bool CanRotateOnCube(KeyCode key, Ray ray, out RaycastHit hit, Ray playerBodyRay, out RaycastHit playerBodyRayHit)
    {
        hit = new RaycastHit(); // 초기화 추가
        playerBodyRayHit = new RaycastHit(); // 초기화 추가

        bool canRotate = false;

        if (Input.GetKeyDown(key) && isHoldMode && !isHangingMoving &&
            !Physics.Raycast(ray, out hit, 0.8f) && hit.collider == null &&
            !Physics.Raycast(playerBodyRay, out playerBodyRayHit, 0.8f) && playerBodyRayHit.collider == null)
        {
            canRotate = true;
        }

        return canRotate;
    }
    bool TryHangOnCube(KeyCode key, Ray ray, out RaycastHit hit)
    {
        hit = new RaycastHit();
        return Input.GetKeyDown(key) && !Physics.Raycast(ray, out hit, 1.1f) && hit.collider == null;
    }

    //블록 등산
    IEnumerator MovementClimbing(GameObject targetCube)
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward + (transform.up *2)) * (targetCube.transform.localScale.y*0.5f);
        float journeyLength = Vector3.Distance(start, end); 

        float startTime = Time.time; 
        float journeyDuration = journeyLength / moveSpeed;

        // 이동 중이면
        while(Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney); 
            yield return null;
        }

        transform.position = end;
    }
    //매달림 해제
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
    IEnumerator HangOnCube(GameObject targetCube, int pressedKeyValue)
    {
        isHoldMode = true; 
        playerRb.useGravity = false; 
        isHanging = true;

        Vector3 start = transform.position;
        Vector3 end = Vector3.zero;
        switch (pressedKeyValue)
        {
            case 0:
                end = targetCube.transform.position + transform.forward * 0.8f;
                break;
            case 1:
                end = targetCube.transform.position - transform.right * 0.8f;
                break;
            case 2:
                end = targetCube.transform.position - transform.forward * 0.8f;
                break;
            case 3:
                end = targetCube.transform.position + transform.right * 0.8f;
                break;
            default: break;
        }    

        float journeyLength = Vector3.Distance(start, end);
        float startTime = Time.time;
        float journeyDuration = journeyLength / motionSpeed;

        // 이동 중이면
        while (Time.time - startTime < journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * motionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fractionOfJourney);

            if (pressedKeyValue !=2)
            {
                // 매달릴때 캐릭터의 방향을 큐브쪽으로 돌림
                Quaternion targetRotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
            }

            yield return null;
        }

        transform.position = end;
        if (pressedKeyValue !=2) transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position); // 최종 회전 설정
        isHanging = false;
    }

    // 큐브에 매달린 상태로 좌,우로 움직임
    IEnumerator MoveHangingOnCube(int pressedKeyValue)
    {
        isHangingMoving = true;
        Vector3 start = transform.position;
        Vector3 end = Vector3.zero;

        if (pressedKeyValue > 2) 
        {
            end = transform.position - transform.right;
        }
        else
            end = transform.position + transform.right;

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
    IEnumerator MovePlayerCurve(GameObject targetCube, int pressedKeyValue)
    {
        isHangingMoving = true; // 시작 플래그
        Vector3 start = transform.position;
        Vector3 end = Vector3.zero;
        Vector3 middlePoint = Vector3.zero;
        switch (pressedKeyValue)
        {
            case 0:
                end = targetCube.transform.position + targetCube.transform.forward * 0.6f;
                middlePoint = (start + end) / 2f; // 중간 지점 계산
                middlePoint += Vector3.forward;// *1.1f; // 해당 지점을 늘림
                break;
            case 1:
                end = targetCube.transform.position - targetCube.transform.right * 0.6f;
                middlePoint = (start + end) / 2f; 
                middlePoint += Vector3.left;
                break;
            case 2:
                end = targetCube.transform.position - targetCube.transform.forward * 0.6f;
                middlePoint = (start + end) / 2f; 
                middlePoint += Vector3.back;
                break;
            case 3:
                end = targetCube.transform.position + targetCube.transform.right * 0.6f;
                middlePoint = (start + end) / 2f; 
                middlePoint += Vector3.right;
                break;
            default:
            break;
        }          

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
        transform.rotation = Quaternion.LookRotation(targetCube.transform.position - transform.position);
        isHangingMoving = false;   
    }  
}