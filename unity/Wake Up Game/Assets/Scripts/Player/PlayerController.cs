using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// First person player controller. (W/A/S/D walk, no sprint or jump)
/// Requires a CharacterController for player movement and collision
///Uses Unity's Input System for keyboard and mouse controls
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Walking speed in mps (target: 2.5 m/s)")]
    ///walking speed in meters per second
    public float walkSpeed = 2.5f;
    /// <summary>
    /// downward acceleration applied to the player
    /// </summary>
    public float gravity = -9.81f; 
 
    [Header("Look")]
    [Tooltip("The first-person camera")]
    ///Reference to the first-person camera
    public Camera playerCamera;
    ///controls how quickly the camera rotates with mouse movement
    public float mouseSensitivity = 0.1f; 
    [Tooltip("How far up/down the player can look")]
    ///maximum vertical camera angle to prevent looking too far up or down
    public float maxPitch = 80f; 
    [Tooltip("Camera field of view, set when the game starts")]
    ///camera field of view applied when the game starts
    public float fieldOfView = 60f; 
 
    private CharacterController controller;
    private float pitch;
    private float verticalVelocity;
 
    /// <summary>
    /// References the CharacterController and finds the player camera if unassigned
    /// </summary>
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }
    
    ///Sets the camera FOV and locks the mouse cursor
    void Start()
    {
        playerCamera.fieldOfView = fieldOfView;
        LockCursor(true);
    }

    /// <summary>
    /// Handles player input, camera movement, and walking 
    /// pauses controls while ThoughtUI is displaying
    /// </summary>

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;
 
        if (keyboard.escapeKey.wasPressedThisFrame) LockCursor(false);
        if (mouse.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked) LockCursor(true);

        if (ThoughtUI.Instance != null && ThoughtUI.Instance.IsShowing) {
            return;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
            Look(mouse.delta.ReadValue());
 
        Move(keyboard);
    }
    
    ///Rotates the player horizontally and camera vertically based on mouse movement
    void Look(Vector2 delta)
    {
        transform.Rotate(Vector3.up, delta.x * mouseSensitivity);
        pitch = Mathf.Clamp(pitch - delta.y * mouseSensitivity, -maxPitch, maxPitch);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    
    ///Moves the player using WASD and applies gravity through CharacterController
    void Move(Keyboard keyboard)
    {
        Vector2 input = Vector2.zero;
        if (keyboard.wKey.isPressed) input.y += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.aKey.isPressed) input.x -= 1f;
        input = Vector2.ClampMagnitude(input, 1f); 
 
        Vector3 move = (transform.right * input.x + transform.forward * input.y) * walkSpeed;
 
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;
 
        controller.Move(move * Time.deltaTime);
    }
    
    ///Locks or unlocks the mouse cursor and controls its visibility
    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}