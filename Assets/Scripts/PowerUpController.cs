using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("PowerUps")]
    [SerializeField] private float healthRestoreAmount = 20f;

    [Header("Parent")]
    [SerializeField] private GameObject parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.RestoreHealth(healthRestoreAmount);
            Destroy(parent);
        }
    }
}
