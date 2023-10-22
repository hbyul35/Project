using UnityEngine;

public class CubeAbilityManager : MonoBehaviour
{
    public GameObject[] abilityCubes;

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
            hasGeneratedItem[i] = false; // �ʱⰪ���� ��� false ����
        }
    }
    void generateItems()
    {
        for (int i = 0; i < ItemCubePrefabs.Length; i++)
        {
            if (!hasGeneratedItem[i]) // �������� �������� �ʾ��� ���� ����
            {
                spawnRandomAbilityItem(i);
            }
        }
    }
    void spawnRandomAbilityItem(int blockIndex)
    {
        Ray upRay = new Ray(ItemCubePrefabs[blockIndex].transform.position, ItemCubePrefabs[blockIndex].transform.up); // �ش� �ε��� - ������ ť��
        RaycastHit upHit;

        if (Physics.Raycast(upRay, out upHit, 0.8f) && upHit.collider.CompareTag("player"))
        {
            int randomItem = Random.Range(1, 4); // 1���� 3������ ������ ���� ����
            Vector3 spawnPosition = ItemCubePrefabs[blockIndex].transform.position + Vector3.up * 1.0f; // �������� ������ġ 

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
