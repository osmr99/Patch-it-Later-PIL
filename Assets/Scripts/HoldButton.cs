using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour
{
    public float progress;
    public Image bar;

    public bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = progress/100;

        if(!clicked && bar.fillAmount >= 1.0f)
        {
            clicked = true;

            GetComponent<Button>().onClick.Invoke();
        }
    }

    public void RestartLevel()
    {
        GameManager.instance.ResetLevel();
    }

    public void BackToMenu()
    {
        GameManager.instance.BackToMenu();
    }
}
