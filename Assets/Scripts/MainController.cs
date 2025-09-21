using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private List<GameObject> activeEnemy;

    [SerializeField] private GameObject destoryEffect1;
    [SerializeField] private GameObject destoryEffect2;

    [SerializeField] private TextMeshProUGUI winText;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableEnemy(int index)
    {
        if (index < 0 || index >= activeEnemy.Count)
        {
            Debug.LogError("Index out of range: " + index);
            return;
        }
        activeEnemy[index].SetActive(true);
    }

    public void WinGame()
    {
        winText.gameObject.SetActive(true);
        PlayerPrefs.SetFloat("Score", gameManager.GetPlayerScore());
        PlayerPrefs.Save();

        Invoke(nameof(LoadEndScene), 3f);
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
