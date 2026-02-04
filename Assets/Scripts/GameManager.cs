using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject settingsMenu;

    public float volumeSFX = 1.0f;
    public float volumeBGM = 1.0f;

    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        LoadCurrentSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettingsMenu()
    {
        /*var existingMenu = FindObjectOfType<SettingsMenu>();
        if (existingMenu != null)
        {
            existingMenu.gameObject.SetActive(true);
            return;
        }*/
            
        Instantiate(settingsMenu);
    }

    public void ResetLevel()
    {
        StartCoroutine(RestartLevel());
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartNextLevelCoroutine()
    {
        StartCoroutine("NextLevel");
    }

    public void PrepNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        var scene = SceneManager.LoadSceneAsync(nextLevelIndex);
        scene.allowSceneActivation = false;
    }

    public IEnumerator NextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        AsyncOperation scene;

        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            scene = SceneManager.LoadSceneAsync(0);
        }
        else
        {
            scene = SceneManager.LoadSceneAsync(nextLevelIndex);
        }

        scene.allowSceneActivation = false;

        var fade = FindObjectOfType<FadeCanvas>();

        fade.SetDesiredAlpha(3);

        while(fade.Fading)
        {
            //Debug.Log("fading");
            yield return new WaitForSeconds(0.1f);
        }

        scene.allowSceneActivation = true;
    }

    IEnumerator RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        var fade = FindObjectOfType<FadeCanvas>();

        fade.SetDesiredAlpha(1);

        //while (fade.Fading)
        {
            //Debug.Log("fading");
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("restarting");
        //scene.allowSceneActivation = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if(nextLevelIndex > SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        
    }

    public void LoadCurrentSettings()
    {
        Debug.Log("Loading saved settings");
        var bgm = PlayerPrefs.GetFloat("bgm", 0.8f);
        var sfx = PlayerPrefs.GetFloat("sfx", 0.8f);
        string saved_resolution = PlayerPrefs.GetString("resolution", "640 X 480");
        //resolution.value = resolution.options.FindIndex(option => option.text == saved_resolution);
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 0) == 1? true : false;

        SetResolution(saved_resolution, fullscreen);
        SetAudio(sfx, bgm);
    }

    public void SetResolution(string res, bool isFullscreen)
    {
        int x_index = res.IndexOf("X");
        string width = res.Substring(0, x_index - 1).Trim();
        string height = res.Substring(x_index + 1).Trim();

        Screen.SetResolution(int.Parse(width), int.Parse(height), isFullscreen);

        FindObjectOfType<CameraFixer>()?.FixScreen();
    }

    public void SetAudio(float sfx, float bgm)
    {
        //Debug.Log("SETTING AUDIO");
        mixer.SetFloat("SFX", /*Mathf.Log10(sfx) * 20*/ (- 60 + sfx * 80)); // -80 + (sfx * 80)
        mixer.SetFloat("BGM", (-60 + bgm * 80));
    }
}
