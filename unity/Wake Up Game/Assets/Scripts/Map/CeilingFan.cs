using UnityEngine;

public class CeilingFan : MonoBehaviour
{
    [Header("Fan Settings")]
    public float rotationspeed = 150f;
    public bool startOn = true;

    private bool isOn;

    void Start()
    {
        isOn = startOn;
    }

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
    
public void ToggleFan()
{
    isOn = !isOn;
}
}
