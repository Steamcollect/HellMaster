using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanelUI : MonoBehaviour
{
    [SerializeField] private MainPanelUI mainPanel;
    [SerializeField] GameObject pauseButtons;
    [SerializeField] SettingsPanel settingsPanel;

    [SerializeField] private RSE_OnPauseStateChanged rseOnPauseStateChanged;

    bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!mainPanel.IsMainMenuActive() && !mainPanel.IsGameOverActive() && !mainPanel.IsSettingsActive())
            {
                if (Time.timeScale == 0f)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        mainPanel.ShowPanel(pauseButtons);
        rseOnPauseStateChanged.Call(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        mainPanel.HideAllPanels();
        rseOnPauseStateChanged.Call(isPaused);
    }

    public void OpenSettings()
    {
        settingsPanel.Show();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}