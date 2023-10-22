using TMPro;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GameManager : MonoBehaviour
{
    [Tooltip("User Body Information")]
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    public TMP_Dropdown purposeDropdown;

    public static GameManager Instance;
    private int score;
    public bool flag = false;
    public int chatacterIndex;
    private BodyManager.BodyType recommendedPhysique; // 사용자의 정보에 따른 추천 피지크

    // 싱글톤
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool Flag
    {
        get { return flag; }
        set { flag = value; }
    }
    private void Start()
    {
        score = 0;
    }
    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        // 점수 변경에 따른 처리 수행
    }

    public void Calculate_BMI_And_Recommend() 
    {
        float weight, height;
        
        if (float.TryParse(weightInputField.text, out weight) && float.TryParse(heightInputField.text, out height))
        {
            height /= 100; // 미터로 변환
            float bmi = weight / (height * height);
            Debug.Log(bmi);

            if (purposeDropdown.value == 0)
            {
                recommendedPhysique = bmi < 18.5f ? BodyManager.BodyType.Underweight : bmi < 24.9f ? BodyManager.BodyType.Basic : BodyManager.BodyType.Obese;
            }
            else if (purposeDropdown.value == 1)
            {
                recommendedPhysique = BodyManager.BodyType.Athlete;
            }
            else if (purposeDropdown.value == 2)
            {
                recommendedPhysique = BodyManager.BodyType.Diabetic;
            }
            Debug.Log("Seccsec");
            // 로컬에 저장
            PlayerPrefs.SetString("RecommendedPhysique", recommendedPhysique.ToString());
            PlayerPrefs.Save();

            flag = true;
        }
        else
        {
            // 유효하지 않은 입력이 있을 때 처리 (예: 경고 표시 등)
            Debug.LogWarning("Invalid input. Please enter valid weight and height.");
        }
    }

}
