using UnityEngine;

// =====================================================================
// MovingPlatform
// Plataforma que vai e volta entre dois pontos e carrega o jogador.
// Vai no prefab "MovingPlatform" (layer "Ground", com um BoxCollider2D).
// =====================================================================
public class MovingPlatform : MonoBehaviour
{
    // Quanto a plataforma anda a partir do ponto inicial (ex: x = 4 anda 4 blocos para a direita)
    // Para uma plataforma que sobe e desce, use y no lugar de x
    public Vector3 Distance = new Vector3(4, 0, 0);
    // Velocidade, em blocos por segundo
    public float Speed = 2;

    Vector3 startPosition;
    Vector3 endPosition;
    // Está indo para o ponto final (true) ou voltando para o início (false)?
    bool goingToEnd = true;

    // Start roda uma vez: calcula os dois pontos do caminho
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + Distance;
    }

    // Update roda uma vez por frame: anda um pouquinho em direção ao destino
    void Update()
    {
        //Escolhe o destino: o ponto final ou o ponto inicial
        Vector3 target = endPosition;
        if(goingToEnd == false)
        {
            target = startPosition;
        }

        //MoveTowards anda no máximo "Speed * Time.deltaTime" em direção ao destino, sem passar dele
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);

        //Chegou no destino: volta pelo outro lado
        if(transform.position == target)
        {
            goingToEnd = !goingToEnd;
        }
    }

    //O jogador vira "filho" da plataforma para ser carregado junto com ela
    //(quando o "pai" se move, os "filhos" se movem junto)
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Só se o jogador estiver em cima (e não batendo por baixo ou pelo lado)
        if(collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    //O jogador saiu da plataforma (pulou ou andou para fora): deixa de ser filho
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
