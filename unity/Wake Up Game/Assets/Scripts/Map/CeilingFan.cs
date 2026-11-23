using UnityEngine;

///Attach this script to a rotating object (ceiling fan currently)
public class CeilingFan : MonoBehaviour
{
    [Header("Fan Settings")]
    public float rotationspeed = 150f; //Rotation speed of the fan in degrees per second
    public bool startOn = true; //Determines whether the fan is spinnning when the scene starts

    private bool isOn;

    void Start()
    {
        isOn = startOn;
    }

    ///Rotates the fan around its local Y-axis while turned on
    void Update()
    {
        if(isOn)
        {
            transform.Rotate(
                Vector3.up * rotationspeed * Time.deltaTime,
                Space.Self
                );
        }
    }

///Toggles the fan between spinning and stopped
public void ToggleFan()
{
    isOn = !isOn;
}
}
