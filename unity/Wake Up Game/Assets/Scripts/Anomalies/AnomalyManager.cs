using UnityEngine;

// Decides each morning whether there is an anomaly and how subtle it is
public class AnomalyManager : MonoBehaviour
{
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

    void Start()
    {
        hasAnomaly = RollAnomaly();
        tier = hasAnomaly ? PickTier() : 0;

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
}