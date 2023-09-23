using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectSceneController : MonoBehaviour
{
    public Text recommendedPhysiqueText;

    void Start()
    {
        // PlayerPrefs에서 추천 체질 정보를 가져옵니다.
        string recommendedPhysique = PlayerPrefs.GetString("RecommendedPhysique", "Normal");

        // 가져온 정보를 UI Text에 표시합니다.
        recommendedPhysiqueText.text = "Recommended Physique: " + recommendedPhysique;
    }
}