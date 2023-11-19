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

    int selectedCharacter = 0;

    public BaseBody selectedBody;
    float userHeight, userWeight, userBMI;

    public bool isCollision = false;

    public void Start()
    {
        userHeight = PlayerPrefs.GetFloat("Height");
        userWeight = PlayerPrefs.GetFloat("Weight");
        userBMI = PlayerPrefs.GetFloat("Bmi");

        selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter");

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
        PlayerPrefs.SetFloat("MuscleMass", selectedBody.MuscleMass);
    }

    private void OnTriggerEnter(Collider other)
    {
        isCollision = true;
        if (other.CompareTag("Carbohydrates") || other.CompareTag("Fat"))
        {
            selectedBody.Weight += 1f;
        }
        else if (other.CompareTag("Fiber"))
        {
            selectedBody.Weight -= 1f;
            selectedBody.BloodSugar -= 1f;
        }
        else if (other.CompareTag("Sugar"))
        {
            selectedBody.Weight += 1f;
            selectedBody.BloodSugar += 1f;
        }
        else if (other.CompareTag("Protein"))
        {
            selectedBody.MuscleMass += 1f;
        }

        // Update BMI
        float changeBMI = selectedBody.Weight / (selectedBody.Height * selectedBody.Height);
        selectedBody.Bmi = changeBMI;

        // Information for the score scene 
        PlayerPrefs.SetFloat("afterWeight", selectedBody.Weight);
        PlayerPrefs.SetFloat("AfterBmi", selectedBody.Bmi);
        PlayerPrefs.SetFloat("AfterMuscleMass", selectedBody.MuscleMass);
        PlayerPrefs.SetFloat("AfterBloodSugar", selectedBody.BloodSugar);
    }
}


public class BaseBody
{
    private float height = 0f;
    private float weight = 0f;
    private float bmi = 0f;
    private float SkeletalMuscleMass = 0f;
    private float bloodsugar = 0f;

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
    public float BloodSugar
    {
        get { return bloodsugar; }
        set { bloodsugar = value; }
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
}

public class Basic : BaseBody
{
    public Basic() { }
    public Basic(float height, float weight, float bmi)
    {
        Height = height;
        Weight = weight;
        Bmi    = bmi;
        MuscleMass = 27f;
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
        MuscleMass = 25f;
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
        BloodSugar = 135f;

        PlayerPrefs.SetFloat("BloodSugar", BloodSugar);
    }
}