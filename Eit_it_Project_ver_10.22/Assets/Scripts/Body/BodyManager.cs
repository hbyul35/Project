using UnityEngine;
public class BodyManager : MonoBehaviour
{
    public enum BodyType
    {
        Obese,                                               // 비만
        Basic,                                               // 기본
        Underweight,                                         // 저체중
        Athlete,                                             // 운동 선수
        Diabetic                                             // 당뇨
    }
    public BodyType currentBodyType;


    // 초기 상태 설정
    BodyType basicBodyType = BodyType.Basic;

    private float height = 173f;
    private float weight = 74f;
    private float SkeletalMuscleMass = 29.5f;
    private int heart =3;

    private void Start()
    {
        SetBodyType(basicBodyType);
    }

    // 체질 변경
    public void SetBodyType(BodyType newBodyType)
    {
        basicBodyType = newBodyType;

        // 체질에 따라 초기 상태 설정
        switch (newBodyType)
        {
            case BodyType.Obese:
                height = 173f;
                weight = 90f;
                SkeletalMuscleMass = 32f;
                heart = 3;
                break;
            case BodyType.Diabetic:
                height = 173f;
                weight = 63f;
                SkeletalMuscleMass = 28f;
                heart = 3;
                break;
            case BodyType.Athlete:
                height = 173f;
                weight = 74f;
                SkeletalMuscleMass = 30f;
                heart = 3;
                break;
            case BodyType.Underweight:
                height = 173f;
                weight = 55f;
                SkeletalMuscleMass = 26f;
                heart = 3;
                break;
        }
    }

}