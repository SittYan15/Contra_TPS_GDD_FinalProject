using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<GameObject> heart;
    [SerializeField] private List<bool> bools;

    [SerializeField] private GameObject desotryVFX;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private bool finalRoom = false;
    [SerializeField] private int roomIndex;
    [SerializeField] private MainController mainController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainController = GameObject.Find("Rooms").GetComponent<MainController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heartDestoryed()
    {
        bools.Add(true);

        if (bools.Count == heart.Count)
        {
            mainController.EnableEnemy(roomIndex);

            Instantiate(desotryVFX, heart[0].transform.position, Quaternion.identity);
            audioSource.Play();
            
            if (finalRoom)
            {
                mainController.WinGame();
                Destroy(gameObject, 1f);
            } else
            {
                Destroy(gameObject, 1f);
            }
        }
    }
}
