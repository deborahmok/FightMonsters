using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerState player;
    [SerializeField] private GameManager gameManager;

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private bool ended = false;

    private void Awake()
    {
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    private void Update()
    {
        if (ended) return;
        if (player == null || gameManager == null) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Quit with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
        
        // LOSE condition
        if (player.CurrentHP <= 0)
        {
            AudioManager.Instance.PlayLose();
            EndLose();
            return;
        }

        // WIN condition
        if (gameManager.GetTreasuresCollected() >= gameManager.GetTotalTreasures())
        {
            AudioManager.Instance.PlayWin();
            EndWin();
            return;
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f; // IMPORTANT (because we paused on win/lose)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EndWin()
    {
        ended = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void EndLose()
    {
        ended = true;
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}