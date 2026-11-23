using UnityEngine;

//VoidFloor anomaly, replaces living room and kitchen floor with a black floor

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