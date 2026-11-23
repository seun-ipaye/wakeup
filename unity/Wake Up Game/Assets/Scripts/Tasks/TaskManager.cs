using System;
using System.Collections.Generic;
using UnityEngine;

//represents a morning task, including its name and required steps
//completed steps, display order on the HUD
public class MorningTask
{
    public string Name;            
    public int StepsRequired;   
    public int Steps;            
    public int HudOrder;

    //returns true when all required steps for this task are completed
    public bool IsComplete => Steps >= StepsRequired;
}

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Serializable]
    //allows specific days to have different tasks instead of all available tasks
    public class DayOverride
    {   
        [Min(1)] public int day = 1;
        [Tooltip("Task names to use on this day, spelled like on the objects")]
        public List<string> taskNames = new List<string>();
    }

    [Tooltip("Optional: give a specific day its own tasks. Leave empty to use every task in the house every day")]
    public List<DayOverride> dayOverrides = new List<DayOverride>();

    [Tooltip("K_r in the GDD: tasks needed before going to work. 0 = all of them")]

    //Number os tasks required before the player can go to work, 0 means all of today's task must be completed
    [Min(0)] public int tasksRequired = 0;

    //Read only list of tasks assigned to the current day
    public IReadOnlyList<MorningTask> Today => today;
    private readonly List<MorningTask> today = new List<MorningTask>();

    //Event triggered whenever a task step is completed
    public event Action<MorningTask> TaskProgressed;

    //Event triggered when an individual task is fully completed
    public event Action<MorningTask> TaskCompleted;
    //Event triggered when all of today's tasks are completed
    public event Action AllTasksCompleted;

    void Awake()
    {
        Instance = this;
        //finds task objects in the scene and counts their required steps
        Dictionary<string, MorningTask> allTasks = FindTasksInScene();
        PickTasksFor(DayManager.CurrentDay, allTasks);

        Debug.Log($"Today's tasks: {string.Join(", ", today.ConvertAll(t => t.Name))}");
    }

    Dictionary<string, MorningTask> FindTasksInScene()
    {
        var tasks = new Dictionary<string, MorningTask>();

        foreach (TaskInteractable obj in FindObjectsByType<TaskInteractable>(FindObjectsSortMode.None))
        {
            string key = Key(obj.taskName);
            if (key == "")
            {
                Debug.LogWarning($"TaskInteractable on '{obj.name}' has no task name", obj);
                continue;
            }

            if (!tasks.TryGetValue(key, out MorningTask task))
            {
                task = new MorningTask { Name = obj.taskName.Trim(), HudOrder = obj.hudOrder };
                tasks[key] = task;
            }

            task.StepsRequired++;
            task.HudOrder = Mathf.Min(task.HudOrder, obj.hudOrder);
        }

        return tasks;
    }

    //Selects tasks for the current day using DayOverride
    void PickTasksFor(int day, Dictionary<string, MorningTask> allTasks)
    {
        today.Clear();

        DayOverride entry = dayOverrides.Find(o => o.day == day);
        if (entry == null)
        {
            today.AddRange(allTasks.Values);
        }
        else
        {
            foreach (string name in entry.taskNames)
            {
                if (allTasks.TryGetValue(Key(name), out MorningTask task))
                    today.Add(task);
                else
                    Debug.LogWarning($"Day {day} override lists '{name}' but no object has that task name");
            }
        }

        today.Sort((a, b) => a.HudOrder != b.HudOrder
            ? a.HudOrder.CompareTo(b.HudOrder)
            : string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
    }

    //converts a task name into a consistent key for matching 
    public static string Key(string taskName)
    {
        if (string.IsNullOrWhiteSpace(taskName)) return "";
        string[] words = taskName.Trim().ToLowerInvariant()
            .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words);
    }

    //Returns today's task matching the provided task name
    public MorningTask Get(string taskName)
    {
        string key = Key(taskName);
        return today.Find(t => Key(t.Name) == key);
    }

    //Checks whether a task is assigned for today
    public bool IsToday(string taskName) => Get(taskName) != null;

    //Completes one step of the specified task and triggers progression events
    public void CompleteStep(string taskName)
    {
        MorningTask task = Get(taskName);
        if (task == null)
        {
            Debug.LogWarning($"'{taskName}' isn't one of today's tasks");
            return;
        }

        if (task.IsComplete) return;

        task.Steps++;
        TaskProgressed?.Invoke(task);

        if (task.IsComplete)
        {
            Debug.Log($"Task complete: {task.Name}");
            TaskCompleted?.Invoke(task);

            if (AllComplete)
            {
                Debug.Log("All tasks complete");
                AllTasksCompleted?.Invoke();
            }
        }
    }

    //Returns the number of fully completed tasks today
    public int CompletedCount => today.FindAll(t => t.IsComplete).Count;

    //Returns true when all tasks assigned for today are complete
    public bool AllComplete => CompletedCount >= today.Count;

    //Returns true when the require number of tasks has been completed
    public bool RequiredTasksDone
    {
        get
        {
            int needed = tasksRequired > 0 ? Mathf.Min(tasksRequired, today.Count) : today.Count;
            return CompletedCount >= needed;
        }
    }
}
