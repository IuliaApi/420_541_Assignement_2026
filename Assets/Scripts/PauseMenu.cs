using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
            GameManager.Instance.SetPause(!GameManager.Instance.Paused);
        pausePanel.SetActive(GameManager.Instance.Paused);
    }

    public void ResumeGame()
    {
        GameManager.Instance.SetPause(false);
        pausePanel.SetActive(false);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }
}
