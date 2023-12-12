using UnityEngine;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    // 각각의 음식 프리팹을 갖고 있는 리스트
    public List<GameObject> carbohydratePrefabs = new List<GameObject>();
    public List<GameObject> proteinPrefabs = new List<GameObject>();
    public List<GameObject> fatPrefabs = new List<GameObject>();
    public List<GameObject> sugarPrefabs = new List<GameObject>();
    public List<GameObject> fiberPrefabs = new List<GameObject>();
    public List<GameObject> vitaminPrefabs = new List<GameObject>();

    // 각 음식 프리팹들의 스폰 확률
    private float carbohydrateProbability = 0.15f;
    private float proteinProbability = 0.40f;
    private float fatProbability = 0.15f;
    private float sugarProbability = 0.15f;
    private float fiberProbability = 0.15f;
    private float vitaminProbability = 0.05f;

    void Start()
    {
        SpawnFood(carbohydratePrefabs, carbohydrateProbability);
        SpawnFood(proteinPrefabs, proteinProbability);
        SpawnFood(fatPrefabs, fatProbability);
        SpawnFood(sugarPrefabs, sugarProbability);
        SpawnFood(fiberPrefabs, fiberProbability);
        SpawnFood(vitaminPrefabs, vitaminProbability);
    }

    void SpawnFood(List<GameObject> foodPrefabs, float spawnProbability)
    {
        float randomValue = Random.value; // 0에서 1 사이의 랜덤 값 생성

        // 랜덤 값이 스폰 확률보다 작을 때 해당 음식 프리팹을 스폰
        if (randomValue <= spawnProbability)
        {
            int randomIndex = Random.Range(0, foodPrefabs.Count); // 랜덤한 인덱스 선택
            GameObject selectedFoodPrefab = foodPrefabs[randomIndex];

            // 선택된 음식 프리팹을 스폰 (위치는 스포너 오브젝트의 위치로 설정)
            Instantiate(selectedFoodPrefab, transform.position, Quaternion.identity);
        }
    }
}