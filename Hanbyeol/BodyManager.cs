﻿using UnityEngine;


public class BodyManager : MonoBehaviour
{
    // 초기 상태 설정
    public BodyType basicBodyType = BodyType.Basic;

    private float height = 173f;
    private float weight = 74f;

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
                break;
            case BodyType.Diabetic:
                height = 173f;
                weight = 74f;
                break;
            case BodyType.Athlete:
                height = 173f;
                weight = 74f;
                break;
            case BodyType.Underweight:
                height = 173f;
                weight = 55f;
                break;
        }
    }

}