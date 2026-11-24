using UnityEngine;

/// <summary>
/// A light switch the player can toggle with E
/// </summary>

public class LightSwitch : MonoBehaviour, InteractableInterface
{
    [Header("Lights")]
    [Tooltip("The lights this switch controls")]

    ///An array of what lights are controlled by this light switch
    public Light[] lights;
    
    /// Gives option to start with light on or off
    public bool startOn = true;
    
    private bool isOn;

    public string Prompt => isOn ? "Turn off light" : "Turn on light";

    void Start()
    {
        SetLights(startOn);
    }

    public void Interact()
    {
        SetLights(!isOn);
    }

    void SetLights(bool on)
    {
        isOn = on;
        foreach (var light in lights)
        {
            if (light != null) light.enabled = on;
        }
    }
}

