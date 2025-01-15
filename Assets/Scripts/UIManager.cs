using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private int buildIdx;
    [SerializeField] private GameObject pauseMenuPanel;  // Reference to the pause menu UI panel

    private bool isPaused = false;


    private void Start()
    {
        UnpauseGame();

    }
    void Update()
    {
        // Toggle pause menu on Esc key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnButtonClicked()
    {
        if (IsValidBuildIndex(buildIdx))
        {
            SceneManager.LoadScene(buildIdx);
        }
        else
        {
            Debug.LogError("Start scene build index is not valid.");
        }
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private bool IsValidBuildIndex(int buildIndex)
    {
        return buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings;
    }

    private void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void UnpauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
