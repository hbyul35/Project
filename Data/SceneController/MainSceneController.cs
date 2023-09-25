using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    private void Start()
    {
        // PlayerPrefs에서 선택한 체질 정보를 읽어옴
        string selectedPhysique = PlayerPrefs.GetString("SelectedPhysique");

        // 선택한 체질에 따라 캐릭터를 생성하고 초기화
        switch (selectedPhysique)
        {
            case "Underweight":
                // Underweight 체질에 해당하는 캐릭터 생성 및 초기화
                break;
            case "Obese":
                // Obese 체질에 해당하는 캐릭터 생성 및 초기화
                break;
            case "Athlete":
                // Athlete 체질에 해당하는 캐릭터 생성 및 초기화
                break;
            case "Diabetic":
                // Athlete 체질에 해당하는 캐릭터 생성 및 초기화
                break;
            default:
                // 기본적으로 어떤 캐릭터도 생성하지 않음 또는 기본 캐릭터 생성
                break;
        }
    }
}
