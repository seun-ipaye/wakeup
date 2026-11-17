using UnityEngine;

public class PlayerFootSteps : MonoBehaviour


{
    [Header("Footstep Sounds")]
    public AudioClip[] footstepSounds;

    [Header("Footstep Settings")]
    public float stepInterval = 0.5f;
    public float volume = 0.2f;

    private AudioSource audioSource;
    private CharacterController controller;
    private float stepTimer;
    private int currentStep = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && new Vector2(controller.velocity.x, controller.velocity.z).magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

    }
        void PlayFootstep()
        {
            if (footstepSounds.Length == 0)
                return;

            AudioClip clip = footstepSounds[currentStep];

            audioSource.pitch = Random.Range(0.95f, 1.05f);

            audioSource.PlayOneShot(clip, volume);

            currentStep = (currentStep + 1) % footstepSounds.Length;
        }

    }