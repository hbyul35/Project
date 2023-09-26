using UnityEngine;
using UnityEngine.UI;

public class NutrientController : MonoBehaviour
{
    // 플레이어의 영양소 상태
    public float carbohydrates = 0f;                                      // 탄수화물
    public float protein = 0f;                                            // 단백질
    public float fats = 0f;                                               // 지방
    public float sugar = 0f;                                              // 당류
    public float dietaryFiber = 0f;                                       // 식이섬유
    public float vitamins = 0f;                                           // 비타민

    // 영양소 표시용 UI 텍스트 등
    public TextMesh nutritionText; // UI 텍스트 오브젝트에 연결

    private void Start()
    {
        // 게임 시작 시 초기 영양소 값을 설정합니다.
        ResetNutrition();
    }

    // 영양소 초기화
    public void ResetNutrition()
    {
        carbohydrates = 0f;
        protein = 0f;
        fats = 0f;
        sugar = 0f;
        dietaryFiber = 0f;
        vitamins = 0f;
    }

    public void UpdateNutrients(float carb, float prot, float fat, float sugar, float fiber, float vit)
    {
        carbohydrates += carb;
        protein += prot;
        this.fats += fat;
        this.sugar += sugar;
        dietaryFiber += fiber;
        vitamins += vit;

        // 여기서 영양소 변화에 따른 처리를 할 수 있음
        // 예: UI 업데이트, 영양소 상태 체크, 게임 진행 상태에 영향 주기 등
    }
}
