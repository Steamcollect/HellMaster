using BT.ScriptablesObject;
using UnityEngine;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private MainPanelUI mainPanel;
    [SerializeField] private RSE_OnGameStart rseOnGameStart;
    [SerializeField] SettingsPanel settingsPanel;

    public void PlayGame()
    {
        rseOnGameStart.Call();
        mainPanel.HideAllPanels();
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