using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}