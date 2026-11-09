using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

// =====================================================================
// PlayerController
// Controla o personagem do jogador: andar, pular, atirar, levar dano,
// coletar moedas e trocar as animações.
// Vai no GameObject "Player" (que também tem Rigidbody2D, Collider e Animator).
// =====================================================================
public class PlayerController : MonoBehaviour
{

    // Referência ao GameManager (é encontrada sozinha no Start)
    public GameManager GM;

    // Velocidade para os lados, em blocos por segundo
    public float MoveSpeed = 5f;
    // Velocidade inicial do pulo: quanto maior, mais alto o pulo
    public float JumpForce = 5f;
    // Dano que o jogador causa ao pisar em um inimigo
    public int Damage = 1;

    // Está encostando no chão? ([SerializeField] mostra no Inspector, só para conferir)
    [SerializeField] bool isGrounded;

    [Header("Health")]
    // Vida máxima (cada ponto é um coração)
    public int MaxHealth = 3;
    private int currentHealth;

    [Header("Invincibility")]
    // Segundos que o jogador fica sem levar dano depois de ser atingido
    public float InvincibilityDuration = 1f;
    bool isInvincible;
    float invincibilityTimer;

    [Header("Knockback")]
    // Segundos que o jogador fica sem controle depois de ser empurrado
    public float KnockbackDuration = 1f;
    private bool isKnockedBack;

    [Header("Combat")]
    // Prefab do tiro que será criado ao atacar
    public GameObject BulletPrefab;
    // Quanto tempo a animação de ataque fica tocando
    public float AttackDuration = 0.2f;
    // Tempo mínimo entre um tiro e outro
    public float AttackCooldown = 0.5f;
    float attackTimerDuration;
    float attackTimerCooldown;
    bool isAttacking;

    // Velocidade que será aplicada no Rigidbody2D a cada frame
    Vector2 currentVelocity;
    Rigidbody2D rb;
    Animator animator;

    // Nome da animação que está tocando agora
    string currentState = "player_idle";

    // Start roda uma vez, quando a fase começa
    void Start()
    {
        //Pega os componentes que estão no mesmo GameObject
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
        GM = FindFirstObjectByType<GameManager>();
        //Guarda a posição inicial como ponto de renascimento
        GM.SetPlayerSpawnPoint(transform.position);
        SetState("player_hurt");
    }

    // Troca a animação, mas só se ela for diferente da atual
    // (se chamássemos animator.Play toda hora, a animação reiniciaria sem parar)
    void SetState(string newState)
    {
        if(currentState == newState)
            return;

        currentState = newState;
        animator.Play(currentState);
    }

