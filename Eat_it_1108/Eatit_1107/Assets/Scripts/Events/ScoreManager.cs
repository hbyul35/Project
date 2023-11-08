using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        //scoreText = GameObject.Find("ScoreText").GetComponent<Text>(); 
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        scoreText.text = "Score: " + score;
    }
}