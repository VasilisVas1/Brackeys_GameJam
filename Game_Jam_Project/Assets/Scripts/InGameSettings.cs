using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public GameObject settingsPanel;

    private bool isPaused = false;

    void Start()
    {
        LoadSettings();
        LockCursor(); // Lock the cursor at the start of the game

        // Add listeners to UI elements
        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        qualityDropdown.onValueChanged.AddListener(ChangeQuality);
        fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
    }

    void Update()
    {
        // Open/Close settings panel with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    private void LoadSettings()
    {
        // Load and apply Master Volume
        if (PlayerPrefs.HasKey("MasterVolume"))
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        AudioListener.volume = masterVolumeSlider.value;

        // Load and apply Resolution
        resolutionDropdown.ClearOptions();
        foreach (Resolution res in Screen.resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(res.ToString()));
        }
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            resolutionDropdown.value = resolutionIndex;
            ApplyResolution(resolutionIndex);
        }

        // Load and apply Quality Settings
        qualityDropdown.ClearOptions();
        foreach (string quality in QualitySettings.names)
        {
            qualityDropdown.options.Add(new Dropdown.OptionData(quality));
        }
        if (PlayerPrefs.HasKey("QualityIndex"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualityIndex");
            qualityDropdown.value = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        // Load and apply Fullscreen Toggle
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
    }

    public void UpdateMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void ChangeResolution(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int index)
    {
        if (index >= 0 && index < Screen.resolutions.Length)
        {
            Resolution newResolution = Screen.resolutions[index];
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        }
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

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        UnlockCursor();
        isPaused = true;


        // Disable camera movement
        FindObjectOfType<FirstPersonController>().isPaused = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        LockCursor();
        isPaused = false;
            FindObjectOfType<FirstPersonController>().isPaused = false;

    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running when returning to main menu
        SceneManager.LoadScene("MainMenu"); // Change to your actual main menu scene name
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
