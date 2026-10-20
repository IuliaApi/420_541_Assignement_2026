using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SceneReady();
    }

    public void PlayGame()
    {
        GameManager.Instance.NewGame();
    }

    public void LoadScene(string sceneName)
    {
        GameManager.Instance.Load(sceneName);
    }

    public void MainMenu()
    {
        LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
