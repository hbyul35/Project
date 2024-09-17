using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text lable;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedChatacter"); // CharacterSelection Class���� ������ ���� ������
        GameObject prefab = characterPrefabs[selectedCharacter];
        prefab.SetActive(true);
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lable.text = prefab.name;
    }

}