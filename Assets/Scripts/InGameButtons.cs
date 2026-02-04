using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameButtons : MonoBehaviour
{
    [SerializeField]
    HoldButton restartButton;
    [SerializeField]
    HoldButton quitButton;

    [SerializeField]
    float buttonHoldSpeed = 1;

    [SerializeField]
    InputActionAsset controls;
    InputActionMap _actionMap;

    InputAction restart;
    InputAction quit;

    bool holdRestart = false;
    bool holdQuit = false;

    // Start is called before the first frame update
    void Start()
    {
        _actionMap = controls.FindActionMap("Player");
        restart = _actionMap.FindAction("Restart");
        restart.canceled += Restart_cancelled;
        restart.performed += Restart_started;

        quit = _actionMap.FindAction("Quit");
        quit.canceled += Quit_cancelled;
        quit.performed += Quit_started;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        holdQuit = true;
    }

    private void Quit_cancelled(InputAction.CallbackContext obj)
    {
        holdQuit = false;
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        holdRestart = true;
    }

    private void Restart_cancelled(InputAction.CallbackContext obj)
    {
        //restartButton.progress = 0;
        holdRestart = false;
    }

    public void OpenSettings()
    {
        GameManager.instance.OpenSettingsMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if(holdRestart)
            restartButton.progress += buttonHoldSpeed * Time.deltaTime;
        else if(restartButton.progress > 0 && !restartButton.clicked)
            restartButton.progress -= buttonHoldSpeed * 2 * Time.deltaTime;

        if (holdQuit)
            quitButton.progress += buttonHoldSpeed * Time.deltaTime;
        else if (quitButton.progress > 0 && !quitButton.clicked)
            quitButton.progress -= buttonHoldSpeed * 2 * Time.deltaTime;
    }
}
