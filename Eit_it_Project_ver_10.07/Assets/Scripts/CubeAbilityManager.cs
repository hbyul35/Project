using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAbilityManager : MonoBehaviour
{
    //public GameObject[] abilityCubes;

    [Header("ITEM_CUBE")]
    public GameObject[] ItemCubePrefabs;
    public GameObject[] AbilityItems;
    private bool[] hasGeneratedItem;


    void Start()
    {

        initializeHasGeneratedItems();
    }

    void Update()
    {
        generateItems();
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

        if(Physics.Raycast(upRay, out upHit, 0.8f) && upHit.collider.CompareTag("player"))
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
}
