using UnityEngine;

// The bed: go back to sleep and retry the day
public class Bed : MonoBehaviour, InteractableInterface
{
    public string Prompt => "Go back to bed";

    public void Interact()
    {
        DayManager.Instance.GoToBed();
    }
}