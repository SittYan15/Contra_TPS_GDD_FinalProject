using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f; // Speed of the bullet
    [SerializeField] private float damage = 20f;

    [Header("VFX")]
    [SerializeField] private Transform vfxHitOther;
    [SerializeField] private Transform vfxHitPlayer;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;
    private AudioSource audioSource;

    [Header("Player")]
    [SerializeField] private float playerOffsetTarget = 1.2f;

    private Rigidbody rb;

    private PlayerController player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * speed; // Set the bullet speed
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.resource = impactSound;
            audioSource.Play();

            Quaternion hitRotation = Quaternion.LookRotation(transform.forward);

            player = other.GetComponent<PlayerController>();
            player.MagicDamage(damage);

            Instantiate(vfxHitPlayer, other.transform.position + Vector3.up * playerOffsetTarget, hitRotation);

        }
        else if (other.CompareTag("HitBox"))
        {
            return;
        }
        else
        {
            Debug.Log("Hit something else: " + other.name);
            Instantiate(vfxHitOther, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Destroy the bullet on impact
    }
}
