using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private BossController bossController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossController = GetComponentInParent<BossController>();
    }

    public void TakeDamage(float damage)
    {
        bossController.TakeDamage(damage);
    }
}
