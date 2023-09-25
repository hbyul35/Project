using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectSceneController : MonoBehaviour {
  private void Start() {
    // PlayerPrefs에서 추천 체질 정보를 읽어옵니다.
    string recommendedPhysique = PlayerPrefs.GetString("RecommendedPhysique");

    // 추천 체질에 해당하는 게임 오브젝트를 활성화합니다.
    GameObject recommendedObject = GameObject.Find(
        recommendedPhysique); // 추천 체질 오브젝트의 이름과 동일하게 설정
    recommendedObject.SetActive(true);
  }
}
