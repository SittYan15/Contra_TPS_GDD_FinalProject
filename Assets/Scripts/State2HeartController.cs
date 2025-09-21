using UnityEngine;

public class State2HeartController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float currentHealth = 200f;

    [SerializeField] private RoomController roomController;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject destroyVFX;
    [SerializeField] private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomController = GetComponentInParent<RoomController>();
        healthBar.SetMaxHealth((int)maxHealth);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth((int)currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(destroyVFX, transform.position, Quaternion.identity);
        audioSource.Play();
        roomController.heartDestoryed();
    }
}
