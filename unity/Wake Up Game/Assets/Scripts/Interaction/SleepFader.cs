using System;
using System.Collections;
using UnityEngine;

public class SleepFader : MonoBehaviour
{
    static SleepFader instance;
    public static bool Busy;
    float alpha;
    float open = 1;
    Texture2D black;

    public static void Sleep(Action onBlack)
    {
        if (Busy) return;
        Busy = true;

        if (instance == null)
        {
            GameObject go = new GameObject("SleepFader");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<SleepFader>();
        }
        instance.StartCoroutine(instance.Run(onBlack));
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
        yield return new WaitForSecondsRealtime(0.5f);
        yield return Move(0, 0.25f, 0.8f);
        yield return Move(0.25f, 0, 0.3f);
        yield return new WaitForSecondsRealtime(0.2f);
        yield return Move(0, 1, 0.7f);
        open = 1;
        Busy = false;
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

        GUI.color = Color.white;
    }
}