using UnityEngine;


/// <summary>
/// Interior doors: open/close animation upon interaction
/// uses interactableInterface
/// </summary>
public class InteriorDoor : MonoBehaviour, InteractableInterface
{
    /// <summary>
    /// correct prompt based on door state
    /// </summary>
    public string Prompt => isOpen ? "Close Door" : "Open Door";                

    public float openAngle = 90f;
    public float rotationSpeed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    /// <summary>
    /// store inital rotation of door
    /// then calculate the rotation based on open angle
    /// </summary>
    void Start()
    {       
        closedRotation = transform.localRotation;                              
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);    
    }

    /// <summary>
    /// door state toggle after interaction
    /// </summary>
    public void Interact()
    {
        isOpen = !isOpen;                                                       
    }

    /// <summary>
    /// find rotation required based on door state
    /// then rotate smoothly to required rotation
    /// </summary>
    void Update()
    {
        Quaternion requiredRotation = isOpen ? openRotation : closedRotation;     
        transform.localRotation = Quaternion.Lerp(transform.localRotation, requiredRotation, rotationSpeed * Time.deltaTime);     
    }
}
