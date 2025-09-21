using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Main")]
    public CharacterController controller;
    public Transform camera;
    public Animator animator;
    public bool isAiming = false;

    [Header("Health")]
    [SerializeField] private bool isDead = false;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float health = 100f;

    [Header("Grivaty & Jump")]
    [SerializeField] private float gravity = -9.81f; // Gravity value
    [SerializeField] private Transform groundCheck; // Transform to check if the player is grounded
    [SerializeField] private float groundDistance = 0.4f; // Distance to check for ground
    [SerializeField] private LayerMask groundMask; // Layer mask to identify ground layers
    [SerializeField] private bool isGrounded; // Is the player grounded 
    private Vector3 gravityVelocity; // Player's velocity

    [SerializeField] private float jumpHeight = 3f; // Jump height

    private Vector3 moveDirection;

    [Header("Movement")]
    [SerializeField] private bool isMoving;
    [SerializeField] private float speed;
    [SerializeField] private bool isRunning;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isSprinting;
    [SerializeField] private float sprintSpeed;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera thirdPersonCamera;
    [SerializeField] private CinemachineCamera aimCinemachineCamera;

    [Header("Shooting Target")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private ParticleSystem muzzleFlash;
    private Vector3 mouseWorldPosition;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    [Header("Animation Rigging")]
    [SerializeField] private Rig aimRig;
    private float aimRigWeight;

    [Header("SFX Footsteps")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float walkFootstepInterval = 0.53f;
    [SerializeField] private float runFootstepInterval = 0.25f;
    [SerializeField] private float footstepInterval = 0.53f; // Time between footsteps

    private float footstepTimer = 0f;

    [Header("SFX Gun")]
    [SerializeField] private AudioSource gunAudioSource;

    [Header("Others")]
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    private float horizontal;
    private float vertical;

    private float targetBlendSpeed;
    private float blendSpeed = 1f;
    private float blendAcceleartion = 10f;
    private float blendVer = 0f;
    private float blendHor = 0f;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        health = PlayerPrefs.GetFloat("Health", maxHealth);
        healthBar.SetHealth(Mathf.RoundToInt(health));
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = isRunning ? runSpeed : speed;

        float footstepInterval = isRunning ? runFootstepInterval : walkFootstepInterval;

        SetAnimation();

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !isDead)
        {
            float targetAngle;

            if (isAiming)
            {
                targetAngle = camera.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Move in all directions relative to the camera while aiming
                moveDirection = camera.right * horizontal + camera.forward * vertical;
                moveDirection.y = 0f; // Prevent vertical movement

            }
            else
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f; // Reset vertical velocity when grounded
        }
        animator.SetBool("isGrounded", isGrounded);

        gravityVelocity.y += gravity * Time.deltaTime; // Apply gravity
        controller.Move(gravityVelocity * Time.deltaTime); // Apply gravity to the character controller

        ShootingTarget();

        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);

        // Footstep SFX logic
        if (isMoving && isGrounded)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = footstepInterval; // Reset timer when not moving
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0 || footstepAudioSource == null) return;
        int index = Random.Range(0, footstepClips.Length);
        footstepAudioSource.clip = footstepClips[index];
        footstepAudioSource.Play();
    }

    public void TakeDamage(float damage, Transform enemyTransform)
    {
        controller.Move(enemyTransform.forward * 5f); // Knockback effect
        health -= damage;
        healthBar.SetHealth(Mathf.RoundToInt(health));

        if (health <= 0) 
        {
            isDead = true;
            Die();
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        healthBar.SetHealth(Mathf.RoundToInt(health));
    }

    public void MagicDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(Mathf.RoundToInt(health));

        if (health <= 0)
        {
            Debug.Log("Player died.");
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);

        SavePlayerData();

        Invoke(nameof(LoadGameOverScene), 3f);
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetFloat("Health", health);
        PlayerPrefs.SetFloat("Score", gameManager.GetPlayerScore());
        PlayerPrefs.Save();
    }

    private void LoadGameOverScene()
    {
        SavePlayerData();
        SceneManager.LoadScene("GameOverScene");
    }

    public void ShootingTarget()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //Vector3 targetPosition = raycastHit.point;
            //Vector3 aimDirection = (targetPosition - spawnBulletPosition.position).normalized;
            //spawnBulletPosition.forward = aimDirection;

            // from vdo
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
    }

    public void Shooting(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isDead) return;

        Instantiate(muzzleFlash, spawnBulletPosition.position, spawnBulletPosition.rotation);
        gunAudioSource.Play();

        if (isAiming)
        {
            Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        }
        else
        {
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, spawnBulletPosition.rotation);
        }
    }

    private void SetGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    public void SetJump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            animator.SetTrigger("onJump");
        }
    }

    public void ActualJump()
    {
        gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
    }

    private void SetAnimation()
    {
        targetBlendSpeed = isRunning ? 2f : 1f;
        blendSpeed = Mathf.Lerp(blendSpeed, targetBlendSpeed, Time.deltaTime * 5f); // 5f = speed of transition
        isMoving = Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f;

        // Send values to Animator
        if (animator != null)
        {
            animator.SetFloat("horizontal", horizontal);
            animator.SetFloat("vertical", vertical);
            animator.SetBool("run", isRunning);
            animator.SetBool("aim", isAiming);
            animator.SetBool("move", isMoving);

            if (isAiming)
            {
                // Vertical blend
                if (vertical > 0f)
                    blendVer += Time.deltaTime * blendAcceleartion;
                else if (vertical < 0f)
                    blendVer -= Time.deltaTime * blendAcceleartion;
                else
                    blendVer = Mathf.MoveTowards(blendVer, 0f, Time.deltaTime * blendAcceleartion);

                // Horizontal blend
                if (horizontal > 0f)
                    blendHor += Time.deltaTime * blendAcceleartion;
                else if (horizontal < 0f)
                    blendHor -= Time.deltaTime * blendAcceleartion;
                else
                    blendHor = Mathf.MoveTowards(blendHor, 0f, Time.deltaTime * blendAcceleartion);

                // Clamp both immediately after updating
                blendVer = Mathf.Clamp(blendVer, -blendSpeed, blendSpeed);
                blendHor = Mathf.Clamp(blendHor, -blendSpeed, blendSpeed);
            }
            else
            {
                blendVer = 0f;
                blendHor = 0f;
            }

            animator.SetFloat("blendVer", blendVer);
            animator.SetFloat("blendHor", blendHor);
        }
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    public void SetSprint()
    {
        isRunning = !isRunning;
    }

    public void SetAim()
    {
        isAiming = !isAiming;
        if (isAiming)
        {
            aimCinemachineCamera.gameObject.SetActive(true);
            crosshair.SetActive(true);
            //aimCinemachineCamera.gameObject.transform.rotation = transform.rotation;
            aimRigWeight = 1f;
        }
        else
        {
            //thirdPersonCamera.gameObject.transform.rotation = camera.gameObject.transform.rotation;
            crosshair.SetActive(false);
            aimCinemachineCamera.gameObject.SetActive(false);
            aimRigWeight = 0f;
        }
    }
}
