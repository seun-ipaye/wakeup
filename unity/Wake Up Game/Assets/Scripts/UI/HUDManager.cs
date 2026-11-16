using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; } //reference to this script
    //Awake when script is created
    [SerializeField] private TaskManager taskManager;
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

    [Header("UI Visual Components")]
    [SerializeField] TextMeshProUGUI GetDressedTaskText;
    [SerializeField] TextMeshProUGUI MakeCoffeeTaskText;
    [SerializeField] TextMeshProUGUI GrabKeysTaskText;
    [SerializeField] TextMeshProUGUI LeaveForWorkTaskText;

    private void Awake() {
        //GetDressedComplete.text = taskManager.
        Instance = this;

        ClockPanel.SetActive(false);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UpdateUI();
        updateDay();
        UpdateUI();
    }

    // Update is called once per frame
    void Update() {

        //Updating day of the week
        updateDay();

        //Updating objectives
        UpdateUI();

        //update the clock display
        UpdateClockUI();
    }
    void UpdateUI() { 
        if (taskManager.isGetDressedComplete) {   //Dressed objective complete
            GetDressedTaskText.fontStyle = FontStyles.Strikethrough;
            Invoke(nameof(DisableDressed), 2f);
        }
        if (taskManager.isMakeCoffeeComplete) {   //Make coffee objective complete
            MakeCoffeeTaskText.fontStyle = FontStyles.Strikethrough;
            Invoke(nameof(DisableCoffee), 2f);
        }
        if (taskManager.isGrabKeysComplete) {   //Grab keys objective complete
            GrabKeysTaskText.fontStyle = FontStyles.Strikethrough;
            Invoke(nameof(DisableKeys), 2f);
        }
        if (taskManager.isLeaveForWorkComplete) { //Leave for work objective complete
            LeaveForWorkTaskText.fontStyle = FontStyles.Strikethrough;
            Invoke(nameof(DisableLeaveForWork), 2f);
        }

    }


    //show the clock UI when player looks at a clock object
    public void ShowClock()
    {
        showClock = true;
    }

    //hide clock UI when player looks away from clock object
    public void HideClock()
    {
        showClock = false;
    }

    //update the time that is displayed on the clock UI
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

    void DisableDressed()
    {
        GetDressedTaskText.gameObject.SetActive(false);
    }
    void DisableKeys()
    {
        GrabKeysTaskText.gameObject.SetActive(false);
    }
    void DisableLeaveForWork()
    {
        LeaveForWorkTaskText.gameObject.SetActive(false);
    }
    void DisableCoffee()
    {
        MakeCoffeeTaskText.gameObject.SetActive(false);
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
    void updateObjectives()
    {


    }
}
