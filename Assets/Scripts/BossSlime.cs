using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// =====================================================================
// BossSlime
// O chefão "Rei Slime": anda de um lado para o outro, pula de tempos
// em tempos e fica mais rápido quando perde metade da vida.
// Quando a vida acaba, o jogo vai para a tela de vitória.
// Vai no GameObject "BossSlime" da Fase3 (um slime grande com Rigidbody2D).
// =====================================================================
public class BossSlime : MonoBehaviour
{
    [Header("Health")]
    // Quantos acertos o chefão aguenta
    public int MaxHealth = 10;
    // Barra de vida do chefão no topo da tela (uma Image do tipo "Filled")
    public Image imgBossLife;
    int currentHealth;

    [Header("Movement")]
    // Velocidade normal
    public float moveSpeed = 2;
    // Velocidade quando ele fica bravo (com metade da vida)
    public float angrySpeed = 4;
    // Quantos blocos ele anda para cada lado a partir da posição inicial
    public float patrolDistance = 5;
    // Para onde ele está indo: 1 = direita, -1 = esquerda
    int facingDirection = 1;
    Vector2 startPosition;

    [Header("Jump")]
    // Velocidade inicial do pulo
    public float JumpForce = 8;
    // De quantos em quantos segundos ele pula
    public float JumpInterval = 3;
    float jumpTimer;

    [Header("Combat")]
    // Dano que ele causa no jogador
    public int Damage = 1;
    // Força do empurrão no jogador
    public float KnockbackForce = 4;
    // Altura do quique do jogador ao pisar nele
    public float bounciness = 7;

    [Header("Victory")]
    // Cena carregada quando o chefão é derrotado
    public string VictoryScene = "Vitoria";

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // Start roda uma vez, quando a fase começa
    void Start()
    {
        currentHealth = MaxHealth;
        startPosition = transform.position;
        jumpTimer = JumpInterval;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update roda uma vez por frame
    void Update()
    {
        Move();
        Jump();
    }

    // Patrulha igual à do slime comum
    void Move()
    {
        if(transform.position.x > startPosition.x + patrolDistance)
        {
            facingDirection = -1;
        }else if(transform.position.x < startPosition.x - patrolDistance)
        {
            facingDirection = 1;
        }

        //Espelha o desenho quando anda para a esquerda
        spriteRenderer.flipX = facingDirection == -1;

        //Muda só a velocidade X; a Y fica com a física (gravidade e pulo)
        rb.linearVelocity = new Vector2(facingDirection * moveSpeed, rb.linearVelocityY);
    }

    // Conta o tempo; quando chega a zero, pula e começa a contar de novo
    void Jump()
    {
        jumpTimer -= Time.deltaTime;
        if(jumpTimer <= 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);
            jumpTimer = JumpInterval;
        }
    }

    // Leva dano (chamado pelo tiro do jogador e quando o jogador pisa nele)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Atualiza a barra de vida (o (float) evita a divisão de inteiros)
        imgBossLife.fillAmount = (float)currentHealth / MaxHealth;

        //Com metade da vida o chefão fica bravo: mais rápido e vermelho
        if(currentHealth <= MaxHealth / 2)
        {
            moveSpeed = angrySpeed;
            spriteRenderer.color = Color.red;
        }

        //Sem vida: o jogador venceu!
        if(currentHealth <= 0)
        {
            SceneManager.LoadScene(VictoryScene);
        }
    }

    // Encostou no corpo do jogador: empurra e machuca (igual ao slime comum)
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

    // O pé do jogador encostou no chefão: foi pisado! O jogador quica e o chefão leva dano
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
