using UnityEngine;

/// <summary>
/// control the Tub Monster anomaly
/// Adds a tubmonster behind the showcurtain in the bathroom when activated
/// </summary>

public class TubMonster : Anomaly
{
    public GameObject tubMonster;

    /// <summary>
    /// hide when the game starts
    /// </summary>
    void Awake()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(false);
        }
    }

    /// <summary>
    /// when selected, show Tub Monster
    /// </summary>
    public override void Activate()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(true);
        }
    }

    ///when not selected, hide Tub Monster
    public override void Deactivate()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(false);
        }
    }
}
