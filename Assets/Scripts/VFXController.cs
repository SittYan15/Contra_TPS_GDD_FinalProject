using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip endClip;

    [SerializeField] private float delayBeforeEndClip = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        audioSource.clip = introClip;
        audioSource.Play();

        Invoke(nameof(PlayEndClip), delayBeforeEndClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayEndClip()
    {
        audioSource.Stop();
        audioSource.clip = endClip;
        audioSource.Play();
    }
}
