using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위함.
public class CubeAbilityManager : MonoBehaviour
{
    public GameObject[] abilityCubes;
    [Header("ITEM_CUBE")]
    public GameObject[] ItemCubePrefabs;
    public GameObject[] AbilityItems;
    private bool[] hasGeneratedItem;
    [Header("DISPOSABLE_CUBE")]
    private float raycastDistance = 0.8f;
    private GameObject currentBlock;  // 일회용 블록 검사 위한 코드
    private bool hitDisposableBlock;
    int trapBlockIndex;
    int trapIndex;
    private GameObject player;

    private GameObject[] traps;
    private GameObject[] trapBlocks;  // 함정 블록
    private GameObject disposableBlock;
    private Vector3 trapPosition;
    private Vector3 trapBlockPosition;
    private float startY = 0.0f; // 시작 높이
    private float endY = 0.6f; // 최종 높이
    private float moveDuration = 1.0f; // 이동하는 데 걸리는 시간
    private float timer = 0.0f;
    private bool movingUp = false;
    private bool trapActivated = false; // Trap과 충돌 상태 기억하는 플래그

    public List<Image> hpImages; // HP 이미지들 Inspector에서 할당할 리스트
    public Sprite emptyHeart;
    void Start()
    {
        StartCoroutine(FindCoroutine());  // 시작 시 플레이어 찾아오는 부분에서 오류 발생하는 것 수정
        trapBlocks = GameObject.FindGameObjectsWithTag("TrapBlock");
        initializeHasGeneratedItems();
        traps = GameObject.FindGameObjectsWithTag("Trap");
        trapPosition = traps[trapIndex].transform.position;
        trapBlockPosition = trapBlocks[trapBlockIndex].transform.position;
        // 각 trapBlock에 대응하는 trap 찾아서 배열에 저장
        for (int i = 0; i < trapBlocks.Length; i++)
        {
            // 이 부분에서 각 trapBlock에 대응하는 trap을 찾아서 배열에 저장
            traps[i] = FindCorrespondingTrap(trapBlocks[i]);
        }
    }
    GameObject FindCorrespondingTrap(GameObject trapBlock)
    {
        // trapBlock에 대응하는 trap을 찾아서 반환하는 함수
        // 예를 들어, trapBlock의 이름과 trap의 이름이 어떤 규칙을 따르는 경우 사용할 수 있음
        // 예시: trapBlock의 이름이 "TrapBlock1"이면, trap의 이름이 "Trap1" 등과 대응
        string trapBlockName = trapBlock.name;
        string trapName = "Trap" + trapBlockName.Substring("TrapBlock".Length); // 예시 규칙

        return GameObject.Find(trapName);
    }
    IEnumerator FindCoroutine()
    {
        player = null;
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("player");
            yield return null;
        }
    }
    void Update()
    {
        StartCoroutine(DelayedFunction());
        ActivateTrap();
    }
    private IEnumerator DelayedFunction()
    {
        yield return new WaitForSeconds(1.0f);
        generateItems();
        RayCollisionCheck();


    }
    void initializeHasGeneratedItems()
    {
        hasGeneratedItem = new bool[ItemCubePrefabs.Length];
        for (int i = 0; i < hasGeneratedItem.Length; i++)
        {
            hasGeneratedItem[i] = false; // 초기값으로 모두 false 설정
        }
    }
    void generateItems()
    {
        for (int i = 0; i < ItemCubePrefabs.Length; i++)
        {
            if (!hasGeneratedItem[i]) // 아이템을 생성하지 않았을 때만 실행
            {
                spawnRandomAbilityItem(i);
            }
        }
    }
    void spawnRandomAbilityItem(int blockIndex)
    {
        Ray upRay = new Ray(ItemCubePrefabs[blockIndex].transform.position, ItemCubePrefabs[blockIndex].transform.up); // 해당 인덱스 - 아이템 큐브
        RaycastHit upHit;

        if (Physics.Raycast(upRay, out upHit, 0.8f) && upHit.collider.CompareTag("player"))
        {
            int randomItem = Random.Range(1, 4); // 1부터 3까지의 랜덤한 숫자 생성
            Vector3 spawnPosition = ItemCubePrefabs[blockIndex].transform.position + Vector3.up * 1.0f; // 아이템의 생성위치 

            switch (randomItem)
            {
                case 1:
                    Instantiate(AbilityItems[0], spawnPosition, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(AbilityItems[1], spawnPosition, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(AbilityItems[2], spawnPosition, Quaternion.identity);
                    break;
            }
            hasGeneratedItem[blockIndex] = true;
        }
    }

    //  충돌 검사를 실행
    public void RayCollisionCheck()
    {
        Vector3 playerFootPosition = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
        Ray playerFootRay = new Ray(player.transform.position, -player.transform.up);
        RaycastHit playerFoothit;

        if (Physics.Raycast(playerFootRay, out playerFoothit, raycastDistance))
        {
            GameObject hitObject = playerFoothit.collider.gameObject;
            Debug.DrawRay(player.transform.position, -player.transform.up * raycastDistance, Color.green);

            if (hitObject.CompareTag("DisposableBlock"))
            {
                if (!hitDisposableBlock)
                {
                    currentBlock = hitObject;
                    hitDisposableBlock = true;
                }
            }
            else if (hitObject.CompareTag("Trap"))
            {
                // Trap과 충돌한 경우
                if (!trapActivated)
                {
                    // 트랩에 충돌한 경우에만 처리
                    trapActivated = true;

                    if (hpImages.Count > 0)
                    {
                        Image imageToRemove = hpImages[0]; // 첫 번째 HP 이미지를 가져옴
                        hpImages.RemoveAt(0); // List에서 제거
                        //Destroy(imageToRemove.gameObject); // 이미지 게임 오브젝트를 제거
                        imageToRemove.sprite = emptyHeart; // 빈 하트 이미지로 변경

                        Debug.Log("HP - 1");
                    }
                }
            }

            else
            {
                if (trapActivated)
                {
                    trapActivated = false;
                }
                if (currentBlock != null && hitDisposableBlock)
                {
                    Destroy(currentBlock);
                    currentBlock = null;
                    hitDisposableBlock = false;
                }
            }
        }
        else
        {
            if (currentBlock != null && hitDisposableBlock)
            {
                Destroy(currentBlock);
                currentBlock = null;
                hitDisposableBlock = false;
            }
        }
    }

    private void ActivateTrap()
    {
        timer += Time.deltaTime;
        float t = timer / moveDuration; // 이동 진행 상태 (0.0에서 1.0)

        if (movingUp)
        {
            t = 1.0f - t; // 내려갈 때 t 값을 뒤집음
        }
        float newY = Mathf.Lerp(startY, endY, t);

        for (int i = 0; i < trapBlocks.Length; i++)
        {
            GameObject trapBlock = trapBlocks[i];
            GameObject trap = traps[i];

            // trapblock의 움직임에 따라 trap의 위치 업데이트
            Vector3 trapblockMovement = trapBlock.transform.position - trapBlockPosition;
            Vector3 newPosition = trapPosition + new Vector3(0.0f, newY, 0.0f) + trapblockMovement;
            trap.transform.position = newPosition;
        }

        // 범위를 벗어나면 방향을 바꾸고 타이머 초기화
        if (newY >= endY || newY <= startY)
        {
            movingUp = !movingUp;
            timer = 0.0f;
        }
    }

}
