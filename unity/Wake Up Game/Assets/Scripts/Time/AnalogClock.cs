using UnityEngine;

//move the hands of an anolog clock object based on the current time of the GameClock

public class AnalogClock : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;

    //set these depending on how your model is oriented
    public Vector3 rotationAxis = Vector3.forward;

    //if the clock hands should move backwards (anomaly)
    public bool reverseHands = false;

    private void Update()
    {
        if (GameClock.Instance == null)
            return;

        int hour = GameClock.Instance.CurrentHour;
        int minute = GameClock.Instance.CurrentMinute;


        //calculate hand rotation
        float minuteAngle = minute * 6f;

        float hourAngle = (hour % 12) * 30f + minute * 0.5f;


        //reverse the rotation of hands when anomaly is active
        if (reverseHands)
        {
            minuteAngle = -minuteAngle;
            hourAngle = -hourAngle;
        }


        //rotate the hands
        minuteHand.localRotation = Quaternion.AngleAxis(minuteAngle, rotationAxis);

        hourHand.localRotation = Quaternion.AngleAxis(hourAngle, rotationAxis);
    }
}
