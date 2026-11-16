using UnityEngine;
using TMPro;

//display the time of the GameClock on a UI element when the player interacts with it
public class Clock : MonoBehaviour
{
    public GameObject timeDisplay;
    public TextMeshPro timeText;

    private void Start()
    {
        timeDisplay.SetActive(false);
    }

    public void ShowTime()
    {
        if (GameClock.Instance == null)
            return;

        timeText.text = GameClock.Instance.GetFormattedTime();
        timeDisplay.SetActive(true);
    }

    public void HideTime()
    {
        timeDisplay.SetActive(false);
    }
}
