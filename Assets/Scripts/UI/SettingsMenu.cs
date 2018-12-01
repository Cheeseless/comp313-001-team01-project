using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Dropdown graphicsDropdown;
    Resolution[] resolutions;

    public GameObject settingsMenuUI;
    private GameObject pauseMenuUI;

    // Use this for initialization
    void Start () {

        pauseMenuUI = GameObject.Find("Canvas").GetComponent<PauseMenu>().pauseMenuUI;

        //resolutionDropdown = GameObject.Find("ResolutionDropdown").GetComponent<Dropdown>();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        int currentQuality = QualitySettings.GetQualityLevel();
        //graphicsDropdown = GameObject.Find("GraphicsDropdown").GetComponent<Dropdown>();
        graphicsDropdown.value = currentQuality;

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log(Screen.fullScreen);
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
