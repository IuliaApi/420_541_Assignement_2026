using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public bool IsGrounded { get; private set; }
    public bool IsCharging { get; private set; }
    public float Charge { get; private set; }
    public float SpeedRemaining => powerUps.SpeedTimeLeft;
    public float ScoreRemaining => powerUps.ScoreTimeLeft;
    public float JumpRemaining => powerUps.JumpTimeLeft;
    public float SpeedMultiplier { get; set; } = 1f;
    public int Health { get; private set; } = 3;
    Rigidbody body;
    Vector3 direction;
    private PlayerPowerUps powerUps;
    float damageUntil, airborneUntil, queuedJump;
    bool usedDouble, running;
    public bool DoubleScore => ScoreRemaining > 0;
    void Awake()
    {
        powerUps = GetComponent<PlayerPowerUps>();
        body = GetComponent<Rigidbody>(); body.constraints = RigidbodyConstraints.FreezeRotation;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.Paused) return;
        IsGrounded = Time.time > airborneUntil && Physics.Raycast(transform.position + Vector3.up * .15f, Vector3.down, .3f, 1, QueryTriggerInteraction.Ignore);
        if (IsGrounded) usedDouble = false;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Transform cam = Camera.main != null ? Camera.main.transform : transform;
        Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        direction = Vector3.ClampMagnitude(forward * input.z + right * input.x, 1);
        running = Input.GetButton("Run");
        if (direction.sqrMagnitude > .01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 14);
        if (Input.GetButtonDown("Jump")) PressJump();
        if (IsCharging)
        {
            if (!IsGrounded) { IsCharging = false; Charge = 0; }
            else
            {
                Charge = Mathf.Min(3, Charge + Time.deltaTime);
                if (Input.GetButtonUp("Jump") || Charge >= 3) ReleaseJump();
            }
        }
        if (transform.position.y < -12) GameManager.Instance.RestartLevel();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance == null || GameManager.Instance.Paused) return;
        Vector3 v = direction * walkSpeed * (running ? 2 : 1) * SpeedMultiplier;
        body.linearVelocity = new Vector3(v.x, body.linearVelocity.y, v.z);
        if (queuedJump > 0)
        {
            body.linearVelocity = new Vector3(v.x, queuedJump, v.z);
            GameManager.Instance.PlaySound(GameManager.Instance.jumpSound);
            queuedJump = 0; airborneUntil = Time.time + .15f; IsGrounded = false;
        }
    }
    public void PressJump()
    {
        if (IsGrounded) { IsCharging = true; Charge = 0; }
        else TryDoubleJump();
    }
    public void ReleaseJump()
    {
        if (!IsCharging) return;
        queuedJump = Mathf.Lerp(5.5f, 12f, Charge / 3); IsCharging = false;
    }
    public bool TryDoubleJump()
    {
        if (IsGrounded || usedDouble || JumpRemaining <= 0 || queuedJump > 0) return false;
        usedDouble = true;
        queuedJump = 8f;
        GetComponent<CharacterAnimator>().PlayDoubleJump();
        Effects.Burst(transform.position + Vector3.up, new Color(.15f,.7f,1));
        return true;
    }
    public void Power(Pickup.Kind kind)
    {
        if (kind == Pickup.Kind.Speed) powerUps.ActivateSpeedBoost(5f);
        if (kind == Pickup.Kind.ScoreBoost) powerUps.ActivateScoreBoost(10f);
        if (kind == Pickup.Kind.DoubleJump) powerUps.ActivateJumpBoost(30f);
    }
    public void Damage()
    {
        if (Time.time < damageUntil) return;
        damageUntil = Time.time + 1.5f;
        Health--;
        GameManager.Instance.PlaySound(GameManager.Instance.damageSound);
        Effects.Burst(transform.position + Vector3.up, Color.red);
        if (Health <= 0) GameManager.Instance.RestartLevel();
    }
}
