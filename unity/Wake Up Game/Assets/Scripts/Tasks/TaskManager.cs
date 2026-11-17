using System;
using System.Collections.Generic;
using UnityEngine;

public class MorningTask
{
    public string Name;            
    public int StepsRequired;   
    public int Steps;            
    public int HudOrder;

    public bool IsComplete => Steps >= StepsRequired;
}

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Serializable]
    public class DayOverride
    {
        [Min(1)] public int day = 1;
        [Tooltip("Task names to use on this day, spelled like on the objects")]
        public List<string> taskNames = new List<string>();
    }

    [Tooltip("Optional: give a specific day its own tasks. Leave empty to use every task in the house every day")]
    public List<DayOverride> dayOverrides = new List<DayOverride>();

    [Tooltip("K_r in the GDD: tasks needed before going to work. 0 = all of them")]
    [Min(0)] public int tasksRequired = 0;

    public IReadOnlyList<MorningTask> Today => today;
    private readonly List<MorningTask> today = new List<MorningTask>();

    public event Action<MorningTask> TaskProgressed;
    public event Action<MorningTask> TaskCompleted;
    public event Action AllTasksCompleted;

    void Awake()
    {
        Instance = this;

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

    public static string Key(string taskName)
    {
        if (string.IsNullOrWhiteSpace(taskName)) return "";
        string[] words = taskName.Trim().ToLowerInvariant()
            .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words);
    }

    public MorningTask Get(string taskName)
    {
        string key = Key(taskName);
        return today.Find(t => Key(t.Name) == key);
    }

    public bool IsToday(string taskName) => Get(taskName) != null;

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

    public int CompletedCount => today.FindAll(t => t.IsComplete).Count;

    public bool AllComplete => CompletedCount >= today.Count;

    public bool RequiredTasksDone
    {
        get
        {
            int needed = tasksRequired > 0 ? Mathf.Min(tasksRequired, today.Count) : today.Count;
            return CompletedCount >= needed;
        }
    }
}
