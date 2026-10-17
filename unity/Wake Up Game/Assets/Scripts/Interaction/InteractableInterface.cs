// Anything the player can press E on implements this
public interface InteractableInterface
{
    // text shown on screen while looking at the object
    string Prompt { get; }
    // called once when the player presses E
    void Interact();
}
