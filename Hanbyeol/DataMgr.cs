using UnityEngine;

public enum BodyType
{
    Obese,                                               // 비만
    Diabetic,                                            // 당뇨
    Athlete,                                             // 운동 선수
    Underweight,                                         // 저체중
    Basic                                                // 기본
}

// 싱글톤 패턴
public class DataMgr : MonoBehaviour
{
    public static DataMgr Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance == null) return;
        DontDestroyOnLoad(gameObject);
    }

    public BodyType currentBody;
}