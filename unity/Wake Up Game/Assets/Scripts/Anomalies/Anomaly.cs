using UnityEngine;

/// <summary>
/// this is the abstract base class for all anomalies
/// </summary>
///each anomaly should inherit from this class and implement the activate/deactivate methods

public abstract class Anomaly : MonoBehaviour
{
    [Header("Anomaly Info")]
    public string anomalyName; ///Name used to identify the anomaly
    public int tier = 1; ///Difficulty tier of the anomaly, default is 1

    ///Activates the anomaly's effects when selected
    public abstract void Activate();

    /// <summary>
    /// Deactivates the anomaly's effects when not selected
    /// </summary>
    public virtual void Deactivate()
    {
    }
}