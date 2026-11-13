using UnityEngine;

//this is the base class for all anomalies
//each anomaly should inherit from this class and implement the activate/deactivate methods

public abstract class Anomaly : MonoBehaviour
{
    [Header("Anomaly Info")]
    public string anomalyName;
    public int tier = 1;

    //when anomaly is selected
    public abstract void Activate();

    //when anomaly is not selected
    public virtual void Deactivate()
    {
    }
}