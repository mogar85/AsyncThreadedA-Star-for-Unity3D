using UnityEngine;
using Extensions.Vector;

public class BaseUnit : MonoBehaviour
{
    public float speed = 20f;
    public float turnDist = 5;
    public float stoppingDist = 10f;
    public float turnSpeed = 3;
    public float rotRange = 10;
    Path path;
    bool isRunning;
    int pathIndex = 0;
    float speedPercent = 1f;
    bool followingPath = false;
    bool pathFailed = false;

    public bool PathFailed
    {
        get
        {
            return pathFailed;
        }

        set
        {
            pathFailed = value;
        }
    }

    public void Move(Vector3 targetPos)
    {
        path = null;
        pathFailed = false;
        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPos, OnPathFound));
    }

    public void OnPathFound(Vector3[] waypoints, bool success)
    {
        if (success)
        {
            path = new Path(waypoints, waypoints[0], turnDist, stoppingDist);
            pathIndex = 0;
            speedPercent = 1f;
            followingPath = true;
        }
        else
        {
            path = null;
            pathFailed = true;
        }
    }

    private void FixedUpdate()
    {
        if (path != null)
        {
            if (followingPath)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
                //if (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                if (Vector3.Distance(transform.position, path.lookPoints[pathIndex]) < .25f)
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        path = null;
                    }
                    else
                        pathIndex++;
                }

                if (followingPath)
                {
                    if (pathIndex >= path.slowDownIndex && stoppingDist > transform.position.ZXDistance(path.lookPoints[path.finishLineIndex]))
                    {
                        //speedPercent = Mathf.Clamp01(transform.position.ZXDistance(path.lookPoints[0]) / stoppingDist);
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDist);
                        if (speedPercent < 0.01f)
                            followingPath = false;
                    }
                    if (path.lookPoints[pathIndex] - transform.position != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

                        if (transform.rotation.eulerAngles.y > targetRotation.eulerAngles.y - rotRange && transform.rotation.eulerAngles.y < targetRotation.eulerAngles.y + rotRange)
                        {
                            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                        }
                    }
                }
            }
        }
    }

    //private IEnumerator FollowPath()
    //{
    //    isRunning = true;
    //    bool followingPath = true;
    //    int pathIndex = 0;
    //    //transform.LookAt(path.lookPoints[0]);

    //    float speedPercent = 1f;

    //    while (followingPath)
    //    {
    //        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
    //        while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
    //        {
    //            if (pathIndex == path.finishLineIndex)
    //            {
    //                followingPath = false;
    //                break;
    //            }
    //            else
    //                pathIndex++;
    //        }

    //        if (followingPath)
    //        {
    //            if (pathIndex >= path.slowDownIndex && stoppingDist > 0)
    //            {
    //                speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDist);
    //                if (speedPercent < 0.01f)
    //                    followingPath = false;
    //            }
    //            Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
    //            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    //            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
    //        }

    //        yield return null;
    //    }
    //    isRunning = false;
    //}

    //public void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        path.DrawWithGizmos();
    //    }
    //}
}
