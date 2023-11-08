using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public NutritionManager.NutrientType nutrientType;

    private string selectedPhysique;

    private void Start()
    {
        selectedPhysique = PlayerPrefs.GetString("SelectedPhysique");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            int scoreChange = GetScoreChange();

            GameManager.Instance.UpdateScore(scoreChange);

            Destroy(gameObject);
        }
    }

    private int GetScoreChange()
    {
        int scoreChange = 0;

        // 체질에 따른 영향 계산
        switch (selectedPhysique)
        {
            case "Obese":
                if (nutrientType == NutritionManager.NutrientType.Protein || nutrientType == NutritionManager.NutrientType.Fiber || nutrientType == NutritionManager.NutrientType.Vitamins)
                {
                    scoreChange = -100;
                }
                else if (nutrientType == NutritionManager.NutrientType.Fat || nutrientType == NutritionManager.NutrientType.Sugar || nutrientType == NutritionManager.NutrientType.Carbohydrates)
                {
                    scoreChange = 100;
                }
                break;
            case "Athlete":
                if (nutrientType == NutritionManager.NutrientType.Protein || nutrientType == NutritionManager.NutrientType.Carbohydrates || nutrientType == NutritionManager.NutrientType.Vitamins || nutrientType == NutritionManager.NutrientType.Fiber)
                {
                    scoreChange = -100;
                }
                else if (nutrientType == NutritionManager.NutrientType.Fat || nutrientType == NutritionManager.NutrientType.Sugar)
                {
                    scoreChange = 100;
                }
                break;
            case "Diabetic":
                if (nutrientType == NutritionManager.NutrientType.Protein || nutrientType == NutritionManager.NutrientType.Vitamins || nutrientType == NutritionManager.NutrientType.Fiber)
                {
                    scoreChange = -100;
                }
                else if (nutrientType == NutritionManager.NutrientType.Fat || nutrientType == NutritionManager.NutrientType.Sugar || nutrientType == NutritionManager.NutrientType.Carbohydrates)
                {
                    scoreChange = 100;
                }
                break;
            case "Underweight":
                if (nutrientType == NutritionManager.NutrientType.Protein || nutrientType == NutritionManager.NutrientType.Fat || nutrientType == NutritionManager.NutrientType.Carbohydrates)
                {
                    scoreChange = -100;
                }
                else if (nutrientType == NutritionManager.NutrientType.Sugar)
                {
                    scoreChange = 100;
                }
                break;
        }

        return scoreChange;
    }
}