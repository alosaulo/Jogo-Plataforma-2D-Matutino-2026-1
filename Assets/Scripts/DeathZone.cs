using UnityEngine;

// =====================================================================
// DeathZone
// Área invisível embaixo da fase: se o jogador cair em um buraco
// e encostar nela, morre na hora (perde uma vida).
// Vai no GameObject "DeathZone", que tem um BoxCollider2D marcado como Trigger.
// =====================================================================
public class DeathZone : MonoBehaviour
{
    // Start não é usado neste script
    void Start()
    {

    }

    // Update não é usado neste script
    void Update()
    {

    }

    // Algo entrou na área: se for o jogador, ele morre
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if(player == null)
                return;
            player.Die();
        }
    }
}
