using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public GameObject obeseCharacterPrefab;    // 비만 체질 캐릭터 프리팹
    public GameObject diabeticCharacterPrefab; // 당뇨 체질 캐릭터 프리팹
    public GameObject athleteCharacterPrefab;  // 운동 선수 체질 캐릭터 프리팹
    public GameObject underweightCharacterPrefab; // 저체중 체질 캐릭터 프리팹
    public BodyManager bodyManager; // BodyManager 스크립트 참조

    private void Start()
    {
        // BodyManager를 사용하여 현재 체질을 가져옵니다.
        BodyType currentBodyType = bodyManager.basicBodyType;

        // 현재 체질에 따라 해당 캐릭터를 생성합니다.
        GameObject selectedCharacterPrefab = GetCharacterPrefab(currentBodyType);

        // 캐릭터를 생성하고 위치를 설정합니다.
        Instantiate(selectedCharacterPrefab, Vector3.zero, Quaternion.identity);
    }

    private GameObject GetCharacterPrefab(BodyType bodyType)
    {
        // BodyType에 따라 적절한 프리팹을 반환합니다.
        switch (bodyType)
        {
            case BodyType.Obese:
                return obeseCharacterPrefab;
            case BodyType.Diabetic:
                return diabeticCharacterPrefab;
            case BodyType.Athlete:
                return athleteCharacterPrefab;
            case BodyType.Underweight:
                return underweightCharacterPrefab;
            default:
                return null;
        }
    }
}
