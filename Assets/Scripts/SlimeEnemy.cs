using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public int Health = 1;
    public float moveSpeed = 1;
    public float patrolDistance = 2;
    public int facingDirection = 1;
    public float bounciness = 5;
    public float KnockbackForce = 3;
    public int Damage = 1;
    private Vector2 currentVelocity;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(transform.position.x > startPosition.x + patrolDistance)
        {
            facingDirection = -1;
        }else if(transform.position.x < startPosition.x - patrolDistance)
        {
            facingDirection = 1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);

        currentVelocity.x = facingDirection * moveSpeed;

        rb.linearVelocity = currentVelocity;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                Vector3 knockBackDir = collision.transform.position - transform.position;
                player.DoKnockBack(knockBackDir, KnockbackForce);
                player.TakeDamage(Damage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerFoot")
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if(player != null)
            {
                player.DoBounce(bounciness);
                TakeDamage(player.Damage);
            }
        }
    }
}
