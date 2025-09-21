using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth = 500f;
    [SerializeField] private float health = 500f;
    [SerializeField] private GameObject heart;
    [SerializeField] private float respawnTime = 2f;

    [Header("Spawn Points")]
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private Transform spawnPoint3;
    [SerializeField] private Transform spawnPoint4;
    [SerializeField] private Transform spawnPoint5;

    [Header("VFX")]
    [SerializeField] private GameObject dieVFX1;
    [SerializeField] private GameObject dieVFX2;
    [SerializeField] private Transform vfxPosition;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    private GameManager gameManager;

    private bool isDead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthBar.SetMaxHealth((int)maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        healthBar.SetHealth((int)health);

        if (health < 450 & respawnTime == 2)
        {
            respawnTime = 1f;
            gameManager.SpawnEnemyFirstPhase(spawnPoint4.position, 2);
            Invoke(nameof(SpawnPosition5), 2f);
        }

        if (health < 200 & respawnTime == 1)
        {
            respawnTime = 0f;
            gameManager.SpawnEnemyFirstPhase(spawnPoint1.position, 2);
            Invoke(nameof(SpawnLeftPosition), 2f);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void SpawnPosition5()
    {
        gameManager.SpawnEnemyFirstPhase(spawnPoint5.position, 2);
    }

    private void SpawnLeftPosition()
    {
        gameManager.SpawnEnemyFirstPhase(spawnPoint2.position, 2);
        Invoke(nameof(SpawnRightPosition), 2f);
    }

    private void SpawnRightPosition()
    {
        gameManager.SpawnEnemyFirstPhase(spawnPoint3.position, 2);
    }

    private void Die()
    {
        isDead = true;
        Destroy(heart);

        audioSource.clip = explosionSound;
        audioSource.Play();

        Instantiate(dieVFX1, vfxPosition.position, Quaternion.identity);
        Instantiate(dieVFX2, vfxPosition.position, Quaternion.identity);

        Invoke(nameof(LoadScene2), 3f);
    }

    private void LoadScene2()
    {
        PlayerPrefs.SetFloat("Score", gameManager.GetPlayerScore());
        SceneManager.LoadScene("State2");
    }
}
