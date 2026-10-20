using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    [SerializeField] private float boostMultiplier = 1.5f;

    public float SpeedTimeLeft { get; private set; }
    public float ScoreTimeLeft { get; private set; }
    public float JumpTimeLeft { get; private set; }

    private CharacterMovement movement;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }

    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }

    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;
    }

    public void ActivateJumpBoost(float duration)
    {
        JumpTimeLeft = duration;
    }

    private void Update()
    {
        if (SpeedTimeLeft > 0f)
        {
            SpeedTimeLeft = Mathf.Max(0f, SpeedTimeLeft - Time.deltaTime);
            if (SpeedTimeLeft <= 0f) movement.SpeedMultiplier = 1f;
        }

        ScoreTimeLeft = Mathf.Max(0f, ScoreTimeLeft - Time.deltaTime);
        JumpTimeLeft = Mathf.Max(0f, JumpTimeLeft - Time.deltaTime);
    }
}
