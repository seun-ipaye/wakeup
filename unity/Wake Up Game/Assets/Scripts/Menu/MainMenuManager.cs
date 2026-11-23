using UnityEngine;
using UnityEngine.SceneManagement;

//handles buttons on main menu

public class MainMenuManager : MonoBehaviour
{
    //Sets GameModeManager to Story mode and loads HouseScene
    public void StartStoryMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Story;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Story mode started");
    }

    //Sets GameModeManager to Hardcore mode and loads HouseScene
    public void StartHardcoreMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Hardcore;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Hardcore mode started");
    }

    //Quits the application when the quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}