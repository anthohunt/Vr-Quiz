using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractGameObjectInfo : MonoBehaviour
{

    public GameObject test;
    
    // Start is called before the first frame update
    void Update()
    {
       Debug.Log( test.name);
    }
}
