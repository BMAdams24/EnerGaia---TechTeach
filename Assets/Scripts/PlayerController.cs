using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movespeed = 8f;
    public float jumpforce = 12f;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private bool isGrounded;


    [Header("Crouch")]
    public float crouchSpeedMultiplier = 0.5f;

    private bool isCrouching;
    private BoxCollider2D col;
    private Vector2 originalColliderSize;
    private Vector3 originalColliderOffset;


    [Header("Potential Energy)")]
    public float maxPotentialEnergy = 100f;
    public float chargeRate = 30f; //Energy gained per second
    public float decayRate = 10f; //Energy loss per second

    public float storedPE;
    private float heightPE;


    [Header("Potential Energy Decay")]
    public float decayDelay = 3f;

    private float decayTimer;


    [Header("Kinetic Energy")]
    public float kineticPowerMultiplier = 0.4f; //The power of the kinetic energy boost

    public float horizontalVerticalLeak = 0.2f; //How much vertical energy gets used when boosting horizontally
    public float verticalHorizontalLeak = 0.2f; //How much horizontal energy gets used when boosting vertically

    public GameObject dashHitbox; //Hitbox gameobject


    [Header("Kinetic Control")]
    public float kineticControlLockTime = 0.3f; //Locks the player input briefly after boosting (makes sure A/D doesnt have to be pressed to boost horizontally, but also still boosts if pressed)

    private float kineticTimer; //Eventually gets set to kineticControlLocktime. Simply starts the timer of ignoring player input (A/D) when boosting (Travels no matter what)


    [Header("Boost Cooldown")]
    public float maxCooldown = 30f;
    private float boostCooldownTimer;
    private bool canBoost = true;

    internal float peBeforeDash;


    [Header("Abilities")]
    public bool hasBoots = false;

    private float lastYPosition;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GameObject.Find("Sprite").GetComponent<Animator>();

        //Original Collider size and offset
        originalColliderSize = col.size;
        originalColliderOffset = col.offset;

        lastYPosition = transform.position.y; //Keeps track of players Y position for increasing PE

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GameObject.Find("Sprite").GetComponent<Animator>();

        originalColliderSize = col.size;
        originalColliderOffset = col.offset;

        if (GameManager.Instance != null && GameManager.Instance.hasBoots)
        {
            UnlockBoots();
        }
    }

    // Update is called once per frame
    void Update()
    {

        float speed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", speed);

        bool crouch = isCrouching;
        animator.SetBool("Crouch", crouch);

        if (kineticTimer > 0f)
        {
            kineticTimer -= Time.deltaTime; //Timer for kinetic energy use
        }


        if (!canBoost) //Adds cooldown timer for boosting based on how much energy was used
        {
            boostCooldownTimer -= Time.deltaTime;

            if (boostCooldownTimer <= 0f)
            {
                canBoost = true;
                boostCooldownTimer = 0f;
            }
        }

        //Calls the functions for movement, jumping, crouching, and potential energy
        HandleMovement();
        CheckGrounded();
        HandleJump();
        HandleCrouch();
        HandlePotentialEnergy();
        HandleKineticRelease();
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        //New movement Input Handleing for version 6.3 cause it forced me to switch from the old... (A/D)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput = 1f;


        float speed = isCrouching ? movespeed * crouchSpeedMultiplier : movespeed; //Checks if player is crouching and adjusts the movespeed accordingly

        if (kineticTimer > 0f)
        {
            return; //If kinetic boost is active, ignore the horizontal input
        }


        if (moveInput != 0)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); //The player input times the speed (crouching or not) is the new linear velocity in the y direction

            transform.localScale = new Vector3(0.6f * Mathf.Sign(moveInput), 0.6f, 0.6f); //Added after kinetic energy. Ensures that facing left will boost left and vise versa
        }

        else
        {
            rb.linearVelocity = new Vector2(0f,rb.linearVelocity.y); //Brake the movement when theres no input/boost (keeps the player from sliding after input)
        }
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded) //if the space bar is pressed
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce); //The linear velocity in the x direction is set to whatever the jump force is
        }
    }


    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //Checks the player to see if it is grounded or not using its position, radius, and layer(ground)
    }

    void OnDrawGizmosSelected() //Debug function for groundCheck. This draws the ground check in Scene View
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void HandleCrouch()
    {
        if (!hasBoots)
        {
            return; //Locks crouching till boots are equiped
        }

        if (Keyboard.current.leftCtrlKey.isPressed) //If left control is input
        {
            if (!isCrouching)
            {
                StartCrouch(); //Start crouching if not already crouching
            }
        }

        else
        {
            if (isCrouching)
            {
                StopCrouch(); //If already crouching, stop crouching
            }
        }
    }

    void StartCrouch()
    {
        isCrouching = true;
    }

    void StopCrouch()
    {
        isCrouching = false;

        col.size = originalColliderSize; //Set the collider size back to the orignal
        col.offset = originalColliderOffset; //Offset the collider back to the original position
    }


    void HandlePotentialEnergy()
    {
        if (!canBoost)
        {
            heightPE = 0f;
            return;
        }

        //Height based PE (visual only)
        float currentHeight = transform.position.y;
        heightPE = Mathf.Clamp(currentHeight * 2f, 0f, maxPotentialEnergy * 0.25f);

        //STORED PE FROM BOOTS
        if (hasBoots && isCrouching && isGrounded && canBoost)
        {
            storedPE += chargeRate * Time.deltaTime;
            storedPE = Mathf.Clamp(storedPE, 0f, maxPotentialEnergy);

            decayTimer = decayDelay;
        }
        else
        {
            if (decayTimer > 0f)
            {
                decayTimer -= Time.deltaTime;
            }
            else
            {
                storedPE -= decayRate * Time.deltaTime;
                storedPE = Mathf.Clamp(storedPE, 0f, maxPotentialEnergy);
            }
        }

        //Prevents totalPE from exceeding max
        float totalPE = storedPE + heightPE;
        if (totalPE > maxPotentialEnergy)
        {
            storedPE = Mathf.Clamp(maxPotentialEnergy - heightPE, 0f, maxPotentialEnergy);
        }
    }

    public float GetCurrentPE()
    {
        return Mathf.Clamp(storedPE + heightPE, 0f, maxPotentialEnergy);
    }

    public float GetPotentialEnergyNormalized()
    {
        float totalPE = storedPE + heightPE;
        return Mathf.Clamp01(totalPE / maxPotentialEnergy);
    }


    void HandleKineticRelease()
    {
        if (!hasBoots)
        {
            return; //Locks boosting until boots are equiped
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && storedPE > 0f)
        {
            ReleaseKineticEnergy(); //When left shift is pressed, release the kinetic energy
        }
    }

    void ReleaseKineticEnergy()
    {
        if (!canBoost)
        {
            return;
        }

        float totalPE = storedPE + heightPE;

        if (totalPE <= 0f)
        {
            return;
        }

        float percentUsed = totalPE / maxPotentialEnergy;
        float totalPower = totalPE * kineticPowerMultiplier; //Total power based on how much potential is stored times the kinetic multiplier
        storedPE = 0f;
        heightPE = 0f;

        float horizontal = 0f;
        float vertical = 0f;

        bool wantsVertical = Keyboard.current.wKey.isPressed; //wantsVertical is set to "W" key input

        float direction = Mathf.Sign(transform.localScale.x);
        if (direction == 0)
        {
            direction = 1;
        }

        float damage = totalPower * 0.5f;
        DashHitbox hitbox = dashHitbox.GetComponent<DashHitbox>();
        hitbox.SetDamage(damage);


        if (wantsVertical) //If "W" is pressed (Vertical)
        {
            vertical = totalPower; //Total power goes to vertical (up)
            horizontal = totalPower * verticalHorizontalLeak * direction; //Horizontal gets some power (curve) in the direction is facing
        }

        else //(Horizontal)
        {
            horizontal = totalPower * direction; //Total power goes to horizontal
            vertical = totalPower * horizontalVerticalLeak; //Vertical gets some power (curve) to give it some height
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + horizontal, rb.linearVelocity.y + vertical);

        StartCoroutine(EnableDashHitbox(0.2f)); //Enable the dash hitbox for 2 seconds (EnableDashHitbox funtion below this one)
        peBeforeDash = totalPE; // store PE for damage


        boostCooldownTimer = maxCooldown * percentUsed; //Boost Cooldown Calculation
        canBoost = false;

        totalPE = 0f; //Resets the Potential Energy bar when used
        kineticTimer = kineticControlLockTime; //Starts a lock timer on kinetic energy use
    }

    private System.Collections.IEnumerator EnableDashHitbox(float duration) //Enable dash hitbox temporarily. Duration is set when called in the ReleaseKineticEnergy function above
    {
        dashHitbox.SetActive(true); //Sets hitbox true
        yield return new WaitForSeconds(duration); //Waits the duration
        dashHitbox.SetActive(false); //Deativates hitbox
    }

    public float GetCooldownNormalized()
    {
        if (canBoost)
        {
            return 0f;
        }

        return boostCooldownTimer / maxCooldown;
    }

    public void UnlockBoots()
    {
        hasBoots = true;
    }
}
