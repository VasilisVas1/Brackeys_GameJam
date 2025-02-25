using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuSettings : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    void Start()
    {
        masterVolumeSlider.value = AudioListener.volume;

        // Setup resolution dropdown
        resolutionDropdown.ClearOptions();
        foreach (Resolution res in Screen.resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(res.ToString()));
        }

        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        // Setup quality dropdown
        qualityDropdown.ClearOptions();
        foreach (string quality in QualitySettings.names)
        {
            qualityDropdown.options.Add(new Dropdown.OptionData(quality));
        }

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreen;

        LoadSettings();
    }

    public void UpdateMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void ChangeResolution(int index)
    {
        Resolution newResolution = Screen.resolutions[index];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityIndex", index);
        PlayerPrefs.Save();
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume");

        if (PlayerPrefs.HasKey("ResolutionIndex"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");

        if (PlayerPrefs.HasKey("QualityIndex"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualityIndex");

        if (PlayerPrefs.HasKey("Fullscreen"))
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == currentResolution.width &&
                Screen.resolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        PlayerPrefs.Save();
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DemoDay"); // Replace with your actual game scene name
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); // Stops Play Mode in the Editor
        #else
            Application.Quit(); // Closes the game in a built version
        #endif

    }
}
