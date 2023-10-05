using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {
  public float moveSpeed = 5f;
  public float maxMoveRange = 4.0f;
  public float rotationSpeed = 2.0f;
  public bool isRotating = true;

  private bool isCliming = false;
  private GameObject targetCube;
  private Quaternion targetRotation = Quaternion.identity;

  void Start() {}

  // Update is called once per frame
  void Update() {
    PlayerMovement();
    PlayerRotation();
    ClimbPlayer();
  }

  void PlayerMovement() {
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) *
                       moveSpeed *Time.deltaTime;

    transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, -maxMoveRange, maxMoveRange),
        transform.position.y,
        Mathf.Clamp(transform.position.z, -maxMoveRange, maxMoveRange));

    transform.Translate(movement);
  }

  void PlayerRotation() {
    Quaternion currentRotation = transform.rotation; // ���� ȸ����
    currentRotation = Quaternion.Slerp(currentRotation, targetRotation,
                                       rotationSpeed * Time.deltaTime);

    if (isRotating) {
      currentRotation = Quaternion.Slerp(currentRotation, targetRotation,
                                         rotationSpeed * Time.deltaTime);
      transform.rotation = currentRotation;

      if (Quaternion.Angle(currentRotation, targetRotation) < 0.05f) {
        isRotating = false;
      }
    } else {
      if (Input.GetKeyDown(KeyCode.E)) {
        targetRotation *= Quaternion.Euler(0, -90.0f, 0);
        isRotating = true;
      } else if (Input.GetKeyDown(KeyCode.Q)) {
        targetRotation *= Quaternion.Euler(0, 90.0f, 0);
        isRotating = true;
      }
    }
  }

  void ClimbPlayer() {
    Ray ray = new Ray(transform.position, transform.forward);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 0.3f)) {
      if (hit.collider.CompareTag("Cube") &&
          Input.GetKeyDown(KeyCode.LeftShift) &&
          !isCliming) // ray가 큐브와 충돌하면
      {
        targetCube =
            hit.collider
                .gameObject; // 타겟 오브젝트에 충돌 큐브 오브젝트 값 추가
        StartCoroutine(MovementClimbing(targetCube)); // 코루틴 시작
      }
    }
  }

  IEnumerator MovementClimbing(GameObject targetCube) {
    isCliming = true; // 시작 플래그

    Vector3 start = transform.position; // 시작 위치 내가 있는곳
    Vector3 end = start + (transform.forward + (transform.up * 2)) *
                              (targetCube.transform.localScale.y * 0.5f);
    float journeyLength = Vector3.Distance(start, end); //

    float startTime = Time.time; // 움직임 시작 시간
    float journeyDuration = journeyLength / moveSpeed;

    // 이동 중이면
    while (Time.time - startTime < journeyDuration) {
      float distanceCovered = (Time.time - startTime) * moveSpeed;
      float fractionOfJourney = distanceCovered / journeyLength;
      transform.position =
          Vector3.Lerp(start, end, fractionOfJourney); // 플레이어를 보간 이동함
      yield return null;
    }

    transform.position = end;
    isCliming = false;
  }
}