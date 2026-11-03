using UnityEngine;


// Interior doors: open/close animation upon interaction
public class InteriorDoor : MonoBehaviour, InteractableInterface
{
    public string Prompt => isOpen ? "Close Door" : "Open Door";                //correct prompt based on door state

    public float openAngle = 90f;
    public float rotationSpeed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {       
        closedRotation = transform.localRotation;                               //store inital rotation of door
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);    //calculate the rotation based on open angle
    }

    public void Interact()
    {
        isOpen = !isOpen;                                                       //door state toggle after interaction
    }

    void Update()
    {
        Quaternion requiredRotation = isOpen ? openRotation : closedRotation;     //find rotation required based on door state
        transform.localRotation = Quaternion.Lerp(transform.localRotation, requiredRotation, rotationSpeed * Time.deltaTime);     //rotate smoothly to required rotation
    }
}
