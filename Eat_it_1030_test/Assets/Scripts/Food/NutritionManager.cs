using UnityEngine;
using System.Collections.Generic;

public class NutritionManager : MonoBehaviour
{
    // �پ��� ����� ������ ����
    public enum NutrientType
    {
        Carbohydrates,       //ź��ȭ��
        Protein,             //�ܹ���
        Fat,                 //����
        Fiber,               //���̼���
        Vitamins,            //��Ÿ��
        Sugar                //���
    }

    // ���� ������Ʈ ����Ʈ �����Ҵ�
    public List<GameObject> foodObjects = new List<GameObject>();

    // ���� ���� ������Ʈ �߰�
    public void AddFoodObject(GameObject foodObject)
    {
        foodObjects.Add(foodObject);
    }

    // ���� ���� ������Ʈ ����
    public void RemoveFoodObject(GameObject foodObject)
    {
        foodObjects.Remove(foodObject);
    }

    // Ư�� ����ҿ� ���� ���� ���� ������Ʈ ���͸�
    public List<GameObject> GetFoodObjectsByNutrition(NutrientType nutrient)
    {
        List<GameObject> filteredFood = new List<GameObject>();
        string tagToFilter = "";

        // NutrientType�� ���� �±׸� ����
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

        // �� ���� ������Ʈ�� �±׸� Ȯ���ϰ� ���͸�
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