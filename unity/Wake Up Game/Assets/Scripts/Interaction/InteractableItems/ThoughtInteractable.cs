using UnityEngine;
using System.Collections.Generic;

//for objects that trigger inner thought when interacted with

public class ThoughtInteractable : MonoBehaviour, InteractableInterface
{
    //days of the week
    public enum StoryDay
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5
    }

    //store thoughts for specific days
    [System.Serializable]
    public class DailyThought
    {
        public StoryDay day;

        [TextArea(2, 5)]
        public string thought;
    }

    [Header("Object ID")]
    [Tooltip("Give thought object a unique ID")]
    public string thoughtID;

    [Header("Daily Thoughts")]
    public DailyThought[] thoughts;

    //track which day each object was examined
    private static Dictionary<string, int> examinedObjects = new Dictionary<string, int>();

    //prompt when looking at object
    public string Prompt => "Look closely";

    //ensure there is a thought assigned before allowing interaction and it has not already been interacted with this day
    public bool CanInteract => !string.IsNullOrEmpty(thoughtID) && GetThoughtForToday() != null && !ExaminedToday();

    //interact to display thought for the day
    public void Interact()
    {
        string thought = GetThoughtForToday();

        if (thought == null || ExaminedToday()) {
            return;
        }

        //mark object as examined for current day
        examinedObjects[thoughtID] = DayManager.CurrentDay;

        if (thought != null && ThoughtUI.Instance != null)
        {
            ThoughtUI.Instance.ShowThought(thought);
        }
    }

    //get current day thought if not null
    string GetThoughtForToday()
    {
        foreach (DailyThought entry in thoughts)
        {
            if (entry == null)
                continue;

            if ((int)entry.day == DayManager.CurrentDay)
            {
                return entry.thought;
            }
        }

        //no thought assigned for that day
        return null;
    }

    //check if object has been examined today
    bool ExaminedToday()
    {
        if (string.IsNullOrEmpty(thoughtID))
        {
            return false;
        }

        return examinedObjects.TryGetValue(thoughtID, out int examinedDay) && examinedDay == DayManager.CurrentDay;
    }

    //clear the thought history when starting a new week
    public static void ResetThoughtHistory()
    {
        examinedObjects.Clear();
    }
}