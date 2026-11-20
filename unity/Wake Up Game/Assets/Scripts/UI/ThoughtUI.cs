using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//controls display of inner thought dialogue

public class ThoughtUI : MonoBehaviour
{
    public static ThoughtUI Instance { get; private set; }

    [Header("UI References")]
    public Canvas thoughtCanvas;
    public CanvasGroup thoughtPanel;
    public Text thoughtText;

    [Header("Timing")]
    public float fadeDuration = 0.3f;
    public float displayDuration = 4f;

    private Coroutine displayRoutine;

    public bool IsShowing { get; private set; }

    void Awake()
    {
        Instance = this;

        //hide UI when scene starts
        thoughtPanel.alpha = 0f;
        thoughtCanvas.gameObject.SetActive(false);
    }

    //display thought
    public void ShowThought(string message)
    {
        //stop previous thought if one is displaying
        if (displayRoutine != null)
            StopCoroutine(displayRoutine);

        displayRoutine = StartCoroutine(DisplayThought(message));
    }

    IEnumerator DisplayThought(string message)
    {
        IsShowing = true;

        thoughtText.text = message;
        thoughtCanvas.gameObject.SetActive(true);
        thoughtPanel.alpha = 0f;

        //fade in
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            thoughtPanel.alpha = timer / fadeDuration;
            yield return null;
        }

        thoughtPanel.alpha = 1f;

        //stay on screen
        yield return new WaitForSeconds(displayDuration);

        //fade out
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            thoughtPanel.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        thoughtPanel.alpha = 0f;
        thoughtCanvas.gameObject.SetActive(false);

        IsShowing = false;
        displayRoutine = null;
    }
}