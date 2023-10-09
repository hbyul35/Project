using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InformationSceneController : MonoBehaviour
{
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    public TMP_Dropdown purposeDropdown;

    public void CalculateBMIAndRecommend()
    {
        // 사용자로부터 입력받은 몸무게와 키를 가져옴
        float weight, height;

        // float 형태로 변환 후 저장
        if (float.TryParse(weightInputField.text, out weight) && float.TryParse(heightInputField.text, out height))
        {
            // BMI 계산
            float bmi = weight / (height * height);

            // 운동 목적에 따라 추천 체질을 설정
            string recommendedPhysique = GetRecommendedPhysique(bmi, purposeDropdown.value);

            // 추천 체질을 PlayerPrefs에 저장
            PlayerPrefs.SetString("RecommendedPhysique", recommendedPhysique);
            PlayerPrefs.Save();
        }
        else
        {
            // 유효하지 않은 입력이 있을 때 처리 (예: 경고 표시 등)
            Debug.LogWarning("Invalid input. Please enter valid weight and height.");
        }

    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    private string GetRecommendedPhysique(float bmi, int purposeIndex)
    {
        // BMI와 운동 목적에 따라 추천 체질을 결정하는 로직
        if (purposeIndex == 0) // 건강
        {
            return bmi < 18.5f ? "Underweight" : bmi < 24.9f ? "Normal" : "Obese";
        }
        else if (purposeIndex == 1) // 근육
        {
            return "Athlete";
        }
        else if (purposeIndex == 2) // 당 조절
        {
            return "Diabetic";
        }

        return "Normal";
    }
}