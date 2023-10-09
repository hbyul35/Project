using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    // 타이틀 화면에서 "Start" 버튼을 누를 때 호출될 함수
    public void StartGame()
    {
        // "InformationScene" 씬으로 이동
        SceneManager.LoadScene("InformationScene");
    }

    // 타이틀 화면에서 "Quit" 버튼을 누를 때 호출될 함수
    public void QuitGame()
    {
        // 게임 종료 (에디터에서는 동작하지 않을 수 있습니다)
        Application.Quit();
    }
}