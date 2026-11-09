using UnityEngine;

// =====================================================================
// CameraFollow
// Faz a câmera seguir o jogador.
// Vai na "Main Camera" de cada fase.
// =====================================================================
public class CameraFollow : MonoBehaviour
{
    // Distância da câmera em relação ao jogador
    // (ex: y = 2 deixa a câmera um pouco acima; z = -10 deixa a câmera "na frente" da cena)
    public Vector3 offset;
    PlayerController player;
    // Start roda uma vez: encontra o jogador na cena
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update roda uma vez por frame: coloca a câmera na posição do jogador + offset
    void Update()
    {
        Vector3 newPos = player.transform.position + offset;
        transform.position = newPos;
    }
}
