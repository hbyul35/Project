using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public GameObject[] Food;

    // 영양소 표시용 UI 텍스트 등
    public TextMesh nutritionText; // UI 텍스트 오브젝트에 연결

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

    private void Start()
    {
        // 게임 시작 시 초기 영양소 값을 설정합니다.
        ResetNutrition();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("carbohydrates"))
        {
            carbohydrates += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("protein"))
        {
            protein += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("fats"))
        {
            fats += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("sugar"))
        {
            sugar += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("dietaryFiber"))
        {
            carbohydrates += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("vitamins"))
        {
            vitamins += 10;
            Destroy(collision.gameObject);
        }
    }
}
