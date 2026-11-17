using UnityEngine;

//the Backwards Clock anomaly, makes hands move in reverse

public class BackwardsClock : Anomaly
{
    public AnalogClock[] clocks;

    //regular clock when the game starts
    void Awake()
    {
        foreach (AnalogClock clock in clocks)
        {
            if (clock != null)
                clock.reverseHands = false;
        }
    }

    //if selected, reverse the clock hands
    public override void Activate()
    {
        foreach (AnalogClock clock in clocks)
        {
            if (clock != null)
                clock.reverseHands = true;
        }
    }

    //if not selected, return clock to normal
    public override void Deactivate()
    {
        foreach (AnalogClock clock in clocks)
        {
            if (clock != null)
                clock.reverseHands = false;
        }
    }
}