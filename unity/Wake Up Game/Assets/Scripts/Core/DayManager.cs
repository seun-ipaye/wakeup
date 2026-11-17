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

    [Header("Front door messages")]
    [TextArea(3, 6)] public string wentToWorkMessage =
        "You went to work and had a long, stressful day.\n\nThat night you fell asleep the moment your head hit the pillow...\n\n...and woke up to your alarm again.";
    [TextArea(3, 6)] public string missedAnomalyMessage =
        "You slept through your alarm and missed work.\n\nWhat a dream...\n\nBack to Monday.";
    [TextArea(3, 6)] public string unfinishedTasksMessage =
        "You rushed out the door without finishing your morning routine.\n\nThe whole day fell apart from there.\n\nBack to Monday.";
    [TextArea(3, 6)] public string wonMessage =
        "You made it through the whole week.\n\nFinally, the weekend.";

    [Header("Bed messages")]
    [TextArea(3, 6)] public string escapedDreamMessage =
        "It was only a dream.\n\nYou drift off again...";
    [TextArea(3, 6)] public string sleptOnNormalMorningMessage =
        "You went back to sleep... but you weren't dreaming.\n\nYou overslept and missed work.\n\nBack to Monday.";
    [TextArea(3, 6)] public string outOfRepeatsMessage =
        "No matter how many times you close your eyes, you can't seem to wake up.\n\nBack to Monday.";

    [Header("Clock messages")]
    [TextArea(3, 6)] public string lateMessage =
        "You lost track of time and were late for work.\n\nBack to Monday.";

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

        if (SleepFader.Busy) return;
        SleepFader.Sleep(() =>
        {
            SleepFader.ShowMessage(lateMessage);
            Lose("Took too much time getting ready for work");
        });
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
            SleepFader.ShowMessage(missedAnomalyMessage);
            Lose("Went to work but something was wrong");
            return;
        }

        if (TaskManager.Instance != null && !TaskManager.Instance.RequiredTasksDone)
        {
            SleepFader.ShowMessage(unfinishedTasksMessage);
            Lose("Went to work without finishing the morning routine");
            return;
        }

        if (CurrentDay >= MaxDays)
        {
            SleepFader.ShowMessage(wonMessage, stayBlack: true);
            Win();
            return;
        }

        SleepFader.ShowMessage(wentToWorkMessage);
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
            SleepFader.ShowMessage(sleptOnNormalMorningMessage);
            Lose("Went back to bed on a normal morning");
            return;
        }

        if (RepeatsUsed >= MaxRepeats)
        {
            SleepFader.ShowMessage(outOfRepeatsMessage);
            Lose("Out of repeats");
            return;
        }

        SleepFader.ShowMessage(escapedDreamMessage);
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