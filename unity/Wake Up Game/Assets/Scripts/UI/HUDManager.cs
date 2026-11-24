using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Manages the HUD of the player 
/// </summary>
public class HUDManager : MonoBehaviour
{
    /// <summary>
    /// Singleton allows other scripts to access HUDManager.Instance
    /// </summary>
    public static HUDManager Instance { get; private set; } 
    /// <summary>
    /// Awake when script is created
    /// </summary>
    [SerializeField] private DayManager dayManager;

    [Header("UI Visual Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject ObjectivesList;
    [SerializeField] private TextMeshProUGUI DayText;

    /// <summary>
    /// clock UI
    /// </summary>
    [Header("Clock UI")]
    [SerializeField] private GameObject ClockPanel;
    [SerializeField] private TextMeshProUGUI ClockTimeText;
    [SerializeField] private CanvasGroup ClockCanvasGroup;
    [SerializeField] private float fadeSpeed = 6f;

    private bool showClock = false;

    [Header("Objectives")]
    [Tooltip("Seconds a finished objective stays crossed out before hiding")]
    [SerializeField] private float objectiveHideDelay = 2f;

    private readonly Dictionary<MorningTask, TextMeshProUGUI> objectiveRows = new Dictionary<MorningTask, TextMeshProUGUI>();

    private void Awake() {
        Instance = this;

        ClockPanel.SetActive(false);
    }



    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        updateDay();
        BuildObjectives();
    }

    void OnDestroy()
    {
        if (TaskManager.Instance != null)
            TaskManager.Instance.TaskProgressed -= UpdateObjective;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update() {

        ///Updating day of the week
        updateDay();

        ///update the clock display
        UpdateClockUI();
    }

    ///show the clock UI when player looks at a clock object
    ///sets showClock to trigger the fade-in
    public void ShowClock()
    {
        showClock = true;
    }

    ///hide clock UI when player looks away from clock object
    ///Sets ShowClock to false to trigger the fade-out
    public void HideClock()
    {
        showClock = false;
    }

    /// <summary>
    /// gets the current formatted time from the GameClock singleton
    /// and updates the time that is displayed on the clock UI
    /// </summary>
    void UpdateClockUI()
    {
        if (ClockPanel.activeSelf && GameClock.Instance != null)
        {
            ClockTimeText.text = GameClock.Instance.GetFormattedTime();
        }

        UpdateClockFade();
    }

    /// <summary>
    /// fade the clock UI in and out when the player looks at a clock object
    /// </summary>
    void UpdateClockFade()
    {
        float targetAlpha = showClock ? 1f : 0f;

        ClockCanvasGroup.alpha = Mathf.MoveTowards(ClockCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        ClockCanvasGroup.interactable = showClock;
        ClockCanvasGroup.blocksRaycasts = showClock;

        ClockPanel.SetActive(ClockCanvasGroup.alpha > 0f);
    }

    void DisableObjective(GameObject objective)
    {
        objective.gameObject.SetActive(false);
    }
    void updateDay() {

        switch (DayManager.CurrentDay)
        {
            case 1:
                DayText.text = "MONDAY";
                break;
            case 2:
                DayText.text = "TUESDAY";
                break;
            case 3:
                DayText.text = "WEDNESDAY";
                break;
            case 4:
                DayText.text = "THURSDAY";
                break;
            case 5:
                DayText.text = "FRIDAY";
                break;
        }
    }
    /// <summary>
    /// the first text under ObjectivesList is the style template for every task line
    /// </summary>
    void BuildObjectives()
    {
        if (ObjectivesList == null || TaskManager.Instance == null) return;

        TextMeshProUGUI template = ObjectivesList.GetComponentInChildren<TextMeshProUGUI>(true);
        if (template == null)
        {
            Debug.LogWarning("ObjectivesList needs one text inside it to copy the style from");
            return;
        }

        foreach (Transform child in ObjectivesList.transform)
            child.gameObject.SetActive(false);

        foreach (MorningTask task in TaskManager.Instance.Today)
        {
            TextMeshProUGUI row = Instantiate(template, ObjectivesList.transform);
            row.name = "Objective " + task.Name;
            row.fontStyle &= ~FontStyles.Strikethrough;
            row.gameObject.SetActive(true);

            objectiveRows[task] = row;
            UpdateObjective(task);
        }

        TaskManager.Instance.TaskProgressed += UpdateObjective;
    }

    void UpdateObjective(MorningTask task)
    {
        if (!objectiveRows.TryGetValue(task, out TextMeshProUGUI row)) return;

        row.text = task.StepsRequired > 1
            ? $"{task.Name} ({task.Steps}/{task.StepsRequired})"
            : task.Name;

        if (task.IsComplete)
        {
            row.fontStyle |= FontStyles.Strikethrough;
            StartCoroutine(HideObjectiveLater(row));
        }
    }

    IEnumerator HideObjectiveLater(TextMeshProUGUI row)
    {
        yield return new WaitForSeconds(objectiveHideDelay);
        row.gameObject.SetActive(false);
    }
}
