using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text itemCount;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        itemCount.text = text;

        if (text == "0")
            Disable();
    }    

    public void Enable()
    {

    }

    public void Disable()
    {
        GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
    }
}
