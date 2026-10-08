using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float maxHealth = 100f; //Boss health
    private float currentHealth; //Current health

    public GameObject quizUI;

    private bool stunned = false; //starts not stunned
    private bool phase75Triggered = false;
    private bool phase25Triggered = false;

    public Slider healthBar;

    [Header("Movement")]
    public float moveSpeed = 2f;
    private Vector2 targetPosition;
    public Transform leftBoundary;
    public Transform rightBoundary;
    private bool facingLeft = true;

    [Header("Projectiles")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    private float attackTimer;
    public Transform player;

    [Header("Jumping")]
    public float jumpForce = 8f;
    private float jumpTimer;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth; //Starting health is maxed
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        jumpTimer = Random.Range(2f, 5f);

        PickNewTarget();
}

    void Update()
    {
        FacePlayer();

        Patrol();

        HandleAttacking();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //Defines isGrounded for checking if the boss is on the ground or not

        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0f && isGrounded)
        {
            Jump();
            jumpTimer = Random.Range(2f, 5f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (stunned) return; //If the boss is stunned, attacks wont do damage (due to da quiz)

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Boss Health: " + currentHealth);

        healthBar.value = currentHealth;
        CheckPhases();
    }

    void CheckPhases()
    {
        float healthPercent = currentHealth / maxHealth; //Keeps track of current health by percent

        if (!phase75Triggered && healthPercent <= 0.75f)
        {
            TriggerStun();
            phase75Triggered = true; //If boss health is at 75%, then trigger the quiz
        }
        else if (!phase25Triggered && healthPercent <= 0.25f)
        {
            TriggerStun();
            phase25Triggered = true; //Same but for 25%
        }
    }

    void TriggerStun()
    {
        stunned = true;

        quizUI.SetActive(true);

        Time.timeScale = 0f; //FREEZE GAME

        QuizManager qm = FindFirstObjectByType<QuizManager>();
        if (qm != null)
        {
            qm.LoadRandomQuestion();
        }

        Debug.Log("Boss Stunned - Game Paused");
    }

    public void EndStun()
    {
        stunned = false;
        quizUI.SetActive(false);

        Time.timeScale = 1f; //RESUME GAME

        Debug.Log("Boss Recovered!");
    }

    public void ApplyQuizDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        healthBar.value = currentHealth;

        EndStun();
    }

    public void TryStun() //TEST FOR QUIZ AND STUN!
    {
        if (currentHealth <= maxHealth * 0.75f && !stunned)
        {
            stunned = true;
            Debug.Log("Boss stunned! Show quiz now.");
        }
    }
    
    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); //Position equals move towards from the current position to the target position at move speed

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f) //If the boss reached the target position
        {
            PickNewTarget(); //Run the Pick new target function and pick a new target position to move to
        }
    }

    void HandleAttacking()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Shoot();
            attackTimer = attackCooldown;
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<BossProjectile>().Initialize(player);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void FacePlayer()
    {
        if (player == null)
        {
            return;
        }

        if (player.position.x < transform.position.x && !facingLeft)
        {
            Flip();
        }

        else if (player.position.x >  transform.position.x && facingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 scale = transform.localScale; //Sets scale to the local transform scale of boss
        scale.x *= -1; //Flips in the x direstion
        transform.localScale = scale;
    }

    void PickNewTarget()
    {
        float randomX = Random.Range(leftBoundary.position.x, rightBoundary.position.x); //Picks a random x value between the left and right boundaries
        targetPosition = new Vector2(randomX, transform.position.y); //Sets the target position to the random value
    }
}
