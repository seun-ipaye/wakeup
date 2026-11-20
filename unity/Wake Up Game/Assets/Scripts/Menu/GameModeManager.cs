using UnityEngine;

//2 game modes (story and hardcore)

public enum GameMode {
    Story,
    Hardcore
}

//track the current game mode
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

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