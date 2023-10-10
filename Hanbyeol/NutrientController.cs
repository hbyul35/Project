using UnityEngine;

public enum BodyType
{
    Obese,                                               
    Diabetic,                                     
    Athlete,                                         
    Underweight,                                     
    Basic                                                
}

public class BodyManager : MonoBehaviour
{

    public BodyType basicBodyType = BodyType.Basic;

    public int life = 3;
    private float height = 173f;
    private float weight = 74f;

    public int score = 0;

    private void Start()
    {
        SetBodyType(basicBodyType);
    }

    public void SetBodyType(BodyType newBodyType)
    {
        basicBodyType = newBodyType;

        switch (newBodyType)
        {
            case BodyType.Obese:
                life = 3;
                height = 173f;
                weight = 90f;
                break;
            case BodyType.Diabetic:
                life = 3;
                height = 173f;
                weight = 74f;
                break;
            case BodyType.Athlete:
                life = 3;
                height = 173f;
                weight = 74f;
                break;
            case BodyType.Underweight:
                life = 3;
                height = 173f;
                weight = 55f;
                break;
        }
    }
}
