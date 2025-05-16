using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeneGames.DialogueSystem;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;
    public float bounceForce = 6f;
    public int health = 3;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving;
    private bool isDead;
    private bool takingDamage;
    private bool playerAlive;
    private bool isMovementDisabled = false; // Flag to disable movement

    private Animator animator;

    void Start()
    {
        playerAlive = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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

    void Update()
    {
        if (playerAlive && !isDead && !isMovementDisabled)
        {
            Move();
        }

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isDead", isDead);
    }

    private void DisableMovement()
    {
        isMovementDisabled = true;
    }

    private void EnableMovement()
    {
        isMovementDisabled = false;
    }

    private void Move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            movement = new Vector2(direction.x, 0);

            isMoving = true;
        }
        else
        {
            movement = Vector2.zero;
            isMoving = false;
        }
        if (!takingDamage)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 damageDirection = new Vector2(transform.position.x, 0);
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();

            playerScript.TakeDamage(damageDirection, 1);
            playerAlive = !playerScript.isDead;
            if (!playerAlive)
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            Vector2 damageDirection = new Vector2(collision.gameObject.transform.position.x, 0);

            TakeDamage(damageDirection, 1);
        }
    }

    public void TakeDamage(Vector2 direction, int damageAmount)
    {
        if (!takingDamage)
        {
            health -= damageAmount;
            takingDamage = true;
            if (health <= 0)
            {
                isDead = true;
                isMoving = false;
                SoundManager.instance.PlaySfx(SoundManager.instance.zombieDie);
            }
            else
            {
                Vector2 bounce = new Vector2(transform.position.x - direction.x, 0.2f).normalized;
                rb.AddForce(bounce * bounceForce, ForceMode2D.Impulse);
                StartCoroutine(DisableDamage());
            }
        }
    }

    IEnumerator DisableDamage()
    {
        yield return new WaitForSeconds(0.4f);
        takingDamage = false;
        rb.velocity = Vector2.zero;
    }

    public void DestroyBody()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
