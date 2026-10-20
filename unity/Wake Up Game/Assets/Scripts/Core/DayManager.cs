using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public const int MaxDays = 5;

    // static so the day survives when the House scene reloads for a new morning
    public static int CurrentDay { get; private set; } = 1;

    // other scripts (UI, anomalies) can listen for this
    public static event Action<int> DayChanged;

    void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Next Day")]
    public void NextDay()
    {
        CurrentDay++;
        Debug.Log($"Day {CurrentDay}");
        DayChanged?.Invoke(CurrentDay);
    }

    [ContextMenu("Reset Week")]
    public void ResetWeek()
    {
        CurrentDay = 1;
        Debug.Log("Week reset to Day 1");
        DayChanged?.Invoke(CurrentDay);
    }
}
