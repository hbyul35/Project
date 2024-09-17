using TMPro;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GameManager : MonoBehaviour
{
    [Header("User Body Information")]
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    public TMP_Dropdown purposeDropdown;

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
    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        // ���� ���濡 ���� ó�� ����
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

}
