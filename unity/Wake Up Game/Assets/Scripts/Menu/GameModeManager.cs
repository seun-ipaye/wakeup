using UnityEngine;

//2 game modes (story and hardcore)
//Defines the two available game modes: Story and Hardcore

public enum GameMode {
    Story,
    Hardcore
}

//Manages the selected game mode across the scenes
//MainMenuManager sets the mode when the player starts a game
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; } //singleton allows other scripts to access the GameModeManager.Instance

    //Stores the selected game mode, defaulting to Story
    //Other gameplay scripts can check this value to apply mode-specific behaviour
    public GameMode currentMode = GameMode.Story;

    private void Awake()
    {
        //handle duplicate GameModeManager instances
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //dont destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}