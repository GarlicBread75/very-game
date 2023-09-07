using UnityEngine;

public class SquashAndStretch2 : MonoBehaviour
{
    Rigidbody rb;
    public Transform rotatableTransform;
    public Transform scalableTransform;

    public float stretchMultiplier;
    public float squashMultiplier;
    public float delayMultiplier;
    public float scaleChangeRate;
    float currentScale;
    float targetScale;

    Quaternion targetRot;
    Quaternion currentRot;

    Vector3 savedVelocity;
    Vector3 savedContactNormal;

    bool ground;
    bool inverted;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (!ground)
        {
            targetRot = Quaternion.LookRotation(rb.velocity, Vector3.forward);
            float velocity = rb.velocity.magnitude;
            targetScale = 1f + velocity * velocity * stretchMultiplier;
            targetScale = Mathf.Clamp(targetScale, 1f, 2f);
        }

        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleChangeRate);
        SquashScale(currentScale);

        if (!inverted && currentScale >= 1f)
        {
            inverted = true;
            rotatableTransform.rotation = targetRot = currentRot = Quaternion.LookRotation(savedContactNormal, Vector3.forward);
        }
        currentRot = Quaternion.Lerp(currentRot, targetRot, Time.deltaTime * 10f);
        rotatableTransform.rotation = currentRot;
    }

    void SquashScale(float value)
    {
        if (value == 0f)
        {
            return;
        }
        scalableTransform.localScale = new Vector3(1 / value, value, 1 / value);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (ground)
        {
            return;
        }
        ground = true;

        savedVelocity = rb.velocity;
        savedContactNormal = collision.contacts[0].normal;
        rb.isKinematic = true;

        targetRot = Quaternion.LookRotation(-collision.contacts[0].normal, Vector3.forward);

        targetScale = Mathf.Lerp(1f, 0.3f, savedVelocity.magnitude * squashMultiplier);

        float velocityProjectionMagnitude = Vector3.Project(savedVelocity, -savedContactNormal).magnitude;
        float groundedTime = velocityProjectionMagnitude * delayMultiplier;
        groundedTime = Mathf.Clamp(groundedTime, 0f, 0.15f);

        transform.position = collision.contacts[0].point + collision.contacts[0].normal * 0.5f;

        Invoke("StartToStretch", groundedTime);
        Invoke("DisableIsKinematic", groundedTime * 1.5f);
    }

    void StartToStretch()
    {
        targetScale = Mathf.Lerp(0.5f, 1f, 1f + savedVelocity.magnitude * stretchMultiplier);
        inverted = false;
    }

    void DisableIsKinematic()
    {
        rb.isKinematic = false;
        Invoke("ExitSaveMode", 0.02f);
    }

    void ExitSaveMode()
    {
        ground = false;
        rb.velocity = Vector3.Reflect(savedVelocity, savedContactNormal);
    }
}