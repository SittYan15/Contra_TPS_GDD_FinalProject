using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f; // Speed of the bullet
    [SerializeField] private float damage = 20f;

    [Header("VFX")]
    [SerializeField] private Transform vfxHitOther;
    [SerializeField] private Transform vfxHitEnemy;

    [Header("PowerUps")]
    [SerializeField] private Transform healthUpPrefab;

    private Rigidbody rb;

    private EnemyController enemy;
    private NightshadeController nightshade;

    private float height = -100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * speed; // Set the bullet speed
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < height)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox"))
        {
            return; // Ignore collisions with hit boxes
        }

        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<EnemyController>();
            enemy.TakeDamage(damage);

            Instantiate(vfxHitEnemy, transform.position, Quaternion.identity);

        }
        else if (other.CompareTag("NightShade"))
        {
            nightshade = other.GetComponent<NightshadeController>();
            nightshade.TakeDamage(damage);

            Instantiate(vfxHitEnemy, transform.position, Quaternion.identity);
        }
        else if (other.CompareTag("PowerBox"))
        {
            Destroy(other.gameObject);
            Instantiate(healthUpPrefab, other.transform.position, Quaternion.identity);
        }
        else if (other.CompareTag("BossHeart"))
        {
            other.GetComponent<HeartController>().TakeDamage(damage);
            Instantiate(vfxHitEnemy, transform.position, Quaternion.identity);
        }
        else if (other.CompareTag("Heart2"))
        {
            other.GetComponent<State2HeartController>().TakeDamage(damage);
            Instantiate(vfxHitEnemy, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Hit something else: " + other.name);
            Instantiate(vfxHitOther, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Destroy the bullet on impact
    }   
}
