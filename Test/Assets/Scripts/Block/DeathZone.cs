using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private float reStartTime = 0;
    public bool deathCount = false;

    void Start()
    {

    }
    void Update()
    {
        if (deathCount)
        {
            if (reStartTime == 0f)
            {
                reStartTime = Time.time;
            }
            if (Time.time - reStartTime >= 2f)
            {
                deathCount = false;
                Debug.Log("DeathZone You Died");

                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player"))
        {
            deathCount = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        reStartTime = 0f;
    }
}