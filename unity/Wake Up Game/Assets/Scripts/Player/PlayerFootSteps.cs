using UnityEngine;

/// <summary>
/// plays footstep sounds when the player is moving on the ground
/// requires CharacterController and AudioSource components on the player
/// </summary>

public class PlayerFootSteps : MonoBehaviour

{   
    /// <summary>
    /// can select an array of audio clips used for footstep sounds
    /// </summary>
    [Header("Footstep Sounds")]
    public AudioClip[] footstepSounds;

    /// <summary>
    /// the control to adjust the speed and volume of the footsteps
    /// </summary>
    [Header("Footstep Settings")]
    public float stepInterval = 0.5f;
    public float volume = 0.2f;

    /// <summary>
    /// references to the player 
    /// </summary>
    private AudioSource audioSource; //plays the footstep sounds
    private CharacterController controller; //CharacterController checks if the player is grounded and moving

    /// <summary>
    /// this is a timer to know when the next footstep will play
    /// </summary>
    private float stepTimer;

    /// <summary>
    /// tracks which sound should play next
    /// </summary>
    private int currentStep = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {   
        ///this is a check if the player is on the ground and moving, it also will ignore very small movements to prevent unwanted sounds
        if (controller.isGrounded && new Vector2(controller.velocity.x, controller.velocity.z).magnitude > 0.1f)
        {   
            ///a time to count down until the next footstep
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {   
                ///plays a footstepsound and then resets the timer
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {  ///when the player stops reset the timer
            stepTimer = 0f;
        }

    }
        void PlayFootstep()
        {   
            ///if no sounds are added to array do nothing
            if (footstepSounds.Length == 0)
                return;
            
            ///select the next footstep from the array
            AudioClip clip = footstepSounds[currentStep];
            
        ///randomizes the pitch in order to make the footsteps more natural sounding
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            
            ///play the selected footstep sound at the set volume
            audioSource.PlayOneShot(clip, volume);
            
            ///goes through the footstep array and then return to the beginning after the last one
            currentStep = (currentStep + 1) % footstepSounds.Length;
        }

    }