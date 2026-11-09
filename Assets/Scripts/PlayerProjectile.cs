using UnityEngine;

// =====================================================================
// PlayerProjectile
// O tiro do jogador: anda em linha reta, causa dano no primeiro inimigo
// que acertar e some depois de alguns segundos.
// Vai no prefab PlayerProjectile (criado pelo PlayerController ao atacar).
// =====================================================================
public class PlayerProjectile : MonoBehaviour
{

    // Velocidade do tiro (negativa = vai para a esquerda)
    public float Speed = 5f;
    // Dano do tiro
    public int Damage = 1;
    // Segundos até o tiro sumir sozinho, caso não acerte nada
    public float Lifetime = 2f;

    Rigidbody2D rb;

    // Start roda uma vez, logo que o tiro é criado
    void Start()
    {
        //Indo para a esquerda: espelha o desenho do tiro
        if(Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        rb = GetComponent<Rigidbody2D>();
        //transform.right é a direção "para a direita" do objeto
        rb.linearVelocity = transform.right * Speed;
        //Apaga o tiro depois de Lifetime segundos
        Destroy(gameObject, Lifetime);
    }

    // Update não é usado neste script
    void Update()
    {

    }

    // O tiro (que é um Trigger) encostou em alguma coisa
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Código desativado: ignoraria o chão
        /*LayerMask layerGround = 6;
        if(collision.gameObject.layer == layerGround)
        {
            return;
        }*/

        //Encostou no próprio jogador: ignora e continua voando
        if(collision.tag == "Player" ||
            collision.tag == "PlayerFoot" ||
            collision.tag == "PlayerAtk")
        {
            return;
        }

        //Acertou um inimigo: descobre qual tipo de inimigo é e causa dano
        if(collision.tag == "Enemy")
        {
            //É um slime comum?
            SlimeEnemy slimeEnemy = collision.GetComponent<SlimeEnemy>();
            if(slimeEnemy != null)
            {
                slimeEnemy.TakeDamage(1);
            }

            //É o chefão?
            BossSlime boss = collision.GetComponent<BossSlime>();
            if(boss != null)
            {
                boss.TakeDamage(1);
            }
        }

        //Acertou qualquer coisa (inimigo, chão, parede): o tiro some
        Destroy(gameObject);
    }

}
