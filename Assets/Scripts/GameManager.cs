using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// =====================================================================
// GameManager
// O "juiz" da fase: guarda as moedas e as vidas, atualiza o HUD,
// faz o jogador renascer e mostra a tela de Game Over.
// Vai em um GameObject vazio chamado "GameManager" (um em cada fase).
// =====================================================================
public class GameManager : MonoBehaviour
{
    // Painel de Game Over (fica desligado até o jogador perder todas as vidas)
    public GameObject PnlGameOver;
    // Texto do HUD que mostra quantas moedas foram coletadas
    public TextMeshProUGUI txtCoin;
    // Imagem dos corações que vai "esvaziando" conforme o jogador perde vida
    public Image imgLifeFiller;


    // Quantas vidas o jogador tem nesta fase
    public int PlayerLives;
    // Onde o jogador renasce depois de morrer
    Vector3 PlayerSpawnPoint;

    PlayerController player;

    [Header("Collections")]
    // Quantas moedas foram coletadas nesta fase
    public int CollectedCoins;

    // Start roda uma vez, quando a fase começa
    void Start()
    {
        //Garante que o jogo não comece pausado (o Game Over pausa o tempo)
        Time.timeScale = 1;
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update roda uma vez por frame: mantém o número de moedas na tela atualizado
    void Update()
    {
        txtCoin.text = CollectedCoins.ToString();
    }

    // Chamado pelo jogador quando ele pega uma moeda
    public void CollectCoin()
    {
        CollectedCoins += 1;
    }

    // Enche a barra de corações (value vai de 0 = vazio até 1 = cheio)
    public void SetLifeFill(float value)
    {
        imgLifeFiller.fillAmount = value;
    }

    // Soma (ou subtrai, se value for negativo) vidas do jogador
    public void SetPlayerLives(int value)
    {
        PlayerLives += value;
    }

    // Guarda o ponto onde o jogador vai renascer
    public void SetPlayerSpawnPoint(Vector3 newPosition)
    {
        PlayerSpawnPoint = newPosition;
    }

    // Leva o jogador de volta ao ponto de renascimento com a vida cheia
    public void RespawnPlayer()
    {
        player.transform.position = PlayerSpawnPoint;
        player.RecoverHealth();
    }

    // Mostra o painel de Game Over e pausa o jogo
    // (timeScale = 0 congela tudo que usa Time.deltaTime e a física)
    void GameOver()
    {
        PnlGameOver.SetActive(true);
        Time.timeScale = 0;
    }

    // Botão "Tentar de novo": recarrega a fase atual do zero
    public void Restart()
    {
        //Recarrega a fase atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Chamado quando o jogador morre: perde uma vida e renasce,
    // ou, se acabaram as vidas, é Game Over
    public void PlayerDied()
    {
        PlayerLives--;
        if(PlayerLives > 0)
        {
            RespawnPlayer();
        }
        else
        {
            GameOver();
        }
    }

}
