using UnityEngine;
using EZPZGames.FunctionalBathroom;


/// <summary>
/// links the Toiletcontroller to the InteractableInterface
/// this is so the player can flush the toilet when selecting E
/// </summary>

public class ToiletInteract : MonoBehaviour, InteractableInterface
{   
    ///reference to the assets toilet controller
    [Header("Toilet")]
    [SerializeField] private ToiletController toilet;

    ///says what the prompt does when player is looking at it
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
