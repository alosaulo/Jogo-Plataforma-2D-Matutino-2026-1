using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================================
// LevelExit
// Saída da fase: quando o jogador encosta, carrega a próxima fase.
// Vai na placa "Saida", que tem um BoxCollider2D marcado como Trigger.
// =====================================================================
public class LevelExit : MonoBehaviour
{
    // Nome da cena que será carregada (ex: "Fase2")
    // É digitado no Inspector, então o mesmo script serve para todas as fases
    public string NextLevel;

    // Algo entrou no Trigger da placa: se for o jogador, troca de fase
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SceneManager.LoadScene(NextLevel);
        }
    }
}
