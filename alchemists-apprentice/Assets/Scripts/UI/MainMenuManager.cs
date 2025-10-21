using UnityEngine;
using UnityEngine.SceneManagement;  // Needed for scene loading

public class MainMenuManager : LoggingMonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Town");
    }

    public void LoadGame()
    {
        log("Load Game clicked - not implemented yet");
    }

    public void QuitGame()
    {
        log("Quit Game clicked");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
