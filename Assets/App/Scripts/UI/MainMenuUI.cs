using BT.ScriptablesObject;
using UnityEngine;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private MainPanelUI mainPanel;
    [SerializeField] private RSE_OnGameStart rseOnGameStart;

    public void PlayGame()
    {
        rseOnGameStart.Call();
        mainPanel.HideAllPanels();
    }

    public void OpenSettings()
    {
        // Logic to open settings
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}