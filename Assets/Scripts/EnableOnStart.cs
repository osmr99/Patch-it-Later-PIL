using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField]
    GameObject[] objects;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in objects)
            g.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
