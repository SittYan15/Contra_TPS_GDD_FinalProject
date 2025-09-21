using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Floating Settings")]
    [SerializeField] private float floatAmplitude = 0.25f; // Height of the float
    [SerializeField] private float floatFrequency = 1f;    // Speed of the float
    [SerializeField] private float rotationSpeed = 90f;    // Degrees per second

    private Vector3 startPosition;

    [SerializeField] private bool rotate = false;
    [SerializeField] private bool enter = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.back * 5, ForceMode.VelocityChange);
        Invoke(nameof(EnableCollisionEnter), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotate) return;

        // Rotate around Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);

        // Floating effect
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rotate) return;

        if (!enter) return;

        rotate = true;
        startPosition = transform.position;
    }

    private void EnableCollisionEnter()
    {
        enter = true;
    }
}
