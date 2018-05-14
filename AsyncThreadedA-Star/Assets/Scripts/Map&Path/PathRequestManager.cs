using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    PathFinding pathFinding;

    static PathRequestManager instance;

    public PathFinding PathFinding
    {
        get
        {
            return pathFinding;
        }

        set
        {
            pathFinding = value;
        }
    }

    public static PathRequestManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(PathRequest request)
    {
        Thread t = new Thread(() => instance.pathFinding.FindPath(request, instance.FinishedProcssingPath));
        t.Start();
    }

    public void FinishedProcssingPath(PathResult originalRequest)
    {
        originalRequest.CallBack(originalRequest.path, originalRequest.success);
    }
}

public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public Action<Vector3[], bool> CallBack;

    public PathRequest(Vector3 PathStart, Vector3 PathEnd, Action<Vector3[], bool> CallBack)
    {
        this.PathStart = PathStart;
        this.PathEnd = PathEnd;
        this.CallBack = CallBack;
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> CallBack;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> CallBack)
    {
        this.path = path;
        this.success = success;
        this.CallBack = CallBack;
    }
}
