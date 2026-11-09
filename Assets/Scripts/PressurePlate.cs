using UnityEngine;

// =====================================================================
// PressurePlate
// Botão de pressão do puzzle: enquanto uma caixa estiver em cima dele,
// a porta fica aberta. Só funciona com objetos que têm a tag "Box"
// (o jogador sozinho não abre a porta).
// Vai no GameObject "Botao", que tem um BoxCollider2D marcado como Trigger.
// =====================================================================
public class PressurePlate : MonoBehaviour
{
    // Porta que abre enquanto tiver uma caixa em cima do botão
    // (arraste o GameObject da porta para este campo no Inspector)
    public GameObject Door;

    // Uma caixa chegou no botão: desliga a porta (ela some e deixa passar)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Box")
        {
            Door.SetActive(false);
        }
    }

    // A caixa saiu do botão: liga a porta de novo (ela volta a bloquear)
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Box")
        {
            Door.SetActive(true);
        }
    }
}
