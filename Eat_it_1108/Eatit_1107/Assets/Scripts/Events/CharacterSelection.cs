using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public TextMeshProUGUI tip;
    public Image RecommendedImg;

    private int selectedCharacter = 0;
    private int Recommendedpick = 0; //추천값 저장-> 피봇역할

    private void Start()
    {
        string recommendedPhysique = PlayerPrefs.GetString("RecommendedPhysique");
        Debug.Log(recommendedPhysique);
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].name == recommendedPhysique)
            {
                characters[i].SetActive(true);
                RecommendedImg.gameObject.SetActive(true);
                selectedCharacter = i;
                Recommendedpick = i;
            }
        }
    }
    private void Update()
    {
        if(characters[Recommendedpick] != characters[selectedCharacter])
        {
            RecommendedImg.gameObject.SetActive(false);
        }
        else
            RecommendedImg.gameObject.SetActive(true);

        ToolTip();
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = characters.Length - 1;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void ToolTip()
    {
        switch (selectedCharacter)
        {
            case 0:
                tip.text = "Obese Tip";
                break;
            case 1:
                tip.text = "Basic Tip";
                break;
            case 2:
                tip.text = "Underweight Tip";
                break;
            case 3:
                tip.text = "Athlete Tip";
                break;
            case 4:
                tip.text = "Diabetic Tip";
                break;
        }
    }

    public void MainSceneLoad()
    {
        PlayerPrefs.SetInt("SelectedChatacter", selectedCharacter);
        SceneManager.LoadScene("MainScene");
    }
}
