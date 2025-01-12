using BT.ScriptablesObject;
using UnityEngine;
public class GameOverPanelUI : MonoBehaviour
{
    [SerializeField] private MainPanelUI mainPanel;
    [SerializeField] GameObject gameOverButtons;

    [SerializeField] private RSE_OnPlayerDeath rseOnPlayerDeath;

    private void OnEnable()
    {
        rseOnPlayerDeath.action += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        rseOnPlayerDeath.action -= ShowGameOverPanel;
    }

    private void ShowGameOverPanel()
    {
        mainPanel.ShowPanel(gameOverButtons);
    }

    public void RestartGame()
    {
        // Logic to restart the game
    }

    public void ReturnToMainMenu()
    {
        mainPanel.ShowPanel(mainPanel.mainMenuPanel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}