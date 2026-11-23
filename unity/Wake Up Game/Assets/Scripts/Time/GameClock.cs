using UnityEngine;
using System;

/// <summary>
/// game clock that runs for 4 real-life minutes, starting at 6am and ending at 7am
/// </summary>

public class GameClock : MonoBehaviour
{
    public static GameClock Instance { get; private set; }

    /// 240 real-life seconds (4 mins) into 60 in-game minutes (6am to 7am)
    public float realTimeLimit = 240f;

    /// Starting game time: 6:00 AM
    public int startHour = 6;

    /// 60 in-game minutes between 6:00 and 7:00
    public int gameMinutes = 60;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }

    ///Tracks if the player has exceed the morning time limit
    public bool IsLate { get; private set; }

    ///Event triggered when the time limit is reached
    public event Action OnTimeUp;

    private float debugTimer = 0f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetClock();
    }

    private void Update()
    {
        if (!isRunning || IsLate)
            return;

        elapsedTime += Time.deltaTime;

        ///converts real time into game time 
        float progress = Mathf.Clamp01(
            elapsedTime / realTimeLimit
        );

        float totalGameMinutes = progress * gameMinutes;

        CurrentHour = startHour +
            Mathf.FloorToInt(totalGameMinutes / 60f);

        CurrentMinute = Mathf.FloorToInt(
            totalGameMinutes % 60f
        );


        /// Log the current game time every 30 real-life seconds
        debugTimer += Time.deltaTime;

        if (debugTimer >= 30f)
        {
            Debug.Log("Current Game Time: " + GetFormattedTime());
            debugTimer = 0f;
        }


        ///marks the player as late, stops the clock and trigger OnTimeUp event
        if (progress >= 1f)
        {
            IsLate = true;
            isRunning = false;

            Debug.Log("Deadline reached, player is late for work");

            OnTimeUp?.Invoke();
        }
    }

    ///resets and starts the morning game clock
    public void StartMorning()
    {
        ResetClock();
        isRunning = true;

        Debug.Log("Morning clock started: " + GetFormattedTime());
    }

    ///stops the morning clock without resetting the current time 
    public void EndMorning()
    {
        isRunning = false;
        Debug.Log("Morning clock ended: " + GetFormattedTime());
    }

    ///reset the clock to its starting time and clears the late status
    public void ResetClock()
    {
        elapsedTime = 0f;
        IsLate = false;

        CurrentHour = startHour;
        CurrentMinute = 0;

        isRunning = false;

        debugTimer = 0f;
        Debug.Log("clock reset: " + GetFormattedTime());

    }

    ///returns the current game time as a formatted string ("6:05 AM")
    public string GetFormattedTime()
    {
        return $"{CurrentHour}:{CurrentMinute:00} AM";
    }
}
