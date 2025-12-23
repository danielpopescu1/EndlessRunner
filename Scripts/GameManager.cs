using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("Assign the GameOver panel (set inactive by default).")]
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Ensure the game over panel is hidden at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // Call this to show Game Over UI
    public void GameOver()
    {
        // Stop the game
        Time.timeScale = 0f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Hook this method to the Retry button OnClick
    public void Restart()
    {
        // Resume time before loading
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Optional: go to main menu if you have one
    public void LoadMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}