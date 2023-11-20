using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public ScrollRect gameTutorialsInstance;
    void Start()
    {

    }
    void Update()
    {

    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void UserInformationSceneLoad()
    {
        SceneManager.LoadScene("UserInformationScene");
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
    public void SwitchingToCharacterLoadScene()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
    public void TutorialSceneLoad()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void GameTutorialsUI()
    {
        gameTutorialsInstance.gameObject.SetActive(true);
    }
}
