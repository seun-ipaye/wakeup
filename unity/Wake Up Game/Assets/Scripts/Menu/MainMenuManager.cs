using UnityEngine;
using UnityEngine.SceneManagement;

//handles buttons on main menu

public class MainMenuManager : MonoBehaviour
{
    //story mode
    public void StartStoryMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Story;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Story mode started");
    }

    //hardcore mode
    public void StartHardcoreMode()
    {
        GameModeManager.Instance.currentMode = GameMode.Hardcore;

        SceneManager.LoadScene("HouseScene");
        Debug.Log("Hardcore mode started");
    }

    //close the game with quit button
    public void QuitGame()
    {
        Application.Quit();
    }
}