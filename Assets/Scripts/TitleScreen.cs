using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevelSelect()
    {
        GameManager.instance.GoToNextLevel();
    }

    public void OpenSettingsMenu()
    {
        GameManager.instance.OpenSettingsMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
