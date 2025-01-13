using BT.ScriptablesObject;
using UnityEngine;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private MainPanelUI mainPanel;
    [SerializeField] private RSE_OnGameStart rseOnGameStart;
    [SerializeField] SettingsPanel settingsPanel;

    [SerializeField] SSO_Achivment_CompleteOnce playButtonAchivment;

    public void PlayGame()
    {
        rseOnGameStart.Call();
        mainPanel.HideAllPanels();

        playButtonAchivment.Achieve();
    }

    public void OpenSettings()
    {
        settingsPanel.Show();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}