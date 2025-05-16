using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI buttons
using HeneGames.DialogueSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float health = 3f;
    public float maxHealth = 3f;

    public float jumpForce = 10f;
    public float bounceForce = 6f;
    public float raycastLength = 0.1f;
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f; // Multiplier for faster falling
    public float lowJumpMultiplier = 2f; // Multiplier for shorter jumps

    private bool onGround;
    private bool takingDamage;
    private bool isAttacking;
    public bool isDead;
    private bool isMovementDisabled = false; // Flag to disable movement

    private Rigidbody2D rb;

    public Animator animator;

    public event Action<float> OnHealthChanged; // Event for health changes

    // On-screen control variables
    private float horizontalInput = 0f; // Tracks horizontal input from buttons
    private bool jumpPressed = false; // Tracks if the jump button is pressed
    private bool attackPressed = false; // Tracks if the attack button is pressed

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Subscribe to dialogue events
        DialogueManager.OnDialogueStart += DisableMovement;
        DialogueManager.OnDialogueEnd += EnableMovement;
    }

    private void OnDestroy()
    {
        // Unsubscribe from dialogue events
        DialogueManager.OnDialogueStart -= DisableMovement;
        DialogueManager.OnDialogueEnd -= EnableMovement;
    }

    private void Update()
    {
        if (!isDead && !isMovementDisabled)
        {
            // Handle keyboard input
            HandleKeyboardInput();

            if (!isAttacking)
            {
                Move();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, groundLayer);
                onGround = hit.collider != null;

                if (jumpPressed && onGround && !takingDamage)
                {
                    Jump();
                    jumpPressed = false; // Reset jumpPressed after jumping
                }
            }

            if (attackPressed && !isAttacking && onGround)
            {
                Attack();
                attackPressed = false; // Reset attackPressed after attacking
            }

            // Apply faster fall or low jump multipliers
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            animator.SetBool("onGround", onGround);
            animator.SetBool("takingDamage", takingDamage);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDead", isDead);
        }
    }

    private void HandleKeyboardInput()
    {
        // Horizontal movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        // Stop horizontal movement when the key is released
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            horizontalInput = 0f;
        }


        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpPressed = true;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.F))
        {
            attackPressed = true;
        }
    }

    private void DisableMovement()
    {
        isMovementDisabled = true;
        animator.SetFloat("movement", 0);   // Stop movement animation
        rb.velocity = Vector2.zero; // Reset velocity to prevent sliding
        // set the animator to idle
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("takingDamage", false);
        animator.SetBool("onGround", true);
    }

    private void EnableMovement()
    {
        isMovementDisabled = false;
    }

    public void Move()
    {
        float horizontalSpeed = horizontalInput * speed * Time.deltaTime;

        animator.SetFloat("movement", Mathf.Abs(horizontalSpeed));

        if (horizontalSpeed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalSpeed > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (!takingDamage)
        {
            transform.Translate(horizontalSpeed, 0, 0);
        }
    }

    public void TakeDamage(Vector2 direction, int damageAmount)
    {
        if (!takingDamage)
        {
            SoundManager.instance.PlaySfx(SoundManager.instance.playerHit);
            takingDamage = true;
            health = Mathf.Clamp(health - damageAmount, 0, maxHealth);
            OnHealthChanged?.Invoke(health); // Trigger the health change event
            if (health <= 0)
            {
                Die(); // Call the Die method if health is 0
            }
            if (!isDead)
            {
                rb.velocity = Vector2.zero; // Reset velocity to prevent sliding
                Vector2 bounce = new Vector2(transform.position.x - direction.x, 0.2f).normalized;
                rb.AddForce(bounce * bounceForce, ForceMode2D.Impulse);
            }
        }
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(health); // Trigger the health change event
    }

    public void DisableDamage()
    {
        takingDamage = false;
        rb.velocity = Vector2.zero;
    }

    public void Die()
    {
        SoundManager.instance.PlaySfx(SoundManager.instance.playerDie);
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero; // Reset velocity to prevent sliding
        rb.isKinematic = true; // Disable physics interactions

        yield return new WaitForSeconds(1f); // Optional delay before death animation
        GameManager.instance.GameOver(); // Call Game Over method from GameManager
    }

    public void Attack()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.swordSounds);
        isAttacking = true;
        animator.SetTrigger("attack");
    }

    public void DisableAttack()
    {
        isAttacking = false;
    }

    public void Jump()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.jumpSounds);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Set upward velocity directly
        animator.SetTrigger("jump"); // Optional: Trigger a jump animation if available
    }

    // On-screen control methods
    public void OnMoveLeftButtonDown()
    {
        horizontalInput = -1f;
    }

    public void OnMoveRightButtonDown()
    {
        horizontalInput = 1f;
    }

    public void OnMoveButtonUp()
    {
        horizontalInput = 0f;
    }

    public void OnJumpButtonDown()
    {
        jumpPressed = true;
    }

    public void OnAttackButtonDown()
    {
        attackPressed = true;
    }

    public void StopPlayer()
    {
        rb.velocity = Vector2.zero; // Reset velocity to prevent sliding
        animator.SetFloat("movement", 0);   // Stop movement animation
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("takingDamage", false);
        animator.SetBool("onGround", true);
        isMovementDisabled = true; // Disable movement
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            // Call the Die method when entering the death zone
            Die();
        }

    }
}
