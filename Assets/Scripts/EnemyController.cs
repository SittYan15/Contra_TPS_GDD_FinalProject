using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float killPoints = 15f;
    private float nextFireTime;

    [Header("Colliders")]
    [SerializeField] private Collider leftHitBoxCollider;
    [SerializeField] private EnemyColliderDetection colliderDetection;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("VFX")]
    [SerializeField] private Transform vfxSlash;
    [SerializeField] private Transform vfxLeftLocation;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioSource walkingAudioSource;

    private bool isDead = false;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider col;

    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        colliderDetection = GetComponentInChildren<EnemyColliderDetection>();

        healthBar.SetMaxHealth((int)maxHealth);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Invoke(nameof(Run), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !isDead) Die();
    }

    private void Run()
    {
        if (!isDead)
        {
            StartWalkingSound();
            animator.SetBool("isRun", true);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.SetHealth((int)health);
    }

    private void Die()
    {
        isDead = true;

        gameManager.AddPlayerScore(killPoints);

        StopWalkingSound();

        audioSource.clip = deathSound;
        audioSource.Play();

        rb.useGravity = false; // Disable gravity
        col.enabled = false; // Disable the collider
        colliderDetection.enabled = false;
        animator.SetBool("isDie", true);

        Invoke(nameof(OnDestory), 3f);
    }

    public void StartWalkingSound()
    {
        walkingAudioSource.Play();
    }

    public void StopWalkingSound()
    {
        walkingAudioSource.Stop();
    }

    public void Attack()
    {

        if (Time.time >= nextFireTime && !isDead)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("big_punch")) // optional safety
            {
                audioSource.clip = attackSound;
                audioSource.Play();

                StopWalkingSound();
                animator.ResetTrigger("punch"); // reset before setting again
                animator.SetTrigger("punch");

                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void OnDestory()
    {
        Destroy(gameObject);
    }

    public void StartSlashVfx()
    {
        Instantiate(vfxSlash, vfxLeftLocation.transform.position, transform.rotation);
    }

    public void HitByHitBox(float type)
    {
        if (type == 0) // Left hit box
        {
            player.GetComponent<PlayerController>().TakeDamage(attackDamage, transform);
        }
        // Add more conditions for other hit box types if needed
    }

    public bool IsDeadStatus()
    {
        return isDead;
    }

    public void EnableLeftHitBox()
    {
        leftHitBoxCollider.enabled = true;
    }

    public void DisableLeftHitBox()
    {
        leftHitBoxCollider.enabled = false;
    }
}
