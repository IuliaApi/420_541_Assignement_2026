using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private Rigidbody body;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<CharacterMovement>();
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 velocity = body.linearVelocity;
        velocity.y = 0;
        animator.SetFloat("Speed", velocity.magnitude, .12f, Time.deltaTime);
        animator.SetBool("Grounded", movement.IsGrounded);
    }

    public void PlayDoubleJump()
    {
        animator.SetTrigger("DoubleJump");
    }
}
