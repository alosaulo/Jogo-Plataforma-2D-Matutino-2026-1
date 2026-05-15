using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

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

    Vector2 currentVelocity;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isKnockedBack) return;

        Gravity();
        Move();

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
