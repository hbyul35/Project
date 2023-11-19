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

    public TextMeshProUGUI[] scoreText;

    public static GameManager Instance;
    private int score;
    public bool flag = false;
    private BodyManager.BodyType recommendedPhysique; 

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
        //scoreText[0].text = "Score: " + score.ToString();
    }
    public void UpdateScore(int changeScore)
    {
        score += changeScore;
        scoreText[0].text = "Score: " + score.ToString();
        PlayerPrefs.SetInt("Score", score);
    }

    public void Calculate_BMI_And_Recommend() 
    {
        float weight, height;
        
        if (float.TryParse(weightInputField.text, out weight) && float.TryParse(heightInputField.text, out height))
        {
            height /= 100; 
            float bmi = weight / (height * height);
            PlayerPrefs.SetFloat("Height", height);
            PlayerPrefs.SetFloat("Weight", weight);
            PlayerPrefs.SetFloat("Bmi", bmi);
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

            PlayerPrefs.SetString("RecommendedPhysique", recommendedPhysique.ToString());
            PlayerPrefs.Save();

            flag = true;
        }
        else
        {
            Debug.LogWarning("Invalid input. Please enter valid weight and height.");
        }
    }
}
