using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    public float Speed = 5f;
    public int Damage = 1;
    public float Lifetime = 2f;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * Speed;
        Destroy(gameObject, Lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        /*LayerMask layerGround = 6;
        if(collision.gameObject.layer == layerGround)
        {
            return;
        }*/

        if(collision.tag == "Player" || 
            collision.tag == "PlayerFoot" || 
            collision.tag == "PlayerAtk")
        {
            return;
        }

        if(collision.tag == "Enemy")
        {
            SlimeEnemy slimeEnemy = collision.GetComponent<SlimeEnemy>();
            if(slimeEnemy != null)
            {
                slimeEnemy.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }

}
