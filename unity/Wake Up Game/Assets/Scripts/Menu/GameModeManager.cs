using UnityEngine;


/// <summary>
///2 game modes (story and hardcore)
///Defines the two available game modes: Story and Hardcore
/// </summary>


public enum GameMode {
    Story,
    Hardcore
}
/// <summary>
/// Manages the selected game mode across the scenes
/// MainMenuManager sets the mode when the player starts a game
/// </summary>

public class GameModeManager : MonoBehaviour
{
    /// <summary>
    /// singleton allows other scripts to access the GameModeManager.Instance
    /// </summary>
    public static GameModeManager Instance { get; private set; } 
    
    
    /// <summary>
    /// Stores the selected game mode, defaulting to Story
    ///Other gameplay scripts can check this value to apply mode-specific behaviour
    /// </summary>
    public GameMode currentMode = GameMode.Story;

    /// <summary>
    /// handle duplicate GameModeManager instances
    /// </summary>
    private void Awake()
    {
        
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ///dont destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}