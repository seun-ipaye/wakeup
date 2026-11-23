using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; } //Singleton allows other scripts to access HUDManager.Instance
    //Awake when script is created
    [SerializeField] private DayManager dayManager;

    [Header("UI Visual Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject ObjectivesList;
    [SerializeField] private TextMeshProUGUI DayText;

    //clock UI
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update() {

        //Updating day of the week
        updateDay();

        //update the clock display
        UpdateClockUI();
    }

    //show the clock UI when player looks at a clock object
    //sets showClock to trigger the fade-in
    public void ShowClock()
    {
        showClock = true;
    }

    //hide clock UI when player looks away from clock object
    //Sets ShowClock to false to trigger the fade-out
    public void HideClock()
    {
        showClock = false;
    }

    //gets the current formatted time from the GameClock singleton
    //and updates the time that is displayed on the clock UI
    void UpdateClockUI()
    {
        if (ClockPanel.activeSelf && GameClock.Instance != null)
        {
            ClockTimeText.text = GameClock.Instance.GetFormattedTime();
        }

        UpdateClockFade();
    }

    //fade the clock UI in and out when the player looks at a clock object
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
    //the first text under ObjectivesList is the style template for every task line
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
