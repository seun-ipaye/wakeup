using UnityEngine;

//move the hands of an anolog clock object based on the current time of the GameClock

public class AnalogClock : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;

    // Set these depending on how your model is oriented.
    public Vector3 rotationAxis = Vector3.forward;


    private void Update()
    {
        if (GameClock.Instance == null)
            return;

        int hour = GameClock.Instance.CurrentHour;
        int minute = GameClock.Instance.CurrentMinute;


        // Calculate hand rotation
        float minuteAngle = minute * 6f;

        float hourAngle = (hour % 12) * 30f + minute * 0.5f;

        // Rotate the hands
        minuteHand.localRotation = Quaternion.AngleAxis(minuteAngle, rotationAxis);

        hourHand.localRotation = Quaternion.AngleAxis(hourAngle, rotationAxis);
    }
}
