using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    [SerializeField] Vector3 followOffset;
    [SerializeField] float smoothSpeed, fovZoomSpeed, minZoom, maxZoom, zoomLimiter, minHeight, maxHeight;
    [SerializeField] Material[] skyboxes;
    Camera cam;

    [Space]

    [SerializeField] bool staticCamera;
    [SerializeField] Vector3 staticPos;
    [SerializeField] float staticFov, moveTreshold;
    int num;

    void Start()
    {
        cam = GetComponent<Camera>();
        num = Random.Range(0, skyboxes.Length);
        RenderSettings.skybox = skyboxes[num];

        if (staticCamera)
        {
            cam.fieldOfView = staticFov;
        }
    }

    void LateUpdate()
    {
        if (targets.Length == 0)
        {
            return;
        }

        if (targets[0] == null)
        {
            targets[0] = GameObject.Find("Body 1").transform;
        }

        if (targets[1] == null)
        {
            targets[1] = GameObject.Find("Body 2").transform;
        }

        if (staticCamera)
        {
            SmallMovement();
        }
        else
        {
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
        }
    }

    void MoveCamera()
    {
        Vector3 desiredPos = GetCenterPoint() + followOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, (GetBiggestDistance('x') + GetBiggestDistance('y') * 2) / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, fovZoomSpeed * Time.deltaTime);
    }

    float GetBiggestDistance(char thing)
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
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

    void SmallMovement()
    {
        Vector3 pos = GetCenterPoint() + followOffset;
        if (pos.y < staticPos.y)
        {
            pos.y = staticPos.y;
        }
        Vector3 desiredPos = Vector3.Lerp(staticPos, new Vector3(pos.x, pos.y, staticPos.z), moveTreshold);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
        {
            return targets[0].position;
        }
        else
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Length; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
    }
}