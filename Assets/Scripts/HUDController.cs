using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TMP_Text scoreText;
    public Image[] hearts;
    public TMP_Text timerText;
    public TMP_Text powerUpText;
    public TMP_Text chargeText;
    private CharacterMovement player;

    private void Start()
    {
        player = FindAnyObjectByType<CharacterMovement>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + GameManager.Instance.Score;
        int seconds = (int)GameManager.Instance.Elapsed;
        timerText.text = "Time: " + (seconds / 60).ToString("00") + ":" + (seconds % 60).ToString("00");
        if (player == null) return;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < player.Health;
        }
        powerUpText.text = "";
        if (player.SpeedRemaining > 0)
            powerUpText.text += "Speed boost: " + Mathf.CeilToInt(player.SpeedRemaining) + "s\n";
        if (player.ScoreRemaining > 0)
            powerUpText.text += "Double score: " + Mathf.CeilToInt(player.ScoreRemaining) + "s\n";
        if (player.JumpRemaining > 0)
            powerUpText.text += "Double jump: " + Mathf.CeilToInt(player.JumpRemaining) + "s";
        chargeText.text = "";
        if (player.IsCharging)
            chargeText.text = "Jump charge: " + Mathf.RoundToInt(player.Charge / 3f * 100f) + "%";
    }
}
