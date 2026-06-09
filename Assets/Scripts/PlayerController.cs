using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public GameManager GM;

    public float MoveSpeed = 5f;
    public float JumpForce = 5f;
    public int Damage = 1;

    [SerializeField] bool isGrounded;

    [Header("Health")]
    public int MaxHealth = 3;
    private int currentHealth;

    [Header("Invincibility")]
    public float InvincibilityDuration = 1f;
    bool isInvincible;
    float invincibilityTimer;

    [Header("Knockback")]
    public float KnockbackDuration = 1f;
    private bool isKnockedBack;

    [Header("Combat")]
    public GameObject BulletPrefab;
    public float AttackDuration = 0.2f;
    public float AttackCooldown = 0.5f;
    float attackTimerDuration;
    float attackTimerCooldown;
    bool isAttacking;

    Vector2 currentVelocity;
    Rigidbody2D rb;
    Animator animator;

    string currentState = "player_idle";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
        GM = FindFirstObjectByType<GameManager>();
        SetState("player_hurt");
    }

    void SetState(string newState)
    {
        if(currentState == newState)
            return;
        
        currentState = newState;
        animator.Play(currentState);
    }

    void UpdateAnimation()
    {
        if (isAttacking)
        {
            SetState("player_atk");
            return;
        }
        if (isGrounded)
        {
            if(currentVelocity.x != 0)
            {
                SetState("player_walk");
            }
            else
            {
                SetState("player_idle");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                SetState("player_jump");
            }
            else if(rb.linearVelocityY < 0)
            {
                SetState("player_fall");
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack)
        {
            SetState("player_hurt");
            return;
        }

        Gravity();
        Move();
        DoAtk();
        UpdateAnimation();

        rb.linearVelocity = currentVelocity;

        InvincibleCounter();
    }

    void InvincibleCounter()
    {
        if(isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
                isInvincible = false;
        }
    }

    void Gravity()
    {
        if(!isGrounded)
        {
            currentVelocity.y -= 9.81f * Time.deltaTime;
        }else if(currentVelocity.y < 0)
        {
            currentVelocity.y = 0;
        }
    }

    void Move()
    {
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        currentVelocity.x = moveInput.x * MoveSpeed;

        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        bool jumpInput = InputSystem.actions["Jump"].triggered;
        
        if(jumpInput && isGrounded)
        {
            currentVelocity.y = JumpForce;
            //rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    public void DoBounce(float force)
    {
        isGrounded = false;
        currentVelocity.y = force;
    }

    void EndKnockBack()
    {
        isKnockedBack = false;
    }

    public void DoKnockBack(Vector2 direction, float force)
    {
        isKnockedBack = true;
        currentVelocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce((direction + Vector2.up) * force, ForceMode2D.Impulse);
        CancelInvoke("EndKnockBack");
        Invoke("EndKnockBack", KnockbackDuration);
    }

    void DoAtk()
    {
        if(attackTimerCooldown > 0)
            attackTimerCooldown -= Time.deltaTime;

        if(InputSystem.actions["Attack"].triggered && attackTimerCooldown <= 0)
        {
            isAttacking = true;
            attackTimerDuration = AttackDuration;
            Instantiate(BulletPrefab, transform.position, transform.rotation);
            attackTimerCooldown = AttackCooldown;
        }

        if(isAttacking)
        {
            attackTimerDuration -= Time.deltaTime;
            if(attackTimerDuration <= 0)
            {
                isAttacking = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(isInvincible) return;

        currentHealth -= damage;
        isInvincible = true;
        invincibilityTimer = InvincibilityDuration;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Morreu!");
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            GM.CollectCoin();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
