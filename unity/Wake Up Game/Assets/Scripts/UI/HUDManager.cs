using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; } //reference to this script
    //Awake when script is created

    [Header("UI Visual Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI checklistText;

    [Header("UI Visual Components")]
    private bool isGetDressedComplete = false;
    private bool isMakeCoffeeComplete = false;
    private bool isGrabKeysComplete = false;
    private bool isLeaveForWorkComplete = false;

    private void Awake() {
        if(Instance == null) {   //if no instance yet
            Instance = this;
        }
        else {
            Destroy(gameObject);    //if there is already an instance, destroy this one, no dupes
        }

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UpdateUI();
    }

    // Update is called once per frame
    void Update() {
        
    }
    /*
    void UpdateUI() { 
        if(isGetDressedComplete) {
            titleText.text = "Get Dressed";
        }
        else {
            titleText.text = "Get Dressed";
        }
        if(isMakeCoffeeComplete) {
            checklistText.text = "Make Coffee";
        }
        else {
            checklistText.text = "Make Coffee";
        }
        if(isGrabKeysComplete) {
            checklistText.text += "Grab Keys";
        }
        else {
            checklistText.text += "Grab Keys";
        }
        if(isLeaveForWorkComplete) {
            checklistText.text += "Leave for Work";
        }
        else {
            checklistText.text += "Leave for Work";
        }

    }
    */
}