    // Escolhe a animação certa de acordo com o que o jogador está fazendo
    void UpdateAnimation()
    {
        //Atacando tem prioridade sobre todo o resto
        if (isAttacking)
        {
            SetState("player_atk");
            return;
        }
        //No chão: andando ou parado
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
        //No ar: subindo (pulo) ou descendo (queda)
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

    // Update roda uma vez por frame (várias vezes por segundo)
    void Update()
    {
        //Atualização de HUD
        UpdateLifeFill();

        //Fisica e animação
        //Enquanto está sendo empurrado, o jogador não controla o personagem
        if (isKnockedBack)
        {
            SetState("player_hurt");
            return;
        }

        Gravity();
        Move();
        DoAtk();
        UpdateAnimation();

        //Aplica no corpo físico a velocidade calculada acima
        rb.linearVelocity = currentVelocity;

        InvincibleCounter();
    }

    // Atualiza a barra de corações: vida atual dividida pela vida máxima (valor de 0 a 1)
    // O (float) evita a divisão de inteiros (sem ele, 2/3 daria 0)
    void UpdateLifeFill()
    {
        float lifeFill =(float)currentHealth/MaxHealth;
        GM.SetLifeFill(lifeFill);
    }

    // Conta o tempo de invencibilidade até ele acabar
    // Time.deltaTime é o tempo (em segundos) que passou desde o último frame
    void InvincibleCounter()
    {
        if(isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
                isInvincible = false;
        }
    }

    // Gravidade feita "na mão":
    // no ar, a velocidade para baixo aumenta a cada frame;
    // no chão, a queda é zerada para o jogador não afundar
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

    // Lê as teclas de movimento (A/D ou setas) e de pulo (Espaço)
    void Move()
    {
        //moveInput.x vale -1 (esquerda), 0 (parado) ou 1 (direita)
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        currentVelocity.x = moveInput.x * MoveSpeed;

        //Vira o sprite para o lado em que está andando (escala -1 espelha a imagem)
        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //triggered é verdadeiro só no frame em que a tecla foi apertada
        bool jumpInput = InputSystem.actions["Jump"].triggered;

        //Só pula se estiver no chão (assim não dá para pular no ar)
        if(jumpInput && isGrounded)
        {
            currentVelocity.y = JumpForce;
            //rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    // Quicar: usado quando o jogador pisa em um inimigo
    public void DoBounce(float force)
    {
        isGrounded = false;
        currentVelocity.y = force;
    }

    // Chamado pelo Invoke depois de KnockbackDuration segundos: devolve o controle
    void EndKnockBack()
    {
        isKnockedBack = false;
    }

    // Empurrão: joga o jogador para longe e para cima, e tira o controle por um tempo
    // direction: direção do empurrão (de quem bateu para o jogador)
    public void DoKnockBack(Vector2 direction, float force)
    {
        isKnockedBack = true;
        currentVelocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce((direction + Vector2.up) * force, ForceMode2D.Impulse);
        //Cancela um empurrão anterior que ainda estava contando e começa a contar de novo
        CancelInvoke("EndKnockBack");
        Invoke("EndKnockBack", KnockbackDuration);
    }

    // Atirar: cria o projétil quando o botão de ataque é apertado (Enter ou clique)
    void DoAtk()
    {
        //Diminui o tempo de espera entre um tiro e outro
        if(attackTimerCooldown > 0)
            attackTimerCooldown -= Time.deltaTime;

        if(InputSystem.actions["Attack"].triggered && attackTimerCooldown <= 0)
        {
            isAttacking = true;
            attackTimerDuration = AttackDuration;
            //Instantiate cria uma cópia do prefab do tiro na posição do jogador
            GameObject projectileGO = Instantiate(BulletPrefab, transform.position, transform.rotation);
            PlayerProjectile playerProjectile = projectileGO.GetComponent<PlayerProjectile>();
            //Se o jogador está virado para a esquerda, o tiro também vai para a esquerda
            if(transform.localScale.x < 0)
            {
                playerProjectile.Speed *= -1;
            }
            attackTimerCooldown = AttackCooldown;
        }

        //Conta quanto tempo falta para a animação de ataque terminar
        if(isAttacking)
        {
            attackTimerDuration -= Time.deltaTime;
            if(attackTimerDuration <= 0)
            {
                isAttacking = false;
            }
        }
    }

    // Leva dano (chamado pelos inimigos, pelo chefão e pelos espinhos)
    public void TakeDamage(int damage)
    {
        //Se ainda está invencível, ignora o dano
        if(isInvincible) return;

        currentHealth -= damage;
        UpdateLifeFill();
        //Começa o tempo de invencibilidade
        isInvincible = true;
        invincibilityTimer = InvincibilityDuration;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    // Morreu: avisa o GameManager, que decide se o jogador renasce ou se é Game Over
    public void Die()
    {
        GM.PlayerDied();
    }

    // Enche a vida de novo (usado ao renascer)
    public void RecoverHealth()
    {
        currentHealth = MaxHealth;
    }

    // Chamado quando um colisor do jogador (o corpo ou o "Foot") entra em um Trigger
    // ou quando um Trigger do jogador (o "Foot") encosta em outro colisor
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Encostou em uma moeda: soma no contador e apaga a moeda
        if(collision.tag == "Coin")
        {
            GM.CollectCoin();
            Destroy(collision.gameObject);
        }
        //O pé encostou em algo da layer "Ground" (chão, plataforma, caixa): está no chão
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    // Chamado quando o jogador deixa de encostar em algo
    void OnTriggerExit2D(Collider2D collision)
    {
        //Saiu do chão: está no ar
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
