using UnityEngine;

// Decides each morning whether there is an anomaly and how subtle it is
public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; }

    public enum Mode { Random, ForceNormal, ForceAnomaly }

    [Header("Testing")]
    [Tooltip("Set before pressing Play. Random is the real game.")]
    public Mode mode = Mode.Random;

    [Header("Chance")]
    [Range(0f, 1f)] public float firstDayChance = 0.5f;
    [Range(0f, 1f)] public float lastDayChance = 0.8f;
    [Range(0f, 1f)] public float lowestChance = 0.2f;

    public const int MaxTier = 3;

    [Header("This morning (read only)")]
    public bool hasAnomaly;
    public int tier;

    [Header("Available Anomalies")]
    public Anomaly[] anomalies;
    public Anomaly selectedAnomaly;

    //anomalies already used this week
    private static System.Collections.Generic.List<Anomaly> usedAnomalies = new System.Collections.Generic.List<Anomaly>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hasAnomaly = RollAnomaly();
        tier = hasAnomaly ? PickTier() : 0;

        if (hasAnomaly){
            SelectAnomaly();
        }

        DayManager.Instance.hasAnomaly = hasAnomaly;

        Debug.Log(hasAnomaly
            ? $"Day {DayManager.CurrentDay}: anomaly (tier {tier})"
            : $"Day {DayManager.CurrentDay}: normal morning");
    }

    bool RollAnomaly()
    {
        if (mode == Mode.ForceNormal) return false;
        if (mode == Mode.ForceAnomaly) return true;
        return Random.value < ChanceForToday();
    }

    // 50% on day 1 rising to 80% on day 5, falling evenly to lowestChance
    // by the last repeat roll, then guaranteed normal once repeats are used up
    float ChanceForToday()
    {
        if (DayManager.RepeatsUsed >= DayManager.MaxRepeats) return 0f;

        float progress = (DayManager.CurrentDay - 1) / (float)(DayManager.MaxDays - 1);
        float chance = Mathf.Lerp(firstDayChance, lastDayChance, progress);
        float repeats = DayManager.RepeatsUsed / (float)(DayManager.MaxRepeats - 1);
        return Mathf.Lerp(chance, lowestChance, repeats);
    }

    // tier 1 = obvious, 3 = subtle; later days unlock subtler tiers
    int PickTier()
    {
        int highest = Mathf.Clamp(Mathf.CeilToInt(DayManager.CurrentDay / 2f), 1, MaxTier);
        return Random.Range(1, highest + 1);
    }

    //select anomaly from the chosen tier
    void SelectAnomaly()
    {
        //store anomalies that match day's tier
        System.Collections.Generic.List<Anomaly> available = new System.Collections.Generic.List<Anomaly>();

        foreach (Anomaly anomaly in anomalies){
            if (anomaly != null && anomaly.tier == tier && !usedAnomalies.Contains(anomaly)){
                available.Add(anomaly);
            }
        }

        //make sure there at least 1 available anomaly
        if (available.Count == 0){
            Debug.LogWarning("No anomalies for tier " + tier);
            return;
        }

        //randomly pick anomaly
        selectedAnomaly = available[Random.Range(0, available.Count)];

        //mark anomaly as used this week
        usedAnomalies.Add(selectedAnomaly);

        //activate selected anomaly
        selectedAnomaly.Activate();

        Debug.Log("Selected anomaly: " + selectedAnomaly.anomalyName);
    }

    //resetanomaly history when new week starts
    public void ResetUsedAnomalies()
    {
        usedAnomalies.Clear();

        Debug.Log("Anomaly history reset for the new week");
    }

}