using UnityEngine;

public class EnemyLeftHitBox : MonoBehaviour
{
    EnemyController enemyController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyController.HitByHitBox(0); // 0 for left hit box
        }
    }
}
