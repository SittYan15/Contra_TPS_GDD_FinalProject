using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Transform spawnPosition;

    [SerializeField] private float enemyType;

    [SerializeField] private GameObject parentObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObject = transform.parent.gameObject;
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered spawn point trigger.");   
            gameManager.SpawnEnemyFirstPhase(spawnPosition.transform.position, enemyType);
            Destroy(parentObject);
        }
    }
}

