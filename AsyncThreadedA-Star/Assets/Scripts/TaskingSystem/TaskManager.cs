using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    int taskCount;

    Queue<Task> pendingTasks = new Queue<Task>();
    List<Task> inProgressTasks = new List<Task>();
    List<Task> completeTasks = new List<Task>();

    private void Update()
    {
        taskCount = pendingTasks.Count;
    }

    public bool CheckForTask()
    {
        return pendingTasks.Count > 0;
    }

    public Task GetTask()
    {
        if (!CheckForTask())
            return null;
        Task t = pendingTasks.Dequeue();
        inProgressTasks.Add(t);
        return t;
    }

    public void CompleteTask(Task t)
    {
        if (inProgressTasks.Contains(t))
        {
            completeTasks.Add(t);
            inProgressTasks.Remove(t);
        }
    }

    public void CreateTask(TaskType type, GameObject targetObj)
    {
        Task task = new Task {
            TaskType = type,
            TargetObj = targetObj
        };
        pendingTasks.Enqueue(task);
    }

    public void CreateTask(TaskType type, Vector3 targetPos)
    {
        Task task = new Task
        {
            TaskType = type,
            TargetPos = targetPos
        };
        pendingTasks.Enqueue(task);
    }

    public void CreateTask(Task task)
    {
        pendingTasks.Enqueue(task);
    }

}
