using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles the end credits screen and navigation back to the main menu
/// </summary>

public class EndCreditManager: MonoBehaviour
{   
    ///loads the MainMenu scene when the player selects the main menu button
    public void MainMenuSelect()
    {

        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu Selected");
    }
}