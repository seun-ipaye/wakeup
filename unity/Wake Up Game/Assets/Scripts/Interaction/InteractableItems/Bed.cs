using UnityEngine;

public class Bed : MonoBehaviour, InteractableInterface
{
    public string Prompt => "Go back to bed";

    public void Interact()
    {
        if (SleepFader.Busy) return;
        SleepFader.Sleep(() => DayManager.Instance.GoToBed());
    }
}