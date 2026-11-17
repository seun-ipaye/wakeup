using UnityEngine;
using UnityEngine.Events;

public class TaskInteractable : MonoBehaviour, InteractableInterface
{
    [Tooltip("The task this object completes, e.g. \"Brush teeth\". Shown on the HUD")]
    public string taskName;

    [Tooltip("Shown while looking at it. Leave blank to use the task name")]
    public string prompt;

    [Tooltip("Position on the HUD list. Lower numbers show first")]
    public int hudOrder;

    [Tooltip("Hide this object once used, e.g. picking up the keys")]
    public bool hideWhenUsed;

    [Tooltip("Extra things to happen when used: play a sound, start an animation, turn on a tap")]
    public UnityEvent onUsed;

    private bool used;

    public string Prompt => string.IsNullOrWhiteSpace(prompt) ? taskName : prompt;

    public bool CanInteract => !used && TaskManager.Instance != null && TaskManager.Instance.IsToday(taskName);

    public void Interact()
    {
        if (!CanInteract) return;
        used = true;

        TaskManager.Instance.CompleteStep(taskName);
        onUsed.Invoke();

        if (hideWhenUsed)
            gameObject.SetActive(false);
    }
}
