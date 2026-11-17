using UnityEngine;
using System;

//game clock that runs for 4 real-life minutes, starting at 6am and ending at 7am

public class GameClock : MonoBehaviour
{
    public static GameClock Instance { get; private set; }

    // 4 real-life minutes
    public float realTimeLimit = 240f;

    // Starting game time: 6:00 AM
    public int startHour = 6;

    // 60 in-game minutes between 6:00 and 7:00
    public int gameMinutes = 60;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }

    public bool IsLate { get; private set; }

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

        float progress = Mathf.Clamp01(
            elapsedTime / realTimeLimit
        );

        float totalGameMinutes = progress * gameMinutes;

        CurrentHour = startHour +
            Mathf.FloorToInt(totalGameMinutes / 60f);

        CurrentMinute = Mathf.FloorToInt(
            totalGameMinutes % 60f
        );


        // Log the current game time every 30 real-life seconds
        debugTimer += Time.deltaTime;

        if (debugTimer >= 30f)
        {
            Debug.Log("Current Game Time: " + GetFormattedTime());
            debugTimer = 0f;
        }



        if (progress >= 1f)
        {
            IsLate = true;
            isRunning = false;

            Debug.Log("Deadline reached, player is late for work");

            OnTimeUp?.Invoke();
        }
    }

    public void StartMorning()
    {
        ResetClock();
        isRunning = true;

        Debug.Log("Morning clock started: " + GetFormattedTime());
    }

    public void EndMorning()
    {
        isRunning = false;
        Debug.Log("Morning clock ended: " + GetFormattedTime());
    }

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

    public string GetFormattedTime()
    {
        return $"{CurrentHour}:{CurrentMinute:00} AM";
    }
}
