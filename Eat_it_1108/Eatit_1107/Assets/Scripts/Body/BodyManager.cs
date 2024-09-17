using TMPro;
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

    public GameObject[] characterPrefabs;
    public TextMeshProUGUI[] userInformation; // 0.hight 1.weight 3.bmi 4. Muscle_Mass
    private BaseBody selectedBody;

    private void Start()
    {
        float userHeight = PlayerPrefs.GetFloat("Height");
        float userWeight = PlayerPrefs.GetFloat("Weight");
        float userBMI = PlayerPrefs.GetFloat("BMI");

        int selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter");

        switch (selectedCharacter)
        {
            case 0:
                selectedBody = new Obese(userHeight, userWeight, userBMI);
                break;
            case 1:
                selectedBody = new Basic(userHeight, userWeight, userBMI);
                break;
            case 2:
                selectedBody = new Underweight(userHeight, userWeight, userBMI);
                break;
            case 3:
                selectedBody = new Athlete(userHeight, userWeight, userBMI);
                break;
            case 4:
                selectedBody = new Diabetic(userHeight, userWeight, userBMI);
                break;
            default:
                selectedBody = new Basic(userHeight, userWeight, userBMI); // 오버플로우 방지
                break;
        }
        CharacterStatus();
    }

    public void CharacterStatus()
    {
        userInformation[0].text = "Height : " + selectedBody.Height.ToString();
        userInformation[1].text = "Weight : " + selectedBody.Weight.ToString();
        userInformation[2].text = "MuscleMass : " + selectedBody.MuscleMass.ToString();
    }
}


public class BaseBody
{
    private float height = 0f;
    private float weight = 0f;
    private float bmi = 0f;
    private float SkeletalMuscleMass = 0f;

    public float Height
    {
        get { return height; }
        set { height = value; }
    }
    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }
    public float Bmi
    {
        get { return bmi; }
        set { bmi = value; }
    }
    public float MuscleMass
    {
        get { return SkeletalMuscleMass; }
        set { SkeletalMuscleMass = value; }
    }
}

public class Obese : BaseBody
{
    public Obese() { }
    public Obese(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 26f;
    }

    // Athlete 클래스 전용 동작
}

public class Basic : BaseBody
{
    public Basic() { }
    public Basic(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 28f;
    }
}

public class Underweight : BaseBody
{
    public Underweight() { }
    public Underweight(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 26f;
    }
}

public class Athlete : BaseBody
{
    public Athlete() { }
    public Athlete(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 30f;
    }
}

public class Diabetic : BaseBody
{
    public Diabetic() { }
    public Diabetic(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 24f;
    }
}