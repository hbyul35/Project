using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour  // ��� ���� ���� �ڵ�
{   
    public float fallSpeed = 1.0f;  // ����� �������� �ӵ� ����
    private bool isFalling = true;  // ���� ����� �������� �ִ��� ���θ� ��Ÿ���� �÷���
    private Transform blockTransform;  // ���� ����� Transform ������Ʈ ����
    private Transform blockBorderTransform;  // BlockBorder�� Transform ������Ʈ ����

    // Start is called before the first frame update
    void Start()
    {
        blockTransform = transform;  // ���� ����� Transform ������Ʈ�� �ʱ�ȭ
        blockBorderTransform = transform.Find("BlockBorder");  // BlockBorder�� Transform ������Ʈ �ʱ�ȭ
    }

    private void Update()
    {
        // ���� ����� ��ġ
        Vector3 currentPosition = blockTransform.position;

        if (isFalling)
        {
            // �Ʒ��� ������ ��ǥ ��ġ ���
            Vector3 targetPosition = currentPosition - Vector3.up; 

            // ����� ��ǥ ��ġ�� ���������� ����
            blockTransform.position = Vector3.MoveTowards(currentPosition, targetPosition, fallSpeed * Time.deltaTime);

            // ���� BlockBorder�� �ٸ� ��ϰ� �浹�ϸ� ������ ���߰� �÷��� ����
            if (AttachBlock() || IsGrounded())
            {
                isFalling = false;
                FixBlockToGround();
            }
        }
        else
        {
            // ����� �������� ���� ���� ���, �÷��̾ ����� �о �浹�� ����� ������ �ٽ� �������� ����
            if (!AttachBlock())
            {
                isFalling = true;
            }
        }
    }

    // BlockBorder�� �ٸ� ��ϰ� �浹�ϴ��� �˻�
    private bool AttachBlock()
    {
        Collider[] colliders = Physics.OverlapBox(blockBorderTransform.position, blockBorderTransform.localScale / 2.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("CubeBorder"))  // ����� ����� �˻�  
            {
                return true;  // �ٸ� ��ϰ� �浹
            }
        }
        return false;  // �ƹ� �͵� �浹���� ����
    }

    // ����� �ٴڰ� ��Ҵ��� �˻�
    private bool IsGrounded()
    {
        return blockTransform.position.y <= 0.1f;
    }

    // ����� �ٴڿ� ����
    private void FixBlockToGround()
    {
        Vector3 newPosition = blockTransform.position;
        newPosition.y = Mathf.Round(newPosition.y); // y��ǥ�� ���� ����� ������ �ݿø��Ͽ� ���� 
        blockTransform.position = newPosition;
    }
}