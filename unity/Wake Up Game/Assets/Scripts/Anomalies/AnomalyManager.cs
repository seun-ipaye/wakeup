using UnityEngine;

///Configures Story Mode anomalies for each day
[System.Serializable]
public class StoryDayAnomalies {
    public string dayName; //Name of the day

    ///range of anomalies assigned that day
    public int minAnomalies; 
    public int maxAnomalies;
    public Anomaly[] anomalies; //Pool of anomalies available for selection
}

/// Decides each morning whether there is an anomaly and how subtle it is
public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; } //Singleton allows other scripts to access AnomalyManager.Instance

    ///Controls Anomaly probability in HardcoreMode
    public enum Mode { Random, ForceNormal, ForceAnomaly }

    [Header("Testing")]
    [Tooltip("Set before pressing Play. Random is the real game.")]
    public Mode mode = Mode.Random;

    [Header("Chance")]
    [Range(0f, 1f)] public float firstDayChance = 0.5f;
    [Range(0f, 1f)] public float lastDayChance = 0.8f;
    [Range(0f, 1f)] public float lowestChance = 0.2f;

    ///Maximum anomaly difficulty tier (1 = obvious, 3 = subtle)
    public const int MaxTier = 3;


    [Header("This morning (read only)")]
    public bool hasAnomaly; ///Tracks whether an anomaly exists this morning and its difficulty tier
    public int tier;

    [Header("Hardcore Anomalies")]
    public Anomaly[] anomalies; ///Pool of available Hardcore Mode anomalies
    public Anomaly selectedAnomaly; ///Reference to the currently selected anomaly

    ///keeps selected story mode anomaly across scene reloads
    private static int storySelectedAnomalyIndex = -1;
    private static int storySelectedDay = 0;
    private static int storyAnomaliesRemaining;
    private static int storyCountSelectedDay = 0;

    ///Configures each day's available anomalies and min/max count
    [Header("Story Mode Anomalies")]
    public StoryDayAnomalies[] storyDayAnomalies;

    ///anomalies already used this week for hardcore mode
    private static System.Collections.Generic.List<Anomaly> usedAnomalies = new System.Collections.Generic.List<Anomaly>();

    void Awake()
    {
        Instance = this;
    }

    ///Selects anomalies based on Story or Hardcore Mode
    ///Updates Daymanager with the morning's anomaly status
    void Start()
    {
        if (GameModeManager.Instance.currentMode == GameMode.Story) {
            SelectStoryAnomaly();
        } else {
            hasAnomaly = RollAnomaly();
            tier = hasAnomaly ? PickTier() : 0;

            if (hasAnomaly)
            {
                SelectAnomaly();
            }
        }

        DayManager.Instance.hasAnomaly = hasAnomaly;

        Debug.Log(hasAnomaly
            ? $"Day {DayManager.CurrentDay}: anomaly (tier {tier})"
            : $"Day {DayManager.CurrentDay}: normal morning");
    }

    ///Determins whether an anomaly occurs using the selected testing mode
    bool RollAnomaly()
    {
        if (mode == Mode.ForceNormal) return false;
        if (mode == Mode.ForceAnomaly) return true;
        return Random.value < ChanceForToday();
    }

    /// 50% on day 1 rising to 80% on day 5, falling evenly to lowestChance
    /// by the last repeat roll, then guaranteed normal once repeats are used up
    float ChanceForToday()
    {
        if (DayManager.RepeatsUsed >= DayManager.MaxRepeats) return 0f;

        float progress = (DayManager.CurrentDay - 1) / (float)(DayManager.MaxDays - 1);
        float chance = Mathf.Lerp(firstDayChance, lastDayChance, progress);
        float repeats = DayManager.RepeatsUsed / (float)(DayManager.MaxRepeats - 1);
        return Mathf.Lerp(chance, lowestChance, repeats);
    }

    /// tier 1 = obvious, 3 = subtle; later days unlock subtler tiers
    int PickTier()
    {
        int highest = Mathf.Clamp(Mathf.CeilToInt(DayManager.CurrentDay / 2f), 1, MaxTier);
        return Random.Range(1, highest + 1);
    }

    ///select anomaly from the chosen tier
    void SelectAnomaly()
    {
        ///store anomalies that match day's tier
        System.Collections.Generic.List<Anomaly> available = new System.Collections.Generic.List<Anomaly>();

        foreach (Anomaly anomaly in anomalies){
            if (anomaly != null && anomaly.tier == tier && !usedAnomalies.Contains(anomaly)){
                available.Add(anomaly);
            }
        }

        ///make sure there at least 1 available anomaly
        if (available.Count == 0){
            Debug.LogWarning("No anomalies for tier " + tier);
            return;
        }

        ///randomly pick anomaly
        selectedAnomaly = available[Random.Range(0, available.Count)];

        ///mark anomaly as used this week
        usedAnomalies.Add(selectedAnomaly);

        ///activate selected anomaly
        selectedAnomaly.Activate();

        Debug.Log("Selected anomaly: " + selectedAnomaly.anomalyName);
    }

    ///select anomaly from the story mode pool for current day
    void SelectStoryAnomaly()
    {

        int dayIndex = DayManager.CurrentDay - 1;

        ///check that current day has configured anomaly pool listed
        if (dayIndex < 0 || dayIndex >= storyDayAnomalies.Length)
        {
            Debug.LogWarning("No story mode anomaly pool for this day");
            return;
        }

        StoryDayAnomalies day = storyDayAnomalies[dayIndex];

        ///only choose the number of anomalies once per day
        if (storyCountSelectedDay != DayManager.CurrentDay)
        {
            storyAnomaliesRemaining = Random.Range(day.minAnomalies, day.maxAnomalies + 1);
            storyCountSelectedDay = DayManager.CurrentDay;

            Debug.Log($"(Story Mode) {day.dayName}: {storyAnomaliesRemaining} anomalies today");
        }

        ///no anomalies remaining == normal morning
        if (storyAnomaliesRemaining <= 0)
        {
            hasAnomaly = false;
            tier = 0;
            selectedAnomaly = null;

            Debug.Log("(Story Mode) Normal morning");
            return;
        }

        ///reuse selected anomaly if player is retrying
        if (day.anomalies == null || day.anomalies.Length == 0) {
            Debug.LogWarning("No anomalies assigned to " + day.dayName);
            return;
        }

        ///reuse same anomaly if player is retrying
        if (storySelectedDay == DayManager.CurrentDay &&
            storySelectedAnomalyIndex >= 0 &&
            storySelectedAnomalyIndex < day.anomalies.Length &&
            day.anomalies[storySelectedAnomalyIndex] != null)
        {
            selectedAnomaly = day.anomalies[storySelectedAnomalyIndex];
        } else {
            ///choose a new anomaly and save position in array
            storySelectedAnomalyIndex = Random.Range(0, day.anomalies.Length);
            selectedAnomaly = day.anomalies[storySelectedAnomalyIndex];
            storySelectedDay = DayManager.CurrentDay;
        }

        if (selectedAnomaly == null)
        {
            Debug.LogWarning("Selected anomaly is missing");
            return;
        }

        hasAnomaly = true;
        tier = selectedAnomaly.tier;

        selectedAnomaly.Activate();

        Debug.Log("(Story Mode) Selected anomaly: " + selectedAnomaly.anomalyName);

    }


    ///when player successfully finds anomaly
    public void FoundStoryAnomaly()
    {
        if (GameModeManager.Instance.currentMode != GameMode.Story)
            return;

        storyAnomaliesRemaining--;

        ///clear selected anomaly so another one can be selected
        storySelectedAnomalyIndex = -1;
        storySelectedDay = 0;

        Debug.Log($"(Story Mode) Anomalies remaining: {storyAnomaliesRemaining}");
    }


    ///reset anomaly history when new week starts
    public void ResetUsedAnomalies()
    {
        usedAnomalies.Clear();

        Debug.Log("Anomaly history reset for the new week");
    }

    ///reset saved story mode anomaly history
    public void ResetStorySelection()
    {
        storySelectedAnomalyIndex = -1;
        storySelectedDay = 0;

        storyAnomaliesRemaining = 0;
        storyCountSelectedDay = 0;
    }
}