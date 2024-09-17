using UnityEngine;
using System.Collections.Generic;

public class NutritionManager : MonoBehaviour
{
    public enum NutrientType
    {
        Carbohydrates,
        Protein,
        Fat,
        Fiber,
        Vitamins,
        Sugar
    }

    public List<GameObject> foodObjects = new List<GameObject>();
    private string selectedPhysique;

    private void Start()
    {
        selectedPhysique = PlayerPrefs.GetString("SelectedPhysique");
    }

    public void AddFoodObject(GameObject foodObject)
    {
        foodObjects.Add(foodObject);
    }

    public void RemoveFoodObject(GameObject foodObject)
    {
        foodObjects.Remove(foodObject);
    }

    public List<GameObject> GetFoodObjectsByNutrition(NutrientType nutrient)
    {
        List<GameObject> filteredFood = new List<GameObject>();
        string tagToFilter = "";

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