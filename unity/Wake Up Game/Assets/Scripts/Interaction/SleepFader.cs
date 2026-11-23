using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SleepFader : MonoBehaviour
{
    static SleepFader instance;
    public static bool Busy; //Prevents multiple sleep transitions from running at once
    float alpha;
    float open = 1;
    Texture2D black;

    static string pendingMessage;
    static bool pendingStayBlack;
    string message;
    float textAlpha;
    GUIStyle textStyle;

    //starts the sleep transition by fading the screen to black
    public static void Sleep(Action onBlack)
    {
        if (Busy) return;
        Busy = true;
        pendingMessage = null;
        pendingStayBlack = false;

        if (instance == null)
        {
            GameObject go = new GameObject("SleepFader");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<SleepFader>();
        }
        instance.StartCoroutine(instance.Run(onBlack));
    }

    // call inside the Sleep callback to show text on the black screen before waking up
    public static void ShowMessage(string text, bool stayBlack = false)
    {
        pendingMessage = text;
        pendingStayBlack = stayBlack;
    }

    void Awake()
    {
        black = new Texture2D(1, 1);
        black.SetPixel(0, 0, Color.black);
        black.Apply();
    }

    IEnumerator Run(Action onBlack)
    {
        yield return FadeTo(1, 1f);
        open = 0;
        alpha = 0;
        onBlack();

        if (!string.IsNullOrWhiteSpace(pendingMessage))
        {
            yield return ShowText(pendingMessage, pendingStayBlack);
            if (pendingStayBlack) yield break;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        yield return Move(0, 0.25f, 0.8f);
        yield return Move(0.25f, 0, 0.3f);
        yield return new WaitForSecondsRealtime(0.2f);
        yield return Move(0, 1, 0.7f);
        open = 1;
        Busy = false;
    }

    IEnumerator ShowText(string text, bool stayBlack)
    {
        message = text;
        yield return FadeText(0, 1, 0.4f);
        if (stayBlack) yield break;

        float readTime = Mathf.Clamp(1f + text.Length * 0.025f, 2f, 5f);
        for (float t = 0; t < readTime; t += Time.unscaledDeltaTime)
        {
            if (t > 0.5f && ContinuePressed()) break;
            yield return null;
        }

        yield return FadeText(1, 0, 0.3f);
        message = null;
    }

    static bool ContinuePressed()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        return (keyboard != null && (keyboard.eKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame))
            || (mouse != null && mouse.leftButton.wasPressedThisFrame);
    }

    IEnumerator FadeText(float from, float to, float time)
    {
        for (float t = 0; t < time; t += Time.unscaledDeltaTime)
        {
            textAlpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        textAlpha = to;
    }

    IEnumerator FadeTo(float to, float time)
    {
        float from = alpha;
        for (float t = 0; t < time; t += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        alpha = to;
    }

    IEnumerator Move(float from, float to, float time)
    {
        for (float t = 0; t < time; t += Time.unscaledDeltaTime)
        {
            open = Mathf.SmoothStep(from, to, t / time);
            yield return null;
        }
        open = to;
    }

    void OnGUI()
    {
        GUI.depth = -1000;

        if (alpha > 0)
        {
            GUI.color = new Color(1, 1, 1, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
        }

        if (open < 1)
        {
            GUI.color = new Color(1, 1, 1, 1 - open * 0.6f);
            float half = Screen.height / 2f * (1 - open);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, half + 1), black);
            GUI.DrawTexture(new Rect(0, Screen.height - half - 1, Screen.width, half + 1), black);
        }

        if (message != null)
        {
            if (textStyle == null)
            {
                textStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
            }
            textStyle.fontSize = Mathf.RoundToInt(Screen.height * 0.035f);
            GUI.color = new Color(1, 1, 1, textAlpha);
            GUI.Label(new Rect(Screen.width * 0.15f, 0, Screen.width * 0.7f, Screen.height), message, textStyle);
        }

        GUI.color = Color.white;
    }
}