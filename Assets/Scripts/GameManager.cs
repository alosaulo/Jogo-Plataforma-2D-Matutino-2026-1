using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PnlStart;
    public GameObject PnlGameOver;
    public TextMeshProUGUI txtCoin;
    public Image imgLifeFiller;


    public int PlayerLives;
    Vector3 PlayerSpawnPoint;

    PlayerController player;

    [Header("Collections")]
    public int CollectedCoins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        txtCoin.text = CollectedCoins.ToString();
    }

    public void CollectCoin()
    {
        CollectedCoins += 1;
    }

    public void SetLifeFill(float value)
    {
        imgLifeFiller.fillAmount = value;
    }

    public void SetPlayerLives(int value)
    {
        PlayerLives += value;
    }

    public void SetPlayerSpawnPoint(Vector3 newPosition)
    {
        PlayerSpawnPoint = newPosition;
    }

    public void RespawnPlayer()
    {
        player.transform.position = PlayerSpawnPoint;
        player.RecoverHealth();
    }

    void GameOver()
    {
        PnlGameOver.SetActive(true);
    }

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
