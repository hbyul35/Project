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
        float weight = float.Parse(weightInputField.text);
        float height = float.Parse(heightInputField.text);

        // BMI 계산
        float bmi = weight / (height * height);

        // 운동 목적에 따라 추천 체질을 설정
        string recommendedPhysique = GetRecommendedPhysique(bmi, purposeDropdown.value);

        // 추천 체질을 PlayerPrefs에 저장
        PlayerPrefs.SetString("RecommendedPhysique", recommendedPhysique);
        PlayerPrefs.Save();

        // SelectScene으로 전환
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
