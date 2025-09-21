using TMPro;
using UnityEngine;

public class ScoreTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Score: " + PlayerPrefs.GetFloat("Score", 0).ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
