using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadCharacter : MonoBehaviour
{
    [Header("Load Characters")]
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text lable;

    [Header("Character Status UI")]
    public TMP_Text[] userInformation = new TMP_Text[3]; // museclemass , bmi, bloodsugar
    private BodyManager BodyManagerInstance;
    private int selectedCharacter = 0;
    private bool muscleMassUpdated = false;

    [Header("Character Life")]
    public int maxHeart = 3;
    private int currentHeart;
    public bool isDeath = false;
    private float delayTime = 0f;
    public Image heartImagePrefab;
    public Sprite heartSprite;
    public Transform heartParent;
    private PlayerHeart playerHeartInstance;
    private DeathZone deathZoneInstance;

    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter");

        Debug.Log("Character Number : " + selectedCharacter); //Checking the select character

        GameObject prefab = characterPrefabs[selectedCharacter];
        prefab.SetActive(true);
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lable.text = prefab.name;

        //Initial Status
        userInformation[0].text = "MuscleMass : " + PlayerPrefs.GetFloat("MuscleMass").ToString();
        userInformation[1].text = "BMI : " + PlayerPrefs.GetFloat("Bmi").ToString("F2");
        if (selectedCharacter == 4)
        {
            userInformation[2].text = "BloodSugar : " + PlayerPrefs.GetFloat("BloodSugar").ToString();
        }

        BodyManagerInstance = FindObjectOfType<BodyManager>();

        playerHeartInstance = FindObjectOfType<PlayerHeart>();
        deathZoneInstance = FindObjectOfType<DeathZone>();

        currentHeart = PlayerPrefs.GetInt("PlayerCurrentHealth", maxHeart);
        HeartsCount(); 
    }

    private void Update()
    {
        UpdateStatus(); // playerStatusUI
        UpdateHearts(); // playerLifeUI
        GameOver();

        if(Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    void UpdateStatus()
    {
        if (!muscleMassUpdated)
        {
            userInformation[0].text = "MuscleMass : " + PlayerPrefs.GetFloat("MuscleMass").ToString();
        }

        if (BodyManagerInstance.isCollision)
        {
            userInformation[0].text = "MuscleMass : " + PlayerPrefs.GetFloat("AfterMuscleMass").ToString();
            userInformation[1].text = "BMI : " + PlayerPrefs.GetFloat("AfterBmi").ToString("F2");

            if (selectedCharacter == 4)
            {
                userInformation[2].text = "BloodSugar : " + PlayerPrefs.GetFloat("AfterBloodSugar").ToString();
            }

            muscleMassUpdated = true;
        }
    }

    public void UpdateHearts()
    {
        if (!isDeath && (playerHeartInstance.deathCount || deathZoneInstance.deathCount))
        {
            isDeath = true;

            if (currentHeart > 0)
            {
                currentHeart--;

                for (int i = 0; i <= currentHeart; i++) // Change the heart image
                {
                    Debug.Log("루프 내부"+currentHeart);
                    //float x = i * 60 - (maxHealth*50 - 50); //x 0 - 100 = -100 / 60 - 100 = -40 / 120 - 100 = 20 // 순차
                    float x = (maxHeart - i - 1) * 60 - (maxHeart * 50 - 50); //x 120 - 100 = 20 / 60 - 100 = -40 / 0 - 100 = 20 // 역순
                    float y = 0f;
                    float z = 0f;

                    Image heartImage = Instantiate(heartImagePrefab);
                    heartImage.sprite = heartSprite;
                    heartImage.transform.position = new Vector3(x, y, z);
                    heartImage.transform.SetParent(heartParent, false);

                    PlayerPrefs.SetInt("PlayerCurrentHealth", currentHeart);

                    return;
                }
            }
        }
    }

    void HeartsCount()
    {
        for (int i = 0; i < currentHeart; i++) // Setting the heart position 
        {
            float x = (maxHeart - i - 1) * 60 - (maxHeart * 50 - 50);
            float y = 0f;
            float z = 0f;

            Image heartImage = Instantiate(heartImagePrefab);
            heartImage.transform.position = new Vector3(x, y, z);
            heartImage.transform.SetParent(heartParent, false);
        }
    }

    void GameOver()
    {
        if (currentHeart <= 0)
        {
            PlayerPrefs.DeleteKey("PlayerCurrentHealth");

            playerHeartInstance.deathCount = false;
            deathZoneInstance.deathCount = false;

            if (delayTime == 0f)
            {
                delayTime = Time.time;
            }
            if (Time.time - delayTime >= 2f)
            {
                SceneManager.LoadScene("ScoreScene");
                PlayerPrefs.SetInt("Scene", 1);
            }
        }
    }
}