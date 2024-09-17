using TMPro;
using UnityEngine;
//using UnityEngine.Purchasing.MiniJSON;
//using UnityEngine.SceneManagement;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GameManager : MonoBehaviour
{

    [Header("User Body Information")]
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    public TMP_Dropdown purposeDropdown;

    public TextMeshProUGUI[] scoreText;

    public static GameManager Instance;
    private int score;
    public bool flag = false;
    public int chatacterIndex;
    private BodyManager.BodyType recommendedPhysique; // ������� ������ ���� ��õ ����ũ

    // �̱���
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
    public void UpdateScore(int changeScore)
    {
        score += changeScore;
        scoreText[0].text = "Score: " + score.ToString();
    }

    public void Calculate_BMI_And_Recommend() 
    {
        float weight, height;
        
        if (float.TryParse(weightInputField.text, out weight) && float.TryParse(heightInputField.text, out height))
        {
            height /= 100; // ���ͷ� ��ȯ
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
            // ���ÿ� ����
            PlayerPrefs.SetString("RecommendedPhysique", recommendedPhysique.ToString());
            PlayerPrefs.Save();

            flag = true;
        }
        else
        {
            // ��ȿ���� ���� �Է��� ���� �� ó�� (��: ��� ǥ�� ��)
            Debug.LogWarning("Invalid input. Please enter valid weight and height.");
        }
    }

    //public void CheckSuccessConditions()
    //{
    //    // �� �ٵ�Ÿ���� ���� ������ �����ϴ��� Ȯ��
    //    bool obeseSuccess = (bodyManager.currentBodyType == BodyManager.BodyType.Obese) && (bodyManager.weight <= 70.0f);
    //    bool diabeticSuccess = (bodyManager.currentBodyType == BodyManager.BodyType.Diabetic) && (bodyManager.SkeletalMuscleMass >= 30.0f);
    //    bool athleteSuccess = (bodyManager.currentBodyType == BodyManager.BodyType.Athlete) && (bodyManager.SkeletalMuscleMass >= 40.0f);
    //    bool underweightSuccess = (bodyManager.currentBodyType == BodyManager.BodyType.Underweight) && (bodyManager.weight >= 63.0f);
    //    bool heartSuccess = (bodyManager.heart >= 1);

    //    if (obeseSuccess || diabeticSuccess || athleteSuccess || underweightSuccess || heartSuccess)
    //    {
    //        SceneManager.LoadScene("ScoreScene");
    //    }
    //}
}
