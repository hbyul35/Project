using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public enum NutrientType
    {
        Carbohydrates,
        Sugar,
        Fat,
        Protein,
        Vitamins,
        Fiber
    }
    public NutrientType nutrientType;

    private string selectedPhysique;

    private void Start()
    {
        selectedPhysique = PlayerPrefs.GetString("RecommendedPhysique");
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
        Dictionary<string, Dictionary<NutrientType, int>> physiqueScoreMap = new Dictionary<string, Dictionary<NutrientType, int>>()
        {
            {"Obese", new Dictionary<NutrientType, int>
                {
                    {NutrientType.Protein, 100},
                    {NutrientType.Fiber, 100},
                    {NutrientType.Vitamins, 100},
                    {NutrientType.Fat, -100},
                    {NutrientType.Sugar, -100},
                    {NutrientType.Carbohydrates, -100}
                }
            },
            {"Athlete", new Dictionary<NutrientType, int>
                {
                    {NutrientType.Protein, 100},
                    {NutrientType.Carbohydrates, 100},
                    {NutrientType.Vitamins, 100},
                    {NutrientType.Fiber, 100},
                    {NutrientType.Fat, -100},
                    {NutrientType.Sugar, -100}
                }
            },
            {"Diabetic", new Dictionary<NutrientType, int>
                {
                    {NutrientType.Protein, 100},
                    {NutrientType.Vitamins, 100},
                    {NutrientType.Fiber, 100},
                    {NutrientType.Fat, -100},
                    {NutrientType.Sugar, -100},
                    {NutrientType.Carbohydrates, -100}
                }
            },
            {"Underweight", new Dictionary<NutrientType, int>
                {
                    {NutrientType.Protein, 100},
                    {NutrientType.Fat, 100},
                    {NutrientType.Carbohydrates, 100},
                    {NutrientType.Sugar, -100}
                }
            },
            {"Basic", new Dictionary<NutrientType, int>
                {
                    {NutrientType.Protein, 100},
                    {NutrientType.Fat, 100},
                    {NutrientType.Carbohydrates, 100},
                    {NutrientType.Sugar, -100}
                }
            }
        };

        if (physiqueScoreMap.TryGetValue(selectedPhysique, out Dictionary<NutrientType, int> nutrientScoreMap))
        {
            if (nutrientScoreMap.TryGetValue(nutrientType, out int nutrientScore))
            {
                scoreChange = nutrientScore;
            }
        }

        return scoreChange;
    }
}