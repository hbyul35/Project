using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{

    public BodyType character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        DataMgr.Instance.currentBody = character;
    }
}
