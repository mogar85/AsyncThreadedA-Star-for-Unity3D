using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform pointer;
    public float gamma;

    BaseUnit unit;
    Selectable currentSelected;
    Queue<CollectResources> quedTasks = new Queue<CollectResources>();

    TaskManager taskManager;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        unit = GetComponent<BaseUnit>();
        taskManager = GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, gamma);
            print("derp");
        }
        MouseInput();
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                Vector3 newPos = hit.point;
                newPos.y = 0;
                pointer.position = newPos;
                unit.Move(pointer.position);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (hit.collider.GetComponent<Selectable>())
                    Selector(hit.collider.GetComponent<Selectable>());
            }
        }

    }

    void Selector(Selectable selectable)
    {
        currentSelected = selectable;
        if (selectable)
            taskManager.CreateTask(TaskType.Gather, selectable.gameObject);
    }

    void ActionInput()
    {
        if (currentSelected == null)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            quedTasks.Enqueue(new CollectResources(currentSelected.transform.position));
        }
    }

    public CollectResources CheckForTask()
    {
        if (quedTasks.Count > 0)
        {
            return quedTasks.Dequeue();
        }
        return null;
    }

}

public class CollectResources
{
    public Vector3 position;

    public CollectResources(Vector3 pos)
    {
        position = pos;
    }
}