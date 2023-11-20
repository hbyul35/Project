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

    public void TutorialSceneLoad()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void CharacterSelectSceneLoad()
    {
        GameManager.Instance.Calculate_BMI_And_Recommend(); 

        if (GameManager.Instance.Flag)
        {           
            SceneManager.LoadScene("CharacterSelectScene");
            GameManager.Instance.Flag = false; 
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            SceneManager.LoadScene("ScoreScene");
            PlayerPrefs.SetInt("Scene", 1); // Information for the score scene 
        }      
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
