using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    TMP_Dropdown resolution;
    [SerializeField]
    TMP_Dropdown windowType;
    [SerializeField]
    Slider sliderSFX;
    [SerializeField]
    Slider sliderBGM;

    bool dontSave = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DismissMenu()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        LoadCurrentSettings();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void LoadCurrentSettings()
    {
        dontSave = true;
        Debug.Log("applying settings to menu");
        sliderBGM.value = PlayerPrefs.GetFloat("bgm", 0.8f);
        sliderSFX.value = PlayerPrefs.GetFloat("sfx", 0.8f);
        string saved_resolution = PlayerPrefs.GetString("resolution", "640 X 480");
        resolution.value = resolution.options.FindIndex(option => option.text == saved_resolution);
        windowType.value = PlayerPrefs.GetInt("fullscreen", 0);
        dontSave = false;

        //ApplyResolution();
        //ApplyAudioChanges();
    }

    public void ApplyResolution()
    {
        var res_choice = resolution.options[resolution.value].text;
        //Debug.Log(res_choice);

        /*
         * int x_index = res_choice.IndexOf("X");
        string width = res_choice.Substring(0, x_index - 1).Trim();
        string height = res_choice.Substring(x_index + 1).Trim();
        */

        //Debug.Log("[" + width + "], [" + height + "]");

        bool isFullscreen = windowType.value == 1;

        /*Screen.SetResolution(int.Parse(width), int.Parse(height), isFullscreen);

        FindObjectOfType<CameraFixer>()?.FixScreen();*/

        GameManager.instance.SetResolution(res_choice, isFullscreen);

        SavePreferences();
    }

    public void ApplyAudioChanges()
    {
        /*mixer.SetFloat("SFX", Mathf.Log10(sliderSFX.value) * 20);
        mixer.SetFloat("BGM", Mathf.Log10(sliderBGM.value) * 20);*/

        GameManager.instance.SetAudio(sliderSFX.value, sliderBGM.value);

        SavePreferences();
    }

    public void SavePreferences()
    {
        if (dontSave)
            return;

        PlayerPrefs.SetString("resolution", Screen.currentResolution.width + " X " + Screen.currentResolution.height);
        PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.SetFloat("sfx", sliderSFX.value);
        PlayerPrefs.SetFloat("bgm", sliderBGM.value);

        Debug.Log("saved preferences!");
    }
}
