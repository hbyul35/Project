using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text lable;
    int selectedCharacter = 0;

    public TMP_Text[] userInformation = new TMP_Text[3]; // museclemass , bmi, bloodsugar
    private BodyManager BodyManagerInstance;

    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter");

        Debug.Log("·Îµå¾À : " + selectedCharacter);

        GameObject prefab = characterPrefabs[selectedCharacter];
        prefab.SetActive(true);
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lable.text = prefab.name;

        BodyManagerInstance = FindObjectOfType<BodyManager>();
        userInformation[0].text = "MuscleMass : " + PlayerPrefs.GetFloat("MuscleMass").ToString();
        userInformation[1].text = "BMI : " + PlayerPrefs.GetFloat("Bmi").ToString("F2");
        if (selectedCharacter == 4)
        {
            userInformation[2].text = "BloodSugar : " + PlayerPrefs.GetFloat("BloodSugar").ToString();
        }
    }

    private void Update()
    {
        if (BodyManagerInstance.isCollision)
        {
            userInformation[0].text = "MuscleMass : " + PlayerPrefs.GetFloat("AfterMuscleMass").ToString();
            userInformation[1].text = "BMI : " + PlayerPrefs.GetFloat("AfterBmi").ToString("F2");

            if(selectedCharacter == 4)
            {
                userInformation[2].text = "BloodSugar : " + PlayerPrefs.GetFloat("AfterBloodSugar").ToString();
            }
        }
    }
}