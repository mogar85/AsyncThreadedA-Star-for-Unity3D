using System;
using UnityEngine;

public class Task
{
    public TaskType TaskType;
    public GameObject TargetObj;
    public Vector3 TargetPos;
    public float WaitTime;
    public Action Action;
}
