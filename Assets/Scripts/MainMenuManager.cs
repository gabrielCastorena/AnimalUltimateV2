using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("MainMenuManager")]
    [SerializeField] private string gameSceneName;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenMaps()
    {
        Debug.Log("PROXIMAMENTE...");
    }
}