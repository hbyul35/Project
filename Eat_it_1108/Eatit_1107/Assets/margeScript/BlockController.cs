using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour  // 블록 낙하 관련 코드
{   
    public float fallSpeed = 1.0f;  // 블록이 떨어지는 속도 조절
    private bool isFalling = false;  // 현재 블록이 떨어지고 있는지 여부를 나타내는 플래그
    private Transform blockTransform;  // 현재 블록의 Transform 컴포넌트 저장
    private Transform blockBorderTransform;  // BlockBorder의 Transform 컴포넌트 저장

    // Start is called before the first frame update
    void Start()
    {
        blockTransform = transform;  // 현재 블록의 Transform 컴포넌트를 초기화
        blockBorderTransform = transform.Find("BlockBorder");  // BlockBorder의 Transform 컴포넌트 초기화
    }

    private void Update()
    {
        // 현재 블록의 위치
        Vector3 currentPosition = blockTransform.position;

        if (isFalling)
        {
            // 아래로 떨어질 목표 위치 계산
            Vector3 targetPosition = currentPosition - Vector3.up; 

            // 블록이 목표 위치로 떨어지도록 보간
            blockTransform.position = Vector3.MoveTowards(currentPosition, targetPosition, fallSpeed * Time.deltaTime);

            // 만약 BlockBorder가 다른 블록과 충돌하면 떨어짐 멈추고 플래그 설정
            if (AttachBlock() || IsGrounded())
            {
                isFalling = false;
                FixBlockToGround();
            }
        }
        else
        {
            // 블록이 떨어지고 있지 않은 경우, 플레이어가 블록을 밀어서 충돌한 블록이 없으면 다시 떨어지게 설정
            if (!AttachBlock())
            {
                isFalling = true;
            }
        }
    }

    // BlockBorder가 다른 블록과 충돌하는지 검사
    private bool AttachBlock()
    {
        Collider[] colliders = Physics.OverlapBox(blockBorderTransform.position, blockBorderTransform.localScale / 2.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("CubeBorder"))  // 블록의 윗면과 검사  
            {
                return true;  // 다른 블록과 충돌
            }
        }
        return false;  // 아무 것도 충돌하지 않음
    }

    // 블록이 바닥과 닿았는지 검사
    private bool IsGrounded()
    {
        return blockTransform.position.y <= 0.1f;
    }

    // 블록을 바닥에 고정
    private void FixBlockToGround()
    {
        Vector3 newPosition = blockTransform.position;
        newPosition.y = Mathf.Round(newPosition.y); // y좌표를 가장 가까운 정수로 반올림하여 고정 
        blockTransform.position = newPosition;
    }
}