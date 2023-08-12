using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField] Vector3 followOffset;
    [SerializeField] float smoothSpeed, fovZoomSpeed, minZoom, maxZoom, zoomLimiter, minHeight, maxHeight;
    //Transform x1, x2;
    Camera cam;
    float maxDistanceX, maxDistanceY;

    void Start()
    {
        cam = GetComponent<Camera>();
        //x1 = GameObject.Find("x1").transform;
        //x1 = GameObject.Find("x1").transform;
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }
        MoveCamera();
        Zoom();

        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
        else
        if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }

        if (maxDistanceX < GetBiggestDistance('x'))
        {
            maxDistanceX = GetBiggestDistance('x');
        }

        if (maxDistanceY < GetBiggestDistance('y'))
        {
            maxDistanceY = GetBiggestDistance('y');
        }
        Debug.Log($"Max X: {maxDistanceX}     Max Y: {maxDistanceY}");
    }

    void MoveCamera()
    {
        Vector3 desiredPos = GetCenterPoint() + followOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, (GetBiggestDistance('x') + GetBiggestDistance('y')) / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, fovZoomSpeed * Time.deltaTime);
    }

    float GetBiggestDistance(char thing)
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        if (thing == 'x')
        {
            return bounds.size.x;
        }
        else
        {
            return bounds.size.y;
        }
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        else
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
    }
}