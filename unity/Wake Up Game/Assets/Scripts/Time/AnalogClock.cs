using UnityEngine;
/// <summary>
/// Move the hands of an anolog clock object based on the current time of the GameClock
/// </summary>


public class AnalogClock : MonoBehaviour
{   
    ///references to the clock's hour and minute hand objects
    public Transform hourHand;
    public Transform minuteHand;

    ///a vector that determines how the hands rotate around, depending on model orientation
    public Vector3 rotationAxis = Vector3.forward;

    ///When true the clock hands should move backwards (anomaly)
    public bool reverseHands = false;

    private void Update()
    {   
        ///use the gameclock singleton to get the current gametime
        if (GameClock.Instance == null)
            return;

        int hour = GameClock.Instance.CurrentHour;
        int minute = GameClock.Instance.CurrentMinute;


        ///calculate hand rotation based on hours and minutes
        ///minute hand moves 6 degress per minute
        float minuteAngle = minute * 6f;

        ///hour hand moves 30 degrees per hour plus 0.5 degrees for each minute
        float hourAngle = (hour % 12) * 30f + minute * 0.5f;


        ///reverse the rotation of hands when anomaly is active
        if (reverseHands)
        {
            minuteAngle = -minuteAngle;
            hourAngle = -hourAngle;
        }


        ///rotate the hands
        minuteHand.localRotation = Quaternion.AngleAxis(minuteAngle, rotationAxis);

        hourHand.localRotation = Quaternion.AngleAxis(hourAngle, rotationAxis);
    }
}
