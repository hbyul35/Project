using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void UserInformationSceneLoad()
    {
        SceneManager.LoadScene("UserInformationScene");
    }
    public void CharacterSelectSceneLoad()
    {
        GameManager.Instance.Calculate_BMI_And_Recommend(); // GameManager의 사용자 정보를 입력함

        if (GameManager.Instance.Flag)
        {           
            SceneManager.LoadScene("CharacterSelectScene");
            GameManager.Instance.Flag = false; // 사용 후 반환
        }        
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
