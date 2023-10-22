using UnityEngine;
using System.Collections.Generic;

public class NutritionManager : MonoBehaviour
{
    // 다양한 영양소 유형을 정의
    public enum NutrientType
    {
        Carbohydrates,       //탄수화물
        Protein,             //단백질
        Fat,                 //지방
        Fiber,               //식이섬유
        Vitamins,            //비타민
        Sugar                //당류
    }

    // 음식 오브젝트 리스트 동적할당
    public List<GameObject> foodObjects = new List<GameObject>();

    // 음식 게임 오브젝트 추가
    public void AddFoodObject(GameObject foodObject)
    {
        foodObjects.Add(foodObject);
    }

    // 음식 게임 오브젝트 제거
    public void RemoveFoodObject(GameObject foodObject)
    {
        foodObjects.Remove(foodObject);
    }

    // 특정 영양소에 따라 음식 게임 오브젝트 필터링
    public List<GameObject> GetFoodObjectsByNutrition(NutrientType nutrient)
    {
        List<GameObject> filteredFood = new List<GameObject>();
        string tagToFilter = "";

        // NutrientType에 따라 태그를 설정
        switch (nutrient)
        {
            case NutrientType.Carbohydrates:
                tagToFilter = "Carbohydrates";
                break;
            case NutrientType.Protein:
                tagToFilter = "Protein";
                break;
            case NutrientType.Fat:
                tagToFilter = "Fat";
                break;
            case NutrientType.Fiber:
                tagToFilter = "Fiber";
                break;
            case NutrientType.Vitamins:
                tagToFilter = "Vitamins";
                break;
            case NutrientType.Sugar:
                tagToFilter = "Sugar";
                break;
        }

        // 각 음식 오브젝트의 태그를 확인하고 필터링
        foreach (var foodObject in foodObjects)
        {
            if (foodObject.CompareTag(tagToFilter))
            {
                filteredFood.Add(foodObject);
            }
        }

        return filteredFood;
    }
}