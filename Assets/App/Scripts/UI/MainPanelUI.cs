using UnityEngine;
public class MainPanelUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject mainPanelVisual;

    private GameObject currentActivePanel;

    [SerializeField] RSE_OnPanelOpen rseOnPanelOpen;

    private void Start()
    {
        HideAllPanels();
        ShowPanel(mainMenuPanel); // Show main menu at the start
    }

    public void ShowPanel(GameObject panel)
    {
        HideAllPanels();
        panel.SetActive(true);
        currentActivePanel = panel;
        UpdateMainPanelVisual();

        rseOnPanelOpen.Call();
    }

    public void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        currentActivePanel = null;
        UpdateMainPanelVisual();
    }

    public bool IsMainMenuActive() => currentActivePanel == mainMenuPanel;
    public bool IsGameOverActive() => currentActivePanel == gameOverPanel;

    private void UpdateMainPanelVisual()
    {
        mainPanelVisual.SetActive(currentActivePanel == mainMenuPanel || currentActivePanel == pausePanel || currentActivePanel == gameOverPanel);
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (currentActivePanel == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}