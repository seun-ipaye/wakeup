using UnityEngine;

// A light switch the player can toggle with E
//Jessica: add emissive logic
public class LightSwitch : MonoBehaviour, InteractableInterface
{
    [Header("Lights")]
    [Tooltip("The lights this switch controls")]
    public Light[] lights;
    public bool startOn = true;

    [Header("Emissive")]
    public Renderer[] glowingLamp;

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


        //toggle emission
        foreach (var lamp in glowingLamp)
        {
            if (lamp == null) continue;
                if (on)
            {
                lamp.material.EnableKeyword("_EMISSION");
            }
            else
            {
                lamp.material.DisableKeyword("_EMISSION");
            }
        }

    }
}