using UnityEngine;

// A light switch the player can toggle with E
public class LightSwitch : MonoBehaviour, InteractableInterface
{
    [Tooltip("The lights this switch controls")]
    public Light[] lights;
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
            if (light != null) light.enabled = on;
    }
}
