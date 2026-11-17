using UnityEngine;
using EZPZGames.FunctionalBathroom;

public class ToiletInteract : MonoBehaviour, InteractableInterface
{
    [Header("Toilet")]
    [SerializeField] private ToiletController toilet;

    public string Prompt => "Flush toilet";

    void Awake()
    {
        if (toilet == null)
        {
            toilet = GetComponentInParent<ToiletController>();
        }
    }

    public void Interact()
    {
        if (toilet != null)
        {
            toilet.Flush();
        }
    }
    
}
