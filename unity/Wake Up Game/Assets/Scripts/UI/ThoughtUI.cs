using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//controls display of inner thought dialogue

public class ThoughtUI : MonoBehaviour
{   
    //allows other scripts to access the ThoughtUI
    public static ThoughtUI Instance { get; private set; }

    [Header("UI References")]

    //Canvas cointaining the thought UI
    public Canvas thoughtCanvas;

    //controls the transparency of the thought panel
    public CanvasGroup thoughtPanel;

    //text component that displays the thought message
    public Text thoughtText;

    //access the players camera that will zoom in on the thought
    public Camera playerCamera;

    //fadeDuration is the time it takes for the thought fade in and out, displayyDuration is the time setting for displaying thoughts
    [Header("Timing")]
    public float fadeDuration = 0.3f;
    public float displayDuration = 4f;

    //controls the camera's zoom effect by reducing the field of view
    [Header("Camera Zoom")]
    public float zoomAmount = 5f;

    //stores the currently running thought coroutine
    private Coroutine displayRoutine;
    //stores the camera's original field of view
    private float originalFOV;

    //checks if a thought is currently displaying 
    public bool IsShowing { get; private set; }



    void Awake()
    {   //set this object as the ThoughtUI instance
        Instance = this;

        //save the camera's original field of view
        originalFOV = playerCamera.fieldOfView;

        //hide UI when scene starts
        thoughtPanel.alpha = 0f;
        thoughtCanvas.gameObject.SetActive(false);
    }

    //display thought
    public void ShowThought(string message, float duration)
    {
        //stop previous thought if one is displaying
        if (displayRoutine != null)
        {
            StopCoroutine(displayRoutine);
        }

        //start the coroutine that displays the new thought
        displayRoutine = StartCoroutine(DisplayThought(message, duration));
    }

    //handles displaying, fading and hiding thoughts
    IEnumerator DisplayThought(string message, float duration)
    {
        IsShowing = true;

        thoughtText.text = message;
        thoughtCanvas.gameObject.SetActive(true);
        thoughtPanel.alpha = 0f;

        float zoomedFOV = originalFOV - zoomAmount;

        //fade in
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / fadeDuration);

            thoughtPanel.alpha = progress;
            playerCamera.fieldOfView = Mathf.Lerp(originalFOV, zoomedFOV, progress);

            yield return null;
        }

        thoughtPanel.alpha = 1f;
        playerCamera.fieldOfView = zoomedFOV;

        //display thought until duration or E is pressed
        timer = 0f;

        while (timer < duration)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        //fade out and restore camera zoom

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / fadeDuration);

            thoughtPanel.alpha = 1f - progress;
            playerCamera.fieldOfView = Mathf.Lerp(zoomedFOV, originalFOV, progress);

            yield return null;
        }

        //Rest UI and active state
        thoughtPanel.alpha = 0f;
        playerCamera.fieldOfView = originalFOV;
        thoughtCanvas.gameObject.SetActive(false);

        IsShowing = false;
        displayRoutine = null;
    }
}