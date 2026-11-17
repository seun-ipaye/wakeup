using UnityEngine;

// The front door: leave for work
public class FrontDoor : MonoBehaviour, InteractableInterface
{
    public string Prompt => "Go to work";

    public void Interact()
    {
        if (SleepFader.Busy) return;
        SleepFader.Sleep(() => DayManager.Instance.GoToWork());
    }
}