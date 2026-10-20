using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Kind { Score, Speed, ScoreBoost, DoubleJump }
    public Kind kind;
    [SerializeField] private int points = 50;
    [SerializeField] private float speedDuration = 5f;
    [SerializeField] private float scoreDuration = 10f;
    [SerializeField] private float jumpDuration = 30f;
    [SerializeField] private float respawnDelay = 30f;

    private Renderer visual;
    private Collider hitbox;
    private bool available = true;

    private void Awake()
    {
        visual = GetComponentInChildren<Renderer>();
        hitbox = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUps powerUps = other.GetComponentInParent<PlayerPowerUps>();
        if (!available || powerUps == null) return;

        available = false;
        hitbox.enabled = false;
        GameManager.Instance.PlaySound(GameManager.Instance.pickupSound);
        Effects.Burst(transform.position, visual.sharedMaterial.color);

        if (kind == Kind.Score)
        {
            int amount = powerUps.ScoreTimeLeft > 0f ? points * 2 : points;
            GameManager.Instance.AddScore(amount);
            visual.enabled = false;
            Destroy(gameObject);
            return;
        }

        if (kind == Kind.Speed) powerUps.ActivateSpeedBoost(speedDuration);
        if (kind == Kind.ScoreBoost) powerUps.ActivateScoreBoost(scoreDuration);
        if (kind == Kind.DoubleJump) powerUps.ActivateJumpBoost(jumpDuration);

        visual.enabled = false;
        Invoke(nameof(Reappear), respawnDelay);
    }

    private void Reappear()
    {
        available = true;
        visual.enabled = true;
        hitbox.enabled = true;
    }
}
