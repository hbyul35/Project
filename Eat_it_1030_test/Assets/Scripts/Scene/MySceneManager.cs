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
        GameManager.Instance.Calculate_BMI_And_Recommend(); // GameManager�� ����� ������ �Է���

        if (GameManager.Instance.Flag)
        {           
            SceneManager.LoadScene("CharacterSelectScene");
            GameManager.Instance.Flag = false; // ��� �� ��ȯ
        }        
    }
    //  임시
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            SceneManager.LoadScene("ScoreScene");
        }     
    }
    public void TitleSceneLoad()
    {
        SceneManager.LoadScene("TitleScene");
    }



    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
