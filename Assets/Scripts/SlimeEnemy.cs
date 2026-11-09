using UnityEngine;

// =====================================================================
// SlimeEnemy
// Inimigo que anda de um lado para o outro (patrulha).
// Machuca o jogador se encostar nele, mas morre se o jogador pisar
// na cabeça dele ou acertar um tiro.
// Vai nos prefabs GreenSlime e BlueSlime.
// =====================================================================
public class SlimeEnemy : MonoBehaviour
{
    // Quantos acertos o slime aguenta
    public int Health = 1;
    // Velocidade da patrulha
    public float moveSpeed = 1;
    // Quantos blocos ele anda para cada lado a partir da posição inicial
    public float patrolDistance = 2;
    // Para onde ele está indo: 1 = direita, -1 = esquerda
    public int facingDirection = 1;
    // Altura do quique do jogador ao pisar no slime
    public float bounciness = 5;
    // Força do empurrão no jogador quando o slime encosta nele
    public float KnockbackForce = 3;
    // Dano que o slime causa no jogador
    public int Damage = 1;
    private Vector2 currentVelocity;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    // Start roda uma vez: guarda de onde o slime começou
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update roda uma vez por frame
    void Update()
    {
        Move();
    }

    // Patrulha: se passou do limite da direita, volta para a esquerda, e vice-versa
    void Move()
    {
        if(transform.position.x > startPosition.x + patrolDistance)
        {
            facingDirection = -1;
        }else if(transform.position.x < startPosition.x - patrolDistance)
        {
            facingDirection = 1;
        }

        //Vira o sprite para o lado em que está andando
        transform.localScale = new Vector3(facingDirection, 1, 1);

        currentVelocity.x = facingDirection * moveSpeed;

        rb.linearVelocity = currentVelocity;
    }

    // Leva dano: quando a vida acaba, o slime é apagado da cena
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Colisão "sólida" com o corpo do jogador: o slime empurra e machuca o jogador
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                //Direção do empurrão: do slime para o jogador
                Vector3 knockBackDir = collision.transform.position - transform.position;
                player.DoKnockBack(knockBackDir, KnockbackForce);
                player.TakeDamage(Damage);
            }
        }
    }

    // O pé do jogador (Trigger com a tag "PlayerFoot") encostou no slime: foi pisado!
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerFoot")
        {
            //O pé é filho do Player, então o PlayerController está no "pai"
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if(player != null)
            {
                player.DoBounce(bounciness);
                TakeDamage(player.Damage);
            }
        }
    }
}
