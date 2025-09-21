using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public void LoadTitleScene()
    {
        Debug.Log("Loading Title Scene");
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadState1()
    {
        PlayerPrefs.SetFloat("Score", 0);
        PlayerPrefs.SetInt("Health", 100);
        PlayerPrefs.Save();
        SceneManager.LoadScene("State1");
    }

    public void LoadState2()
    {
        SceneManager.LoadScene("State2");
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
