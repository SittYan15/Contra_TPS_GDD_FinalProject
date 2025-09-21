using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Potal")]
    [SerializeField] private GameObject spawnPotal;

    [Header("Enemy Mutant")]
    private float offsetPosition = 0.5f;
    [SerializeField] private GameObject mutantEnemy;
    [SerializeField] private GameObject nightShade;

    [Header("PlayerScore")]
    [SerializeField] private float score;
    [SerializeField] private TextMeshProUGUI textMeshScore;

    private Vector3 spawnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = PlayerPrefs.GetFloat("Score", 0);
        textMeshScore.text = "Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void SpawnEnemyFirstPhase(Vector3 spawnPosition, float enemyType)
    {
        this.spawnPosition = spawnPosition;

        if (enemyType == 1)
        {
            Instantiate(spawnPotal, this.spawnPosition, spawnPotal.transform.rotation);
            //Instantiate(spawnPotal, spawnLocationFirstPhase.position, spawnPotal.transform.rotation);
            Invoke(nameof(SpawnMutantEnemy), 1f);
        }
        else if (enemyType == 2)
        {
            Instantiate(spawnPotal, this.spawnPosition, spawnPotal.transform.rotation);
            //Instantiate(spawnPotal, spawnLocationFirstPhase.position, spawnPotal.transform.rotation);
            Invoke(nameof(SpawnNightShade), 1f);
        }
    }

    public void AddPlayerScore(float points)
    {
        score += points;
        textMeshScore.text = "Score: " + score.ToString();
    }

    public float GetPlayerScore()
    {
        return score;
    }

    private void SpawnMutantEnemy()
    {
        Instantiate(mutantEnemy, this.spawnPosition + new Vector3(0, offsetPosition, 0), mutantEnemy.transform.rotation);
        //Instantiate(mutantEnemyPrafab, spawnLocationFirstPhase.position + new Vector3(0, offsetPosition, 0), mutantEnemyPrafab.transform.rotation);
    }

    private void SpawnNightShade()
    {
        Instantiate(nightShade, this.spawnPosition, nightShade.transform.rotation);
        //Instantiate(mutantEnemyPrafab, spawnLocationFirstPhase.position + new Vector3(0, offsetPosition, 0), mutantEnemyPrafab.transform.rotation);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("State1");
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
