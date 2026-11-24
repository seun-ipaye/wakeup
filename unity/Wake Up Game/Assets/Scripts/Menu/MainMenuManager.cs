using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles buttons on main menu
/// </summary>

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Sets GameModeManager to Story mode and loads HouseScene
    /// </summary>
    public void StartStoryMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Story;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Story mode started");
    }

    /// <summary>
    /// Sets GameModeManager to Hardcore mode and loads HouseScene
    /// </summary>
    public void StartHardcoreMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Hardcore;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Hardcore mode started");
    }

    /// <summary>
    /// Quits the application when the quit button is pressed
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}