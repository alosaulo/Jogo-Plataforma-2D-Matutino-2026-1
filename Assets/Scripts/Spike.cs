using UnityEngine;

// =====================================================================
// Spike
// Espinho: machuca o jogador (tira vida) e empurra ele para longe.
// Diferente da DeathZone, não mata na hora.
// Vai no prefab "Spike", que tem um BoxCollider2D marcado como Trigger.
// =====================================================================
public class Spike : MonoBehaviour
{
    // Quanto de vida o espinho tira
    public int Damage = 1;
    // Força do empurrão
    public float KnockbackForce = 4;

    // Algo encostou no espinho
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Só reage ao corpo do jogador (o pé tem outra tag, "PlayerFoot")
        if(collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if(player == null)
                return;

            //Empurra o jogador para longe do espinho e tira vida
            //Direção do empurrão: do espinho para o jogador
            Vector3 knockBackDir = collision.transform.position - transform.position;
            player.DoKnockBack(knockBackDir, KnockbackForce);
            player.TakeDamage(Damage);
        }
    }
}
