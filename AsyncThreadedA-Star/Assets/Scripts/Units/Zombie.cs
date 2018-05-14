using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Vector;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    TaskManager taskManager;

    BaseUnit baseUnit;
    PlayerController player;
    bool inProgress;
    [SerializeField]
    Task currentTask;
    Task mainTask;
    Queue<Task> tasks = new Queue<Task>();
    [SerializeField]
    bool isWaiting;

    public TaskManager TaskManager
    {
        get
        {
            return taskManager;
        }

        set
        {
            taskManager = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        baseUnit = GetComponent<BaseUnit>();
        player = PlayerController.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (HasTasks())
        {
            if (!HasCurrentTask())
            {
                currentTask = GetNextTask();
                PerformTask();
            }
            else
            {
                CheckTaskProgress();
            }
        }
        else
        {
            GetMainTask();
        }
    }

    void GetMainTask()
    {
        if (mainTask == null)
        {
            mainTask = TaskManager.GetTask();
            if (mainTask == null)
            {
                mainTask = new Task() { TaskType = TaskType.Guard, TargetObj = player.gameObject };
            }
            GenerateRequiredTasks();
        }
    }

    void GenerateRequiredTasks()
    {
        if (mainTask == null)
            return;
        switch (mainTask.TaskType)
        {
            case TaskType.Gather:
                tasks.Enqueue(new Task { TaskType = TaskType.Move, TargetPos = mainTask.TargetObj.transform.position });
                tasks.Enqueue(new Task { TaskType = TaskType.Wait, WaitTime = .5f });
                tasks.Enqueue(new Task { TaskType = TaskType.Action, Action = () => print("Gathered resource " + mainTask.TargetObj.name) });
                break;
            case TaskType.Guard:
                Vector3 pos = mainTask.TargetObj.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
                bool esc = true;
                int i = 0;
                while (esc)
                {
                    if (!PathRequestManager.Instance.PathFinding.Grid.NodeFromWorldPoint(pos).walkable)
                    {
                        pos = mainTask.TargetObj.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
                    }
                    else
                    {
                        esc = false;
                    }
                    if (i++ > 100)
                    {
                        print("fuck");
                        break;
                    }
                }
                tasks.Enqueue(new Task
                {
                    TaskType = TaskType.Move,
                    TargetPos = mainTask.TargetObj.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f))
                });
                tasks.Enqueue(new Task { TaskType = TaskType.Wait, WaitTime = .5f });
                break;
            default:
                break;
        }
    }

    bool HasTasks()
    {
        if (tasks.Count > 0 || mainTask != null)
        {
            return true;
        }
        return false;
    }

    bool HasCurrentTask()
    {
        return currentTask != null;
    }

    void PerformTask()
    {
        if (currentTask == null)
            return;

        switch (currentTask.TaskType)
        {
            case TaskType.Move:
                baseUnit.Move(currentTask.TargetPos);
                break;
            case TaskType.Gather:
                currentTask = null;
                mainTask = null;
                break;
            case TaskType.Wait:
                if (!isWaiting)
                    StartCoroutine(TaskWait(currentTask.WaitTime));
                break;
            case TaskType.Action:
                currentTask.Action();
                print("action performed");
                currentTask = GetNextTask();
                PerformTask();
                break;
            default:
                break;
        }
    }

    Task GetNextTask()
    {
        if (tasks.Count > 0)
        {
            return tasks.Dequeue();
        }
        return mainTask;
    }

    void CheckTaskProgress()
    {
        switch (currentTask.TaskType)
        {
            case TaskType.Move:
                float f = transform.position.ZXDistance(currentTask.TargetPos);
                if (f < 1f || baseUnit.PathFailed)
                {
                    if (currentTask == mainTask)
                    {
                        taskManager.CompleteTask(mainTask);
                        mainTask = null;
                    }
                    currentTask = null;
                }
                break;
            case TaskType.Gather:
                if (transform.position.ZXDistance(currentTask.TargetPos) < 1f)
                {
                    if (currentTask == mainTask)
                    {
                        taskManager.CompleteTask(mainTask);
                        mainTask = null;
                    }
                    currentTask = null;
                }
                break;
            case TaskType.Guard:
                GenerateRequiredTasks();
                currentTask = null;
                break;
            case TaskType.Wait:
                if (!isWaiting)
                {
                    currentTask = null;
                }
                break;
            default:
                break;
        }

        if (mainTask != null && mainTask.TaskType == TaskType.Guard)
        {
            if (taskManager.CheckForTask())
            {
                tasks.Clear();
                currentTask = null;
                mainTask = null;
            }
        }
    }

    IEnumerator TaskWait(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
        currentTask = null;
        yield return null;
    }

}
