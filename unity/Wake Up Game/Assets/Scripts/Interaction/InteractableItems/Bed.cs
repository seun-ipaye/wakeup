using UnityEngine;

/// <summary>
/// A script that determines this asset will handle the go back to bed logic for the DayManager script
/// </summary>
public class Bed : MonoBehaviour, InteractableInterface
{
    /// Will display "Go back to bed" to the player
    public string Prompt => "Go back to bed";

    
    /// This is interactable and the SleepFader script will play 
    public void Interact() 
    {
        if (SleepFader.Busy) return;
        SleepFader.Sleep(() => DayManager.Instance.GoToBed());
    }
}