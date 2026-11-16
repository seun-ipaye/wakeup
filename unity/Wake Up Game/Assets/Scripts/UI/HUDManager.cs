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
    

    [Header("UI Visual Components")]
    [SerializeField] TextMeshProUGUI GetDressedTaskText;
    [SerializeField] TextMeshProUGUI MakeCoffeeTaskText;
    [SerializeField] TextMeshProUGUI GrabKeysTaskText;
    [SerializeField] TextMeshProUGUI LeaveForWorkTaskText;

    private void Awake() {
        //GetDressedComplete.text = taskManager.

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
