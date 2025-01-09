using UnityEngine;
public class MainMenuUI : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] GameObject mainMenuPanel;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    [Header("Output")]
    [SerializeField] RSE_OnGameStart rseOnGameStart;

    public void PlayButton()
    {
        Hide();
        rseOnGameStart.Call();
    }

    void Show()
    {
        mainMenuPanel.SetActive(true);
    }
    void Hide()
    {
        mainMenuPanel.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}