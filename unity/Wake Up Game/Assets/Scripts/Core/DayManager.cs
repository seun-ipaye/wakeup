using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tracks the work week and handles the go-to-work / go-to-bed decision
public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public const int MaxDays = 5;
    public const int MaxRepeats = 5;

    // static so these survive when the House scene reloads for a new morning
    public static int CurrentDay { get; private set; } = 1;
    public static int RepeatsUsed { get; private set; }

    // blocks repeat calls once a new morning is loading
    static bool changing;

    // set each morning by AnomalyManager
    [HideInInspector] public bool hasAnomaly;

    // other scripts (UI, anomalies) can listen for these
    public static event Action<int> DayChanged;
    public static event Action Won;
    public static event Action Lost;

    void Awake()
    {
        Instance = this;
        changing = false;
    }

    void Start()
    {
        //start clock when the game starts on the first day
        if (GameClock.Instance != null) {

            //listen for clock reaching time limit
            GameClock.Instance.OnTimeUp += HandleTimeUp;

            GameClock.Instance.StartMorning();
            Debug.Log("DayManager started the morning clock");
        } else {
            Debug.LogError("GameClock instance not found");
        }
    }

    //called when the clock reaches time limit in morning
    void HandleTimeUp()
    {
        Debug.Log("Time limit reached! The player is late for work.");

        Lose("Took too much time getting ready for work");
    }

    //destroy event listener when the scene is reloaded
    void OnDestroy()
    {
        //restart the clock when the scene is reloaded for a new morning
        if (GameClock.Instance != null){
            GameClock.Instance.OnTimeUp -= HandleTimeUp;
        }
    }

    // called by the front door
    [ContextMenu("Go To Work")]
    public void GoToWork()
    {
        if (changing) return;

        if (hasAnomaly)
        {
            Lose("Went to work but something was wrong");
            return;
        }

        if (TaskManager.Instance != null && !TaskManager.Instance.RequiredTasksDone)
        {
            Lose("Went to work without finishing the morning routine");
            return;
        }

        if (CurrentDay >= MaxDays)
        {
            Win();
            return;
        }

        NextDay();
        StartMorning();
    }

    // called by the bed
    [ContextMenu("Go To Bed")]
    public void GoToBed()
    {
        if (changing) return;

        if (!hasAnomaly)
        {
            Lose("Went back to bed on a normal morning");
            return;
        }

        if (RepeatsUsed >= MaxRepeats)
        {
            Lose("Out of repeats");
            return;
        }

        RepeatsUsed++;
        Debug.Log($"Repeating Day {CurrentDay} ({RepeatsUsed}/{MaxRepeats})");
        StartMorning();
    }

    public void NextDay()
    {
        CurrentDay++;
        RepeatsUsed = 0;
        Debug.Log($"Day {CurrentDay}");
        DayChanged?.Invoke(CurrentDay);
    }

    public void ResetWeek()
    {
        CurrentDay = 1;
        RepeatsUsed = 0;

        AnomalyManager.Instance.ResetUsedAnomalies();

        Debug.Log("Week reset to Day 1");
        DayChanged?.Invoke(CurrentDay);
    }

    void Win()
    {
        Debug.Log("You made it through the week!");
        Won?.Invoke();
    }

    void Lose(string reason)
    {
        Debug.Log($"Lost: {reason}");
        Lost?.Invoke();
        ResetWeek();
        StartMorning();
    }

    // reload the house so every morning starts fresh
    void StartMorning()
    {
        changing = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}