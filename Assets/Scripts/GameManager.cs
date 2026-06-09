using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Collections")]
    public int CollectedCoins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectCoin()
    {
        CollectedCoins += 1;
    }

}
