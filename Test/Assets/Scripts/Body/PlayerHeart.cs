using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHeart : MonoBehaviour
{
    private GameObject[] targetCubes = new GameObject[2];
    private float collisionStartTime = 0;
    public bool deathCount = false;
    void Start()
    {
    }

    void Update()
    {
        Vector3 forwardRay = new Vector3(transform.localScale.x/2.2f, transform.localScale.y/2.2f, 0.2f);
        Vector3 upRay = new Vector3(transform.localScale.x/2.2f, 0.2f, transform.localScale.z/2.2f);
        float castDistance = 1f;
        RaycastHit[] hit = new RaycastHit[2];

        if (Physics.BoxCast(transform.position, forwardRay * 0.5f, transform.forward, out hit[0], Quaternion.identity, castDistance))
        {
            targetCubes[0] = hit[0].collider.gameObject;
        }
        if (Physics.BoxCast(transform.position, upRay * 0.5f, transform.up, out hit[1], Quaternion.identity, castDistance))
        {
            targetCubes[1] = hit[1].collider.gameObject;
        }

        if (deathCount)
        {
            if (collisionStartTime == 0f)
            {
                collisionStartTime = Time.time;
            }

            if (Time.time - collisionStartTime >= 2f)
            {
                deathCount = false;
                Debug.Log("PlayerHeart You Died");

                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == targetCubes[0] || other.gameObject == targetCubes[1])
        {
            deathCount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetCubes[0] || other.gameObject == targetCubes[1])
        {
            collisionStartTime = 0f;
        }
    }
}
