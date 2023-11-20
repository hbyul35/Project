using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionCheck : MonoBehaviour
{
    public bool isColliding = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            isColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            isColliding = false;
        }
    }
}
