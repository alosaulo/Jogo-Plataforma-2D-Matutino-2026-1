using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JumpForce = 5f;

    [SerializeField] bool isGrounded;
    Vector2 currentVelocity;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        Move();
        rb.linearVelocity = currentVelocity;
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
