using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }
    public float Elapsed { get; private set; }
    public bool Paused { get; private set; }

    public AudioClip pickupSound;
    public AudioClip jumpSound;
    public AudioClip damageSound;

    private AudioSource audioSource;
    private int levelStartScore;
    private bool loading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Paused && SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            Elapsed += Time.deltaTime;
        }
    }

    public void SceneReady()
    {
        loading = false;
        SetPause(false);
        if (SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            levelStartScore = Score;
        }
    }

    public void AddScore(int points)
    {
        Score += points;
    }

    public void NewGame()
    {
        Score = 0;
        Elapsed = 0;
        levelStartScore = 0;
        Load("Level1");
    }

    public void RestartLevel()
    {
        Score = levelStartScore;
        Load(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "Level1") Load("Level2");
        else if (scene == "Level2") Load("Level3");
        else Load("EndScene");
    }

    public void Load(string sceneName)
    {
        if (loading) return;
        loading = true;
        SetPause(false);
        SceneManager.LoadScene(sceneName);
    }

    public void SetPause(bool value)
    {
        Paused = value;
        Time.timeScale = value ? 0 : 1;
        bool playing = SceneManager.GetActiveScene().name.StartsWith("Level");
        Cursor.visible = !playing || value;
        Cursor.lockState = playing && !value ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
