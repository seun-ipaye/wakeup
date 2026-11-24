/// <summary>
/// Anything the player can press E on implements this
/// an interface other interactable objects can implement
/// </summary>
public interface InteractableInterface
{
    /// <summary>
    /// text shown on screen while looking at the object
    /// </summary>
    string Prompt { get; }
    /// <summary>
    /// called once when the player presses E
    /// </summary>
    void Interact();
    bool CanInteract => true;
}
