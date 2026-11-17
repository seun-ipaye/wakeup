using UnityEngine;

//this is the base class for all anomalies
//each anomaly should inherit from this class and implement the activate/deactivate methods

public class VoidFloor : Anomaly {
    [Header("Anomaly Info")]
    public GameObject voidFloor;

    public void Awake() {
        voidFloor.SetActive(false);
    }

    //when anomaly is selected
    public override void Activate() { 
        voidFloor.SetActive(true);
    }

    //when anomaly is not selected
    public override void Deactivate() {
        voidFloor.SetActive(false);
    }
}