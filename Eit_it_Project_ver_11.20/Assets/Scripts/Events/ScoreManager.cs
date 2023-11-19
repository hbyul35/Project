using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text[] resultsUI;
    public GameObject ChallengeUI;

    float[] beforeStats = new float[4]; // Initial stats
    float[] afterStats = new float[4];  // Changed Stats

    int selectedCharacter = 0;
    void Start()
    {
        LoadPlayerStats("Weight", "MuscleMass", "Bmi", "BloodSugar", beforeStats);
        LoadPlayerStats("afterWeight", "AfterMuscleMass", "AfterBmi", "AfterBloodSugar", afterStats);

        int sceneIndex = PlayerPrefs.GetInt("Scene");
        selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter"); // Checking for diabetics

        if (sceneIndex == 1)
        {
            DisplayResult(selectedCharacter);
            PlayerPrefs.DeleteKey("Scene");

            switch (selectedCharacter)
            {
                case 0:
                case 2:
                    if (afterStats[2] >= 18.5f && afterStats[2] <= 24.9f) // bmi
                    {
                        ChallengeUI.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                    }
                    else
                        ChallengeUI.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                    break;
                case 1://Basic type case
                    break;
                case 3:
                    if (afterStats[1] >= 40f) // musclemass
                    {
                        ChallengeUI.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                    }
                    else
                        ChallengeUI.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                    break;
                case 4:
                    if (afterStats[3] <= 120) //bloodsugar
                    {
                        ChallengeUI.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                    }
                    else
                        ChallengeUI.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                    break;
            }
        }
    }

    void LoadPlayerStats(string weightKey, string muscleMassKey, string bmiKey, string bloodSugarKey, float[] stats)
    {
        stats[0] = PlayerPrefs.GetFloat(weightKey);
        stats[1] = PlayerPrefs.GetFloat(muscleMassKey);
        stats[2] = PlayerPrefs.GetFloat(bmiKey);
        stats[3] = PlayerPrefs.GetFloat(bloodSugarKey);
    }

    void DisplayResult(int selectedCharacter)
    {
        resultsUI[0].text = GetResultText("Weight", beforeStats[0], afterStats[0]);
        resultsUI[1].text = GetResultText("MuscleMass", beforeStats[1], afterStats[1]);
        resultsUI[2].text = GetResultText("BMI", beforeStats[2], afterStats[2], "F2");
        resultsUI[4].text = "Score : " + PlayerPrefs.GetInt("Score").ToString();

        if (selectedCharacter == 4)
        {
            resultsUI[3].text = GetResultText("BloodSugar", beforeStats[3], afterStats[3]);
        }
    }

    string GetResultText(string statName, float before, float after, string format = "")
    {
        string formattedBefore = before.ToString(format);
        string formattedAfter = after.ToString(format);
        return $"[ {statName} ]\nBfore : {formattedBefore}     ==>      After : {formattedAfter}";
    }
}