using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public Image qWhiteImage;
    public Image spaceWhiteImage;
    public Image eWhiteImage;
    public Image sWhiteImage;
    public Image qBlackImage;
    public Image spaceBlackImage;
    public Image eBlackImage;
    public Image sBlackImage;
    public Image shiftWhiteImage;
    public Image shiftBlackImage;
    public Image gWhiteImage;
    public Image gBlackImage;
    public Image wWhiteImage;
    public Image wBlackImage;
    public TMP_Text press;

    private int tutorialStep;

    void Start()
    {
        tutorialStep = 0;
        qBlackImage.enabled = false;
        wBlackImage.enabled = false;
        wWhiteImage.enabled = false;
        spaceBlackImage.enabled = false;
        eBlackImage.enabled = false;
        sBlackImage.enabled = false;
        shiftWhiteImage.enabled = false;
        gBlackImage.enabled = false;
        qWhiteImage.enabled = false;
        spaceWhiteImage.enabled = false;
        eWhiteImage.enabled = false;
        sWhiteImage.enabled = false;
        shiftBlackImage.enabled = false;
        gWhiteImage.enabled = false;
        qWhiteImage.enabled = true;

        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            switch (tutorialStep)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qWhiteImage.enabled = false;
                        qBlackImage.enabled = true;
                        yield return new WaitForSeconds(1f);
                        qBlackImage.enabled = false;
                        eWhiteImage.enabled = true;
                        tutorialStep++;

                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        eWhiteImage.enabled = false;
                        eBlackImage.enabled = true;
                        yield return new WaitForSeconds(1f);
                        eBlackImage.enabled = false;
                        wWhiteImage.enabled = true;
                        shiftWhiteImage.enabled = true;
                        tutorialStep++;

                    }
                    break;
                case 2:
                    if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        wWhiteImage.enabled = false;
                        shiftWhiteImage.enabled = false;
                        wBlackImage.enabled = true;
                        shiftBlackImage.enabled = true;
                        yield return new WaitForSeconds(1f);
                        shiftBlackImage.enabled = false;
                        wBlackImage.enabled = false;
                        sWhiteImage.enabled = true;
                        spaceWhiteImage.enabled= true;
                        tutorialStep++;
                        
                    }
                    break;
                case 3:
                    if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
                    {
                        sWhiteImage.enabled = false;
                        spaceWhiteImage.enabled = false;
                        sBlackImage.enabled = true;
                        spaceBlackImage.enabled = true;
                        yield return new WaitForSeconds(1f);
                        sBlackImage.enabled = false;
                        spaceBlackImage.enabled = false;
                        sWhiteImage.enabled = true;
                        gWhiteImage.enabled = true;
                        tutorialStep++;
                    }
                    break;
                case 4:
                    if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.S))
                    {
                        sWhiteImage.enabled = false;
                        gWhiteImage.enabled = false;
                        sBlackImage.enabled = true;
                        gBlackImage.enabled = true;
                        yield return new WaitForSeconds(1f);
                        sBlackImage.enabled = false;
                        gBlackImage.enabled = false;
                        press.enabled = false;
                    }
                    break;
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TutorialGoal")
        {
            SceneManager.LoadScene("UserInformationScene");
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class TutorialManager : MonoBehaviour
//{
//    public Image qBlackImage;
//    public Image spaceBlackImage;
//    public Image eBlackImage;
//    public Image sBlackImage;
//    public Image qWhiteImage;
//    public Image spaceWhiteImage;
//    public Image eWhiteImage;
//    public Image sWhiteImage;
//    public Image shiftBlackImage;
//    public Image shiftWhiteImage;
//    public Image gBlackImage;
//    public Image gWhiteImage;

//    private int TutorialStep;

//    private void Start()
//    {
//        TutorialStep = 0;
//        qBlackImage.enabled = true;
//        shiftBlackImage.enabled = false;
//        shiftWhiteImage.enabled = false;
//        gBlackImage.enabled = false;
//        gWhiteImage.enabled = false;
//        spaceBlackImage.enabled = false;
//        eBlackImage.enabled = false;
//        sBlackImage.enabled = false;
//        qWhiteImage.enabled = false;
//        spaceWhiteImage.enabled = false;
//        eWhiteImage.enabled = false;
//        sWhiteImage.enabled = false;

//        StartCoroutine(TutorialStart());
//    }

//    IEnumerator TutorialStart()
//    {
//        yield return new WaitForSeconds(1f);

//        while (true)
//        {
//            if (TutorialStep == 0)
//            {
//                Step1();
//                yield return new WaitWhile(() => TutorialStep < 1);
//            }
//            else if (TutorialStep == 1)
//            {
//                Step2();
//                yield return new WaitWhile(() => TutorialStep < 2);
//            }
//            else if (TutorialStep == 2)
//            {
//                Step3();
//                yield return new WaitWhile(() => TutorialStep < 3);
//            }
//            else if (TutorialStep == 3)
//            {
//                Step4();
//                yield return new WaitWhile(() => TutorialStep < 4);
//            }
//            else if (TutorialStep == 4)
//            {
//                Step5();
//                yield return new WaitWhile(() => TutorialStep < 5);
//            }

//            yield return null;
//        }
//    }

//    private void Step1()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            TutorialStep = 1;
//            qBlackImage.enabled = false;
//            qWhiteImage.enabled = true;
//            Step2();
//        }
//    }

//    private void Step2()
//    {
//        qWhiteImage.enabled = false;
//        eBlackImage.enabled = true;
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            TutorialStep = 4;
//            eBlackImage.enabled = false;
//            eWhiteImage.enabled = true;
//            Step3();
//        }
//    }

//    private void Step3()
//    {
//        eWhiteImage.enabled = false;
//        shiftBlackImage.enabled = true;
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            TutorialStep = 3;
//            shiftBlackImage.enabled = false;
//            shiftWhiteImage.enabled = true;
//            Step4();
//        }
//    }

//    private void Step4()
//    {
//        shiftWhiteImage.enabled = false;
//        sBlackImage.enabled = true;
//        spaceBlackImage.enabled = true;
//        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
//        {
//            TutorialStep = 4;
//            spaceWhiteImage.enabled = false;
//            sBlackImage.enabled = false;
//            sWhiteImage.enabled = true;
//            spaceBlackImage.enabled = false;
//            spaceWhiteImage.enabled = true;
//            Step5();
//        }
//    }

//    private void Step5()
//    {
//        sWhiteImage.enabled = false;
//        spaceWhiteImage.enabled = false;
//        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.S))
//        {
//            TutorialStep = 5;
//            gBlackImage.enabled = false;
//            gWhiteImage.enabled = true;
//            sBlackImage.enabled = false;
//            sWhiteImage.enabled = true;

//            MoveToUserInformationScene();
//        }
//    }

//    private void MoveToUserInformationScene()
//    {
//        SceneManager.LoadScene("UserInformationScene");
//    }
//}