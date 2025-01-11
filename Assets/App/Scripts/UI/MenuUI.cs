using UnityEngine;
public class MenuUI : MonoBehaviour
{
    //[Header("Settings")]

    bool isOnMainMenu = true;
    bool isOnSettings = false;
    bool isPaused = false;

    [Header("References")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject mainMenuButtons;
    [SerializeField] GameObject pauseButtons;
    [SerializeField] GameObject gameOverButtons;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    
    [Header("Output")]
    [SerializeField] RSE_OnGameStart rseOnGameStart;

    private void OnEnable()
    {
        rseOnPlayerDeath.action += OpenGameOverButton;
    }
    private void OnDisable()
    {
        rseOnPlayerDeath.action -= OpenGameOverButton;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOnSettings)
            {
                if (isOnMainMenu)
                {
                    OpenMainMenuButton();
                }
                else
                {
                    OpenPauseMenuButton();
                }
            }
            else if (!isOnMainMenu)
            {
                if (isPaused)
                {
                    ClosePauseMenu();
                }
                else
                {
                    ShowMenu();
                    OpenPauseMenuButton();
                    PauseGame();
                    Cursor.lockState = CursorLockMode.None;
                }

            }
        }
    }

    public void PlayButton()
    {
        HideMenu();
        rseOnGameStart.Call();
        isOnMainMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void ShowMenu()
    {
        menuPanel.SetActive(true);
    }
    void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    void OpenPauseMenuButton()
    {
        Time.timeScale = 1;
        mainMenuButtons.SetActive(false);
        pauseButtons.SetActive(true);
        gameOverButtons.SetActive(false);

        isOnMainMenu = false;
        isPaused = true;
        isOnSettings = false;
    }
    public void ClosePauseMenu()
    {
        HideMenu();
        ContinueGame();
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OpenMainMenuButton()
    {
        mainMenuButtons.SetActive(true);
        pauseButtons.SetActive(false);
        gameOverButtons.SetActive(false);

        isOnMainMenu = true;
        isPaused = false;
        isOnSettings = false;
    }
    void OpenGameOverButton()
    {
        ShowMenu();
        mainMenuButtons.SetActive(false);
        pauseButtons.SetActive(false);
        gameOverButtons.SetActive(true);

        isOnMainMenu = true;
        isPaused = false;
        isOnSettings = false;

        Cursor.lockState = CursorLockMode.None;
    }

    // Degueu mais pas le temps...
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}