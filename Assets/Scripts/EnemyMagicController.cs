using UnityEngine;

public class NightshadeController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minDistanceToAttack = 10f;

    [Header("Health")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float health = 200f;
    [SerializeField] private float killPoints = 30f;

    [Header("VFX")]
    [SerializeField] private GameObject dieVFX;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    [Header("Magic Attack")]
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private float offsetHeight = 1.3f; // Height offset to aim at player's upper body
    private float nextFireTime;

    private Animator animator;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        healthBar.SetMaxHealth((int)maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Get direction to player (ignoring Y axis so it doesn�t tilt up/down)
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f) // Prevent jitter if very close
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, player.position) < minDistanceToAttack)
        {
            OnAttack();
        }

        if (player == null) return;

        // Check if it's time to shoot
        if (Time.time >= nextFireTime)
        {
            animator.SetTrigger("isAttack");
            nextFireTime = Time.time + 1f / fireRate;
        }

    }

    public void ShootAtPlayer()
    {
        Vector3 targetPos = player.position + Vector3.up * offsetHeight;

        Vector3 direction = (targetPos - firePoint.position).normalized;

        Instantiate(magicProjectilePrefab, firePoint.position, Quaternion.LookRotation(direction, Vector3.up));


        //GameObject projectile = Instantiate(magicProjectilePrefab, firePoint.position, Quaternion.identity);

        //Vector3 direction = (player.position - firePoint.position).normalized;

        //Rigidbody rb = projectile.GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.linearVelocity = direction * projectileSpeed;
        //}

        //projectile.transform.rotation = Quaternion.LookRotation(direction);

        //Destroy(projectile, 5f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth((int)health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Disable further actions
        this.enabled = false;

        gameManager.AddPlayerScore(killPoints);

        audioSource.loop = false;
        audioSource.clip = deathSound;
        audioSource.Play();

        Instantiate(dieVFX, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnAttack()
    {

    }
}
