using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//controls display of inner thought dialogue

public class ThoughtUI : MonoBehaviour
{
    public static ThoughtUI Instance { get; private set; }

    [Header("UI References")]
    public Canvas thoughtCanvas;
    public CanvasGroup thoughtPanel;
    public Text thoughtText;
    public Camera playerCamera;

    [Header("Timing")]
    public float fadeDuration = 0.3f;
    public float displayDuration = 4f;

    [Header("Camera Zoom")]
    public float zoomAmount = 5f;

    private Coroutine displayRoutine;
    private float originalFOV;

    public bool IsShowing { get; private set; }

    void Awake()
    {
        Instance = this;

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

        displayRoutine = StartCoroutine(DisplayThought(message, duration));
    }

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

        //stay on screen
        timer = 0f;

        while (timer < duration)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        //fade out and zoom out

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / fadeDuration);

            thoughtPanel.alpha = 1f - progress;
            playerCamera.fieldOfView = Mathf.Lerp(zoomedFOV, originalFOV, progress);

            yield return null;
        }

        thoughtPanel.alpha = 0f;
        playerCamera.fieldOfView = originalFOV;
        thoughtCanvas.gameObject.SetActive(false);

        IsShowing = false;
        displayRoutine = null;
    }
}