using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tracks the work week and handles the go-to-work / go-to-bed decision
public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public const int MaxDays = 5;
    public const int MaxRepeats = 2;

    // static so these survive when the House scene reloads for a new morning
    public static int CurrentDay { get; private set; } = 1;
    public static int RepeatsUsed { get; private set; }

    [Tooltip("Set by the anomaly system. Tick it by hand to test.")]
    public bool hasAnomaly;

    // other scripts (UI, anomalies) can listen for these
    public static event Action<int> DayChanged;
    public static event Action Won;
    public static event Action Lost;

    void Awake()
    {
        Instance = this;
    }

    // called by the front door
    [ContextMenu("Go To Work")]
    public void GoToWork()
    {
        if (hasAnomaly)
        {
            Lose("Went to work but something was wrong");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}