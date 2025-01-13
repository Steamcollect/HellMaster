using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    //[Header("Settings")]

    bool isOpen = false;

    [Header("References")]
    [SerializeField] GameObject settingsPanel;
    [SerializeField] AudioMixer mainMixer;

    [Space(10)]
    // RSO
    [SerializeField] RSO_MouseSensitivityMultiplier rsoMouseSensitivity;
    // RSF
    // RSP

    //[Header("Input")]
    [Header("Output")]
    [SerializeField] RSE_SaveData rseSaveData;

    [Header("UI Elements")]
    [SerializeField] UnityEngine.UI.Slider musicSlider;
    [SerializeField] TMP_Text musicPercentageText;
    [SerializeField] UnityEngine.UI.Slider soundSlider;
    [SerializeField] TMP_Text soundPercentageText;
    [SerializeField] UnityEngine.UI.Slider mouseSensitivitySlider;
    [SerializeField] TMP_Text mouseSensitivityPercentageText;
    [SerializeField] UnityEngine.UI.Toggle fullScreenToggle;

    [SerializeField] RSO_ContentSaved rsoContentSaved;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            Hide();
        }
    }

    public void MusicSlider(float value)
    {
        mainMixer.SetFloat("Music", LinearToDecibel(value));
        rsoContentSaved.Value.musicVolume = value;
        UpdatePercentageText(musicSlider, musicPercentageText, musicSlider.minValue, musicSlider.maxValue);
    }

    public void SoundSlider(float value)
    {
        mainMixer.SetFloat("Sound", LinearToDecibel(value));
        rsoContentSaved.Value.soundVolume = value;
        UpdatePercentageText(soundSlider, soundPercentageText, soundSlider.minValue, soundSlider.maxValue);
    }

    public void FullScreenToggle(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        rsoContentSaved.Value.isFullScreen = isFullScreen;
    }

    public void MouseSensitivitySlider(float value)
    {
        rsoMouseSensitivity.Value = value;
        rsoContentSaved.Value.mouseSensitivity = value;
        UpdatePercentageText(mouseSensitivitySlider, mouseSensitivityPercentageText, mouseSensitivitySlider.minValue, mouseSensitivitySlider.maxValue);
    }

    float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    public void Show()
    {
        settingsPanel.SetActive(true);
        isOpen = true;

        // Load settings into the UI
        musicSlider.value = rsoContentSaved.Value.musicVolume;
        UpdatePercentageText(musicSlider, musicPercentageText, musicSlider.minValue, musicSlider.maxValue);

        soundSlider.value = rsoContentSaved.Value.soundVolume;
        UpdatePercentageText(soundSlider, soundPercentageText, soundSlider.minValue, soundSlider.maxValue);

        mouseSensitivitySlider.value = rsoContentSaved.Value.mouseSensitivity;
        UpdatePercentageText(mouseSensitivitySlider, mouseSensitivityPercentageText, mouseSensitivitySlider.minValue, mouseSensitivitySlider.maxValue);

        fullScreenToggle.isOn = rsoContentSaved.Value.isFullScreen;
    }

    public void Hide()
    {
        isOpen = false;
        settingsPanel.SetActive(false);
        rseSaveData.Call();
    }

    private void UpdatePercentageText(UnityEngine.UI.Slider slider, TMP_Text percentageText, float minValue, float maxValue)
    {
        float normalizedValue = Mathf.InverseLerp(minValue, maxValue, slider.value);
        percentageText.text = Mathf.RoundToInt(normalizedValue * 100) + "%";
    }

    public void ResetAchivmentsButton()
    {
        rsoContentSaved.Value = new();
        rseSaveData.Call();
    }
}