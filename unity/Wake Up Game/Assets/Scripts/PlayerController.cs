using UnityEngine;
using UnityEngine.InputSystem;
 
// First person player controller. (W/A/S/D walk, no sprint or jump)
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Walking speed in mps (target: 2.5 m/s)")]
    public float walkSpeed = 2.5f;
    public float gravity = -9.81f;
 
    [Header("Look")]
    [Tooltip("The first-person camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 0.1f;
    [Tooltip("How far up/down the player can look")]
    public float maxPitch = 80f;
    [Tooltip("FOV (target: 70-80).")]
    public float fieldOfView = 75f;
 
    private CharacterController controller;
    private float pitch;
    private float verticalVelocity;
 
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }
 
    void Start()
    {
        playerCamera.fieldOfView = fieldOfView;
        LockCursor(true);
    }
 
    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;
 
        if (keyboard.escapeKey.wasPressedThisFrame) LockCursor(false);
        if (mouse.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked) LockCursor(true);
 
        if (Cursor.lockState == CursorLockMode.Locked)
            Look(mouse.delta.ReadValue());
 
        Move(keyboard);
    }
 
    void Look(Vector2 delta)
    {
        transform.Rotate(Vector3.up, delta.x * mouseSensitivity);
        pitch = Mathf.Clamp(pitch - delta.y * mouseSensitivity, -maxPitch, maxPitch);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
 
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
 
    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}