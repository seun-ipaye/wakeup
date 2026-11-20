using UnityEngine;
using UnityEngine.SceneManagement;

//handles buttons on main menu

public class EndCreditManager: MonoBehaviour
{
    public void MainMenuSelect()
    {

        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu Selected");
    }
}