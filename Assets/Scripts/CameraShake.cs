using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeTime;
    private float shakeMagnitude;

    void LateUpdate()
    {
        // Always track the camera’s *current* baseline (because camera is moving)
        Vector3 basePos = transform.localPosition;

        if (shakeTime > 0f)
        {
            transform.localPosition = basePos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTime -= Time.deltaTime;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeTime = duration;
        shakeMagnitude = magnitude;
    }
}