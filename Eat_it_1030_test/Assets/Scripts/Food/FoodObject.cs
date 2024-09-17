using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public NutritionManager.NutrientType nutrientType;
    public int scoreEffect;

    public void Consume()
    {
        // 음식을 먹을 때 실행되는 메서드
        // 여기서 음식 소멸 및 영양소 효과 적용 로직을 추가할 수 있음
        // 예: Score 증가 또는 감소, 체질에 따라 다른 효과 적용
        Destroy(gameObject);

        // 점수 영향 적용
        int scoreChange = GetScoreChange();
        // 점수를 적용하는 함수 호출 (게임매니저 스크립트에 구현)
        GameManager.Instance.UpdateScore(scoreChange);
    }

    private int GetScoreChange()
    {
        int scoreChange = 0;
        string selectedPhysique = PlayerPrefs.GetString("SelectedPhysique");

        // 체질에 따른 영향 계산
        switch (selectedPhysique)
        {
            case "Obese":
                if (nutrientType == NutritionManager.NutrientType.Protein ||
                    nutrientType == NutritionManager.NutrientType.Fiber ||
                    nutrientType == NutritionManager.NutrientType.Vitamins)
                {
                    scoreChange = scoreEffect;
                }
                else if (nutrientType == NutritionManager.NutrientType.Fat ||
                         nutrientType == NutritionManager.NutrientType.Sugar ||
                         nutrientType == NutritionManager.NutrientType.Carbohydrates)
                {
                    scoreChange = -scoreEffect;
                }
                break;
            case "Athlete":
                // 다른 체질에 대한 영향도 유사한 방식으로 계산
                break;
                // 다른 체질에 대한 영향 계산 추가
        }

        return scoreChange;
    }
}