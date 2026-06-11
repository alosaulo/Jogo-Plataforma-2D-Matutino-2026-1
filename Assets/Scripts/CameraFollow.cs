using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.transform.position + offset;
        transform.position = newPos;
    }
}
